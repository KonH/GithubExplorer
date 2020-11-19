using System.Text.Json;

namespace GithubExplorer {
	public sealed class Serializer {
		public string Serialize(object source) =>
			JsonSerializer.Serialize(source, new JsonSerializerOptions {
				WriteIndented = true
			});
	}
}