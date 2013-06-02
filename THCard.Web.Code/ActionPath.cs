using System;
using System.Web.Routing;

namespace THCard.Web {
	public sealed class ActionPath {
		public ActionPath(RouteValueDictionary routeValues) {
			AreaName = routeValues.GetValueOrDefault("area", "*");
			ControllerName = routeValues.GetValueOrDefault("controller", "*");
			ActionName = routeValues.GetValueOrDefault("action", "*");
		}

		public ActionPath(string areaName, string controllerName, string actionName) {
			AreaName = areaName;
			ControllerName = controllerName;
			ActionName = actionName;
		}

		public string AreaName { get; private set; }
		public string ControllerName { get; private set; }
		public string ActionName { get; private set; }

		public override string ToString() {
			return string.Format("{0}/{1}/{2}", AreaName, ControllerName, ActionName);
		}

		private static bool Equals(ActionPath left, ActionPath right) {
			if (object.ReferenceEquals(left, right)) return true;
			if (object.ReferenceEquals(left, null) || object.ReferenceEquals(right, null)) return false;
			return
				string.Equals(left.AreaName, right.AreaName, StringComparison.OrdinalIgnoreCase) &&
				string.Equals(left.ControllerName, right.ControllerName, StringComparison.OrdinalIgnoreCase) &&
				string.Equals(left.ActionName, right.ActionName, StringComparison.OrdinalIgnoreCase);
		}

		public override bool Equals(object obj) {
			return Equals(this, obj as ActionPath);
		}

		public static bool operator ==(ActionPath left, ActionPath right) {
			return Equals(left, right);
		}

		public static bool operator !=(ActionPath left, ActionPath right) {
			return !(left == right);
		}
	}
}