using System.Threading.Tasks;

namespace GithubExplorer {
	public sealed class RepositoriesUseCase {
		public async Task Handle(string userName, string format, string output) {
			var explorer = new Explorer();
			var repositories = await explorer.GetRepositories(userName);
			var serializer = new Serializer();
			var data = serializer.Serialize(repositories, format);
			var writer = new Writer();
			writer.Write(output, data);
		}
	}
}