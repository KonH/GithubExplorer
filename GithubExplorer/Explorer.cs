using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Octokit;

namespace GithubExplorer {
	public sealed class Explorer {
		readonly ILogger      _logger;
		readonly GitHubClient _client;

		public Explorer(ILogger<Explorer> logger) {
			_logger = logger;
			_client = new GitHubClient(new ProductHeaderValue("KonH-GithubExplorer"));
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
			_logger.LogInformation($"Found {result.Count} pull requests for '{userName}'");
			return result;
		}
	}
}