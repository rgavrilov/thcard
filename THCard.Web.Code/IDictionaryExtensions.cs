using System.Collections.Generic;

namespace THCard.Web {
	public static class IDictionaryExtensions {
		public static T GetValueOrDefault<T>(this IDictionary<string, object> dictionary, string key, T defaultValue) {
			T valueAsObject;
			if (dictionary.TryGetValue(key, out valueAsObject)) {
				return valueAsObject;
			}
			else {
				return defaultValue;
			}
		}

		public static bool TryGetValue<T>(this IDictionary<string, object> dictionary, string key, out T value) {
			object valueAsObject;
			if (dictionary.TryGetValue(key, out valueAsObject) && valueAsObject is T) {
				value = (T) valueAsObject;
				return true;
			}
			else {
				value = default(T);
				return false;
			}
		}
	}
}