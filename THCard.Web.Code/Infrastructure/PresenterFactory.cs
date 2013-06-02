using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace THCard.Web.Infrastructure {
	public class PresenterFactory : IPresenterFactory {
		private static readonly ConcurrentDictionary<PresenterTarget, PresenterMethod> Presenters =
			new ConcurrentDictionary<PresenterTarget, PresenterMethod>();

		private readonly IPresenterActivator _presenterActivator;

		public PresenterFactory(IPresenterActivator presenterActivator) {
			_presenterActivator = presenterActivator;
		}

		public Func<object, object> GetPresenter(string viewName, Type controllerType, Type actionResultType) {
			var currentTarget = new PresenterTarget(viewName, controllerType, actionResultType);
			PresenterMethod presenterMethod;
			if (Presenters.TryGetValue(currentTarget, out presenterMethod)) {
				return viewResult => presenterMethod.Invoke(viewResult, _presenterActivator);
			}
			return IdentityPresenter;
		}

		public static void RegisterPresenter(Type presenterType) {
			Type controllerType = PresenterForAttribute.GetControllerType(presenterType);
			if (controllerType == null) {
				return;
			}
			IEnumerable<PresenterMethod> presenterMethods = PresenterMethod.GetMethods(presenterType);
			foreach (PresenterMethod presenterMethod in presenterMethods) {
				RegisterPresenterMethod(presenterMethod, controllerType);
			}
		}

		public static void RegisterPresenters() {
			Type[] types = Assembly.GetExecutingAssembly().GetTypes();
			foreach (Type type in types) {
				if (PresenterForAttribute.GetControllerType(type) != null) {
					RegisterPresenter(type);
				}
			}
		}

		private static void RegisterPresenterMethod(PresenterMethod presenterMethod, Type controllerType) {
			var target = new PresenterTarget(presenterMethod.ViewName, controllerType, presenterMethod.ActionResultType);
			Presenters.TryAdd(target, presenterMethod);
		}

		private static object IdentityPresenter(object actionResult) {
			object viewModel = actionResult;
			return viewModel;
		}
	}

	public interface IPresenterActivator {
		object CreateInstance(Type type);
	}

	public sealed class DelegatePresenterActivator : IPresenterActivator {
		private readonly Func<Type, object> _presenterInstanceCreator;

		public DelegatePresenterActivator(Func<Type, object> presenterInstanceCreator) {
			_presenterInstanceCreator = presenterInstanceCreator;
		}

		public object CreateInstance(Type type) {
			return _presenterInstanceCreator(type);
		}
	}
}