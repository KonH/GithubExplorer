using System;
using System.Collections.Generic;
using System.Text.Json;

namespace GithubExplorer {
	public sealed class Serializer {
		readonly Dictionary<string, Func<object, string>> _serializers =
			new Dictionary<string, Func<object, string>> {
				["json"] = o => JsonSerializer.Serialize(o)
			};

		public string Serialize(object source, string method) =>
			_serializers.TryGetValue(method, out var serializer)
				? serializer(source)
				: throw new ArgumentException("Provided method is not supported", nameof(method));
	}
}