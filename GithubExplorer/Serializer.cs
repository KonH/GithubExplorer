using System;
using System.Collections.Generic;
using System.Text.Json;

namespace GithubExplorer {
	public sealed class Serializer {
		readonly Dictionary<string, Func<object, string>> _serializers =
			new Dictionary<string, Func<object, string>> {
				["json"] = o => JsonSerializer.Serialize(o)
			};

		public string Serialize(object source, string format) =>
			_serializers.TryGetValue(format, out var serializer)
				? serializer(source)
				: throw new ArgumentException("Provided format is not supported", nameof(format));
	}
}