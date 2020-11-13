using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GithubExplorer {
	public static class Startup {
		static void ConfigureServices(IServiceCollection services) {
			services.AddLogging(c => c.AddConsole());

			services.AddScoped<Explorer>();
			services.AddScoped<Serializer>();
			services.AddScoped<Writer>();

			services.AddScoped<RepositoriesUseCase>();
		}

		public static IServiceProvider Build() {
			var services = new ServiceCollection();
			ConfigureServices(services);
			return services.BuildServiceProvider();
		}
	}
}