using System.IO;
using Microsoft.Extensions.Logging;

namespace GithubExplorer {
	public sealed class Writer {
		readonly ILogger _logger;

		public Writer(ILogger<Writer> logger) {
			_logger = logger;
		}

		public void Write(string path, string content) {
			File.WriteAllText(path, content);
			_logger.LogInformation($"Result saved into '{path}'");
		}
	}
}