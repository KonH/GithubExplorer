using System.Collections.Generic;
using System.Linq;
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

			var json = serializer.Serialize(repositories, "");

			json.Should().Contain(expectedId.ToString());
		}

		sealed class TestClass {
			public string? FirstValue  { get; set; }
			public string? SecondValue { get; set; }
		}

		[Test]
		public void IsFilteredContainRequiredProperties() {
			var collection = Enumerable.Range(0, 3)
				.Select(i => new TestClass {
					FirstValue  = $"wanted_1_{i}",
					SecondValue = $"wanted_2_{i}"
				})
				.ToArray();
			var serializer = new Serializer();

			var json = serializer.Serialize(collection, "FirstValue;SecondValue");

			json.Should().Contain("wanted_1_").And.Contain("wanted_2_");
		}

		[Test]
		public void IsFilteredDoesNotContainOtherProperties() {
			var collection = Enumerable.Range(0, 3)
				.Select(i => new TestClass {
					FirstValue  = $"wanted_{i}",
					SecondValue = $"skipped_{i}"
				})
				.ToArray();
			var serializer = new Serializer();

			var json = serializer.Serialize(collection, "FirstValue");

			json.Should().Contain("wanted_").And.NotContain("skipped_");
		}
	}
}