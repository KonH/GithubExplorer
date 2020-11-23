using System;

namespace GithubExplorer.IntegrationTests {
	public static class TestEnvironment {
		public static string? GetAccessToken() => Environment.GetEnvironmentVariable("GH_ACCESS_TOKEN");
	}
}