using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Octokit;

namespace GithubExplorer {
	public sealed class Explorer {
		readonly ILogger              _logger;
		readonly RetryRateLimitRunner _retryRunner;

		public Explorer(
			ILogger<Explorer> logger, RetryRateLimitRunner retryRunner) {
			_logger      = logger;
			_retryRunner = retryRunner;
		}

		async Task<User> GetUser(GitHubClient client, string userName) {
			_logger.LogInformation($"Retrieving user information for '{userName}'");
			return await client.User.Get(userName);
		}

		public async Task<IEnumerable<Repository>> GetRepositories(string? accessToken, string userName, int? maximumCount) {
			var client = CreateClient(accessToken);
			var user   = await GetUser(client, userName);
			_logger.LogInformation($"Retrieving repositories for '{userName}'");
			var result = await client.Repository.GetAllForUser(user.Login);
			if ( maximumCount != null ) {
				_logger.LogInformation($"Trim actual results {result.Count} to maximum {maximumCount}");
				result = result
					.Take(maximumCount.Value)
					.ToArray();
			}
			_logger.LogInformation($"Found {result.Count} repositories for '{userName}'");
			return result;
		}

		public async Task<IReadOnlyList<Issue>> GetPullRequests(string? accessToken, string userName, int? maximumCount) {
			var client = CreateClient(accessToken);
			var request = new SearchIssuesRequest {
				Type   = IssueTypeQualifier.PullRequest,
				Author = userName,
			};
			var result = (await client.Search.SearchIssues(request)).Items;
			if ( maximumCount != null ) {
				_logger.LogInformation($"Trim actual results {result.Count} to maximum {maximumCount}");
				result = result
					.Take(maximumCount.Value)
					.ToArray();
			}
			await TryEnrich(client, result);
			_logger.LogInformation($"Found {result.Count} pull requests for '{userName}'");
			return result;
		}

		async Task TryEnrich(GitHubClient client, IReadOnlyList<Issue> issues) {
			var prop = typeof(Issue).GetProperty(nameof(Issue.PullRequest));
			if ( prop == null ) {
				return;
			}
			_logger.LogInformation("Trying to enrich missing PR information");
			foreach ( var issue in issues ) {
				// https://api.github.com/repos/{owner}/{name}/issues/{number}
				var url        = issue.HtmlUrl;
				var parts      = url.Split('/');
				var owner      = parts[^4];
				var name       = parts[^3];
				var repository = await _retryRunner.Run(() => client.Repository.Get(owner, name));
				var pr         = await _retryRunner.Run(() => client.PullRequest.Get(repository.Id, issue.Number));
				prop.SetValue(issue, pr);
				_logger.LogInformation($"Issue '{url}' enriched with PR information");
			}
		}

		GitHubClient CreateClient(string? accessToken) {
			var client = new GitHubClient(new ProductHeaderValue("KonH-GithubExplorer"));
			if ( string.IsNullOrWhiteSpace(accessToken) ) {
				_logger.LogInformation("Authorizing anonymously");
			} else {
				_logger.LogInformation("Authorizing using access token");
				client.Credentials = new Credentials(accessToken);
			}
			return client;
		}
	}
}