using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Octokit;

namespace GithubExplorer {
	public sealed class Explorer {
		readonly ILogger              _logger;
		readonly GitHubClient         _client;
		readonly RetryRateLimitRunner _retryRunner;

		public Explorer(ILogger<Explorer> logger, RetryRateLimitRunner retryRunner) {
			_logger      = logger;
			_retryRunner = retryRunner;
			_client      = new GitHubClient(new ProductHeaderValue("KonH-GithubExplorer"));
		}

		async Task<User> GetUser(string userName) {
			_logger.LogInformation($"Retrieving user information for '{userName}'");
			return await _client.User.Get(userName);
		}

		public async Task<IEnumerable<Repository>> GetRepositories(string userName) {
			var user = await GetUser(userName);
			_logger.LogInformation($"Retrieving repositories for '{userName}'");
			var result = await _client.Repository.GetAllForUser(user.Login);
			_logger.LogInformation($"Found {result.Count} repositories for '{userName}'");
			return result;
		}

		public async Task<IReadOnlyList<Issue>> GetPullRequests(string userName) {
			var request = new SearchIssuesRequest {
				Type   = IssueTypeQualifier.PullRequest,
				Author = userName
			};
			var result = (await _client.Search.SearchIssues(request)).Items;
			await TryEnrich(result);
			_logger.LogInformation($"Found {result.Count} pull requests for '{userName}'");
			return result;
		}

		async Task TryEnrich(IReadOnlyList<Issue> issues) {
			var prop = typeof(Issue).GetProperty(nameof(Issue.PullRequest));
			if ( prop == null ) {
				return;
			}
			foreach ( var issue in issues ) {
				// https://api.github.com/repos/{owner}/{name}/issues/{number}
				var url        = issue.HtmlUrl;
				var parts      = url.Split('/');
				var owner      = parts[^4];
				var name       = parts[^3];
				var repository = await _retryRunner.Run(() => _client.Repository.Get(owner, name));
				var pr         = await _retryRunner.Run(() => _client.PullRequest.Get(repository.Id, issue.Number));
				await Task.Delay(TimeSpan.FromSeconds(3));
				prop.SetValue(issue, pr);
			}
		}
	}
}