using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace GithubExplorer.IntegrationTests {
	public sealed class ExplorerTests {
		const string UserName = "KonH";

		[Test]
		public async Task IsRepositoriesFound() {
			var services = Startup.Build();
			var explorer = services.GetRequiredService<Explorer>();

			var repositories = (await explorer.GetRepositories(UserName)).ToArray();

			repositories.Should().NotBeEmpty();
			repositories.Should().Contain(r => r.Name == "konh.github.io");
		}
	}
}