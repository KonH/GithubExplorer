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

		async Task<User> GetUser(string userName) =>
			await _client.User.Get(userName);

		public async Task<IEnumerable<Repository>> GetRepositories(string userName) {
			_logger.LogInformation($"Retrieving user information for '{userName}'");
			var user = await GetUser(userName);
			_logger.LogInformation($"Retrieving repositories for '{userName}'");
			var result = await _client.Repository.GetAllForUser(user.Login);
			_logger.LogInformation($"Found {result.Count} repositories for '{userName}'");
			return result;
		}
	}
}