using System;
using System.Linq;

namespace THCard.Web.Infrastructure {
	public class PresenterForAttribute : Attribute {
		public PresenterForAttribute(Type controllerType) {
			ControllerType = controllerType;
		}

		public Type ControllerType { get; private set; }

		public static Type GetControllerType(Type presenterType) {
			Type controllerType =
				presenterType
					.GetCustomAttributes(typeof (PresenterForAttribute), true)
					.Cast<PresenterForAttribute>()
					.Select(a => a.ControllerType)
					.FirstOrDefault();
			return controllerType;
		}
	}
}