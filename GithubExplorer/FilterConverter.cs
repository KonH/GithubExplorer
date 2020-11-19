using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GithubExplorer {
	internal sealed class FilterConverter<T> {
		readonly PropertyInfo[] _properties;

		public FilterConverter(IEnumerable<PropertyInfo> properties) {
			_properties = properties.ToArray();
		}

		public Dictionary<string, object?>? Filter(T source) {
			if ( source == null ) {
				return null;
			}
			return _properties
				.ToDictionary(p => p.Name, p => p.GetValue(source));
		}
	}
}