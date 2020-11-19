using System.Threading.Tasks;

namespace GithubExplorer {
	public sealed class PullRequestsUseCase {
		readonly Explorer   _explorer;
		readonly Serializer _serializer;
		readonly Writer     _writer;

		public PullRequestsUseCase(Explorer explorer, Serializer serializer, Writer writer) {
			_explorer   = explorer;
			_serializer = serializer;
			_writer     = writer;
		}

		public async Task Handle(string userName, string output, string filter) {
			var issues = await _explorer.GetPullRequests(userName);
			var data   = _serializer.Serialize(issues, filter);
			_writer.Write(output, data);
		}
	}
}