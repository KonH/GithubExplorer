using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace GithubExplorer.IntegrationTests {
	public sealed class PullRequestsUseCaseTests {
		const string UserName = "KonH";
		const string FilePath = "temp.json";

		[TearDown]
		public void TearDown() {
			if ( File.Exists(FilePath) ) {
				File.Delete(FilePath);
			}
		}

		[Test]
		public async Task IsResultFound() {
			var services = Startup.Build();
			var useCase  = services.GetRequiredService<PullRequestsUseCase>();

			// Filter is required, because of broken serialization (StringEnum members throw exceptions)
			await useCase.Handle(
				TestEnvironment.GetAccessToken(), UserName, FilePath, "HtmlUrl;Title;CreatedAt;PullRequest.Merged;", 10);

			var json = await File.ReadAllTextAsync(FilePath);
			Assert.Multiple(() => {
				json.Should().Contain("https://github.com/shouldly/shouldly/pull/681");
				json.Should().Contain("Adds DynamicShould.Throw method to cover corner case");
				json.Should().Contain("\"Merged\": true");
			});
		}
	}
}