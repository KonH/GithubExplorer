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
			var converter = PrepareConverter<T>(filter);
			if ( converter == null ) {
				return source;
			}
			return source
				.Select(s => TryFilter(s, converter));
		}

		object? TryFilter<T>(T source, FilterConverter<T> converter) => converter.Filter(source);

		FilterConverter<T>? PrepareConverter<T>(string filter) {
			if ( string.IsNullOrWhiteSpace(filter) ) {
				return null;
			}
			var parts = filter.Split(';');
			var type  = typeof(T);
			var properties = type.GetProperties()
				.Where(p => parts.Contains(p.Name));
			return new FilterConverter<T>(properties);
		}
	}
}