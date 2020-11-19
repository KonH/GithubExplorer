using System.Linq;
using System.Threading.Tasks;

namespace GithubExplorer {
	public sealed class RepositoriesUseCase {
		readonly Explorer   _explorer;
		readonly Serializer _serializer;
		readonly Writer     _writer;

		public RepositoriesUseCase(Explorer explorer, Serializer serializer, Writer writer) {
			_explorer   = explorer;
			_serializer = serializer;
			_writer     = writer;
		}

		public async Task Handle(string userName, string output, string filter) {
			var repositories = await _explorer.GetRepositories(userName);
			var data         = _serializer.Serialize(repositories.ToArray(), filter);
			_writer.Write(output, data);
		}
	}
}