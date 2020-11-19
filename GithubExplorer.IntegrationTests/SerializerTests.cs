using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Octokit;

namespace GithubExplorer.IntegrationTests {
	public sealed class SerializerTests {
		[Test]
		public void IsRepositoriesSerializedToJson() {
			var expectedId = long.MaxValue;
			var repositories = new List<Repository> {
				new Repository(expectedId)
			};
			var serializer = new Serializer();

			var json = serializer.Serialize(repositories);

			json.Should().Contain(expectedId.ToString());
		}
	}
}