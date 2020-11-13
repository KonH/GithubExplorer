using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace GithubExplorer.IntegrationTests {
	public sealed class WriterTests {
		const string FilePath = "temp.txt";

		[TearDown]
		public void TearDown() {
			if ( File.Exists(FilePath) ) {
				File.Delete(FilePath);
			}
		}

		[Test]
		public void IsFileSaved() {
			var expectedString = "test";
			var writer = new Writer();

			writer.Write(FilePath, expectedString);

			File.Exists(FilePath).Should().BeTrue();
			File.ReadAllText(FilePath).Should().Contain(expectedString);
		}
	}
}