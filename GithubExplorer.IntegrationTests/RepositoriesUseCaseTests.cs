using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace GithubExplorer.IntegrationTests {
	public sealed class RepositoriesUseCaseTests {
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
			var useCase  = services.GetRequiredService<RepositoriesUseCase>();

			await useCase.Handle(TestEnvironment.GetAccessToken(), UserName, FilePath, "", null);

			var json = await File.ReadAllTextAsync(FilePath);
			json.Should().Contain("konh.github.io");
		}
	}
}