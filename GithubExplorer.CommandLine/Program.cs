using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CommandLine;

namespace GithubExplorer.CommandLine {
	sealed class Program {
		[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
		sealed class Options {
			[Option('t', "target", HelpText = "Target to execute (available: 'repositories')", Required = true)]
			public string Target   { get; set; }
			[Option('u', "username", HelpText = "Related user", Required = true)]
			public string Username { get; set; }
			[Option('o', "output", HelpText = "Filepath to save results", Required = true)]
			public string Output   { get; set; }
			[Option('f', "format", HelpText = "Result format (only json supported for now)", Default = "json")]
			public string Format   { get; set; }
		}

		static async Task Main(string[] args) {
			await Parser.Default.ParseArguments<Options>(args)
				.WithParsedAsync(o => new Program(o).Execute());
		}

		readonly Dictionary<string, Func<Options, Task>> _targets = new Dictionary<string, Func<Options, Task>> {
			["repositories"] = o => {
				var useCase = new RepositoriesUseCase();
				return useCase.Handle(o.Username, o.Format, o.Output);
			}
		};

		readonly Options _options;

		Program(Options options) {
			_options = options;
		}

		async Task<int> Execute() {
			try {
				if ( !_targets.TryGetValue(_options.Target, out var target) ) {
					throw new ArgumentException("Provided target is not supported", nameof(target));
				}
				await target(_options);
				return 0;
			} catch ( Exception e ) {
				Console.WriteLine(e);
				return -1;
			}
		}
	}
}