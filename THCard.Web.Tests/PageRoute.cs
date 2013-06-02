using System;
using System.Collections.Generic;
using System.Web.Routing;

namespace THCard.Web.Tests {
	public sealed class PageRoute : RouteValueDictionary {

		public const string PageKey = "Page";

		public PageRoute(string page) : base(new Dictionary<string, object> {{PageKey, page}}) {
			Page = page;
		}

		public string Page { get; private set; }

		public class PageRouteEqualityComparer : IEqualityComparer<RouteValueDictionary> {
			public bool Equals(RouteValueDictionary x, RouteValueDictionary y) {
				if (object.ReferenceEquals(x, y)) return true;
				if (x == null || y == null) return false;
				object xPageAsObject;
				object yPageAsObject;
				return
					x.TryGetValue(PageRoute.PageKey, out xPageAsObject) &&
					y.TryGetValue(PageRoute.PageKey, out yPageAsObject) &&
					object.Equals(xPageAsObject, yPageAsObject);
			}

			public int GetHashCode(RouteValueDictionary obj) {
				throw new NotImplementedException();
			}
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(this, obj)) {
				return true;
			}
			var otherRouteValues = obj as RouteValueDictionary;
			if (otherRouteValues != null) {
				object pageAsObject;
				return otherRouteValues.TryGetValue(PageKey, out pageAsObject) &&
				       (pageAsObject).Equals(Page);
			}
			return false;
		}
	}
}