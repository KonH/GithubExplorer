using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace GithubExplorer {
	public sealed class Serializer {
		public string Serialize<T>(IReadOnlyCollection<T> source, string filter) =>
			JsonSerializer.Serialize(TryFilter(source, filter), new JsonSerializerOptions {
				WriteIndented = true
			});

		object TryFilter<T>(IReadOnlyCollection<T> source, string filter) {
			var converter = FilterConverter<T>.TryCreate(filter);
			if ( converter == null ) {
				return source;
			}
			return source
				.Select(s => TryFilter(s, converter));
		}

		object? TryFilter<T>(T source, FilterConverter<T> converter) => converter.Filter(source);
	}
}