using System.Linq;
using System.Reflection;

namespace GithubExplorer {
	internal sealed class PropertySource {
		readonly PropertyInfo[] _properties;

		public PropertySource(PropertyInfo[] properties) {
			_properties = properties;
		}

		public (string[] path, object? result) GetValue(object source) {
			var value     = source;
			var lastIndex = 0;
			for ( var i = 0; i < _properties.Length; i++ ) {
				var prop = _properties[i];
				value = prop.GetValue(value);
				if ( value == null ) {
					break;
				}
				lastIndex = i;
			}
			var path = _properties
				.Take(lastIndex + 1)
				.Select(p => p.Name)
				.ToArray();
			return (path, value);
		}
	}
}