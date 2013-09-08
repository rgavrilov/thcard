using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace THCard.Web {
	public static class AssetManager {
		private static readonly ConcurrentBag<string> _scripts = new ConcurrentBag<string>();

		public static IReadOnlyCollection<string> RegisteredStyles {
			get { return _scripts.ToList(); }
		}

		public static void RequireStyle(string path) {
			_scripts.Add(path);
		}
	}
}