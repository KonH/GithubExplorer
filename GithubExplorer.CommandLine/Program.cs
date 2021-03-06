﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;

namespace GithubExplorer.CommandLine {
	sealed class Program {
		[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
		sealed class Options {
			[Option('a', "accessToken", HelpText = "Access token to use API", Required = false)]
			public string AccessToken { get; set; }
			[Option('t', "target", HelpText = "Target to execute (available: 'repositories')", Required = true)]
			public string Target   { get; set; }
			[Option('u', "username", HelpText = "Related user", Required = true)]
			public string Username { get; set; }
			[Option('o', "output", HelpText = "Filepath to save results", Required = true)]
			public string Output   { get; set; }
			[Option('f', "filter", HelpText = "Filter to select subset of properties from the result", Required = false)]
			public string Filter  { get; set; }
			[Option('m', "maximumCount", HelpText = "Maximum count of results (if required)", Required = false)]
			public string MaximumCountString { get; set; }

			public int? MaximumCount => int.TryParse(MaximumCountString, out var value) ? new int?(value) : null;
		}

		static async Task Main(string[] args) {
			await Parser.Default.ParseArguments<Options>(args)
				.WithParsedAsync(o => new Program(o).Execute());
		}

		readonly Dictionary<string, Func<IServiceProvider, Options, Task>> _targets =
			new Dictionary<string, Func<IServiceProvider, Options, Task>> {
				["repositories"] = (s, o) => {
					var useCase = s.GetRequiredService<RepositoriesUseCase>();
					return useCase.Handle(o.AccessToken, o.Username, o.Output, o.Filter, o.MaximumCount);
				},
				["pullrequests"] = (s, o) => {
					var useCase = s.GetRequiredService<PullRequestsUseCase>();
					return useCase.Handle(o.AccessToken, o.Username, o.Output, o.Filter, o.MaximumCount);
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
				var services = Startup.Build();
				await target(services, _options);
				return 0;
			} catch ( Exception e ) {
				Console.WriteLine(e);
				return -1;
			}
		}
	}
}