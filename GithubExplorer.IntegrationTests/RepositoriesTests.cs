using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace GithubExplorer.IntegrationTests {
	public sealed class RepositoriesTests {
		const string UserName = "KonH";

		[Test]
		public async Task IsRepositoriesFound() {
			var explorer = new Explorer();

			var repositories = (await explorer.GetRepositories(UserName)).ToArray();

			repositories.Should().NotBeEmpty();
			repositories.Should().Contain(r => r.Name == "konh.github.io");
		}
	}
}