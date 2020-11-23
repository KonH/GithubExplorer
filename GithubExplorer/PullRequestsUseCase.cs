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

		public async Task Handle(string userName, string output, string filter, int? maximumCount) {
			var issues = await _explorer.GetPullRequests(userName, maximumCount);
			var data   = _serializer.Serialize(issues, filter);
			_writer.Write(output, data);
		}
	}
}