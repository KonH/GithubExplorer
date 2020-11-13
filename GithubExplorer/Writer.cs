using System.IO;

namespace GithubExplorer {
	public sealed class Writer {
		public void Write(string path, string content) =>
			File.WriteAllText(path, content);
	}
}