using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GithubExplorer {
	internal sealed class FilterConverter<T> {
		readonly PropertySource[] _properties;

		FilterConverter(PropertySource[] properties) {
			_properties = properties;
		}

		public Dictionary<string, object?>? Filter(T source) {
			if ( source == null ) {
				return null;
			}
			var result = new Dictionary<string, object?>();
			foreach ( var src in _properties ) {
				var (path, r) = src.GetValue(source);
				var container = result;
				for ( var i = 0; i < path.Length; i++ ) {
					if ( !container.TryGetValue(path[i], out var value) ) {
						var isLast = i >= (path.Length - 1);
						value = isLast ? r : new Dictionary<string, object?>();
						container.Add(path[i], value);
						if ( isLast ) {
							break;
						}
					}
					container = (Dictionary<string, object?>) value!;
				}
			}
			return result;
		}

		public static FilterConverter<T>? TryCreate(string filter) {
			if ( string.IsNullOrWhiteSpace(filter) ) {
				return null;
			}
			var parts = filter
				.Split(';')
				.Select(p => p.Split('.'))
				.ToArray();
			var sources = parts
				.Select(ConstructSource)
				.ToArray();
			return new FilterConverter<T>(sources);
		}

		static PropertySource ConstructSource(string[] parts) {
			var props = new List<PropertyInfo>();
			foreach ( var p in parts ) {
				var type = (props.Count > 0) ? props[^1].PropertyType : typeof(T);
				var prop = type.GetProperty(p);
				if ( prop == null ) {
					break;
				}
				props.Add(prop);
			}
			return new PropertySource(props.ToArray());
		}
	}
}