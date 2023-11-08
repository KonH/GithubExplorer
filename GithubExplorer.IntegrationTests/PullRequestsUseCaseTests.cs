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

		[Test] // Pretty fragile test case, rework it in some future (huh)
		public async Task IsResultFound() {
			var services = Startup.Build();
			var useCase  = services.GetRequiredService<PullRequestsUseCase>();

			// Filter is required, because of broken serialization (StringEnum members throw exceptions)
			await useCase.Handle(
				TestEnvironment.GetAccessToken(), UserName, FilePath, "HtmlUrl;Title;CreatedAt;PullRequest.Merged;", 30);

			var json = await File.ReadAllTextAsync(FilePath);
			Assert.Multiple(() => {
				json.Should().Contain("https://github.com/microsoft/component-detection/pull/311");
				json.Should().Contain("Use extended log file names format to prevent collisions in concurrent builds");
				json.Should().Contain("\"Merged\": true");
			});
		}
	}
}