using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;

namespace GithubExplorer {
	public sealed class Explorer {
		readonly GitHubClient _client;

		public Explorer() {
			_client = new GitHubClient(new ProductHeaderValue("KonH-GithubExplorer"));
		}

		async Task<User> GetUser(string userName) =>
			await _client.User.Get(userName);

		public async Task<IEnumerable<string>> GetRepositories(string userName) {
			var user         = await GetUser(userName);
			var repositories = await _client.Repository.GetAllForUser(user.Login);
			return repositories
				.Select(r => r.Name);
		}
	}
}