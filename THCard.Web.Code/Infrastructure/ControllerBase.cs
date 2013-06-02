using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using JetBrains.Annotations;
using Ninject;

namespace THCard.Web.Infrastructure {
	public class ControllerBase : Controller {
		[Inject]
		public IPresenterFactory PresenterFactory { get; set; }

		protected virtual RedirectResult Redirect(Uri url) {
			return Redirect(url.ToString());
		}

		protected ActionResult RedirectToAction<TController>(Expression<Action<TController>> action) where TController : Controller {
			return MvcContrib.ControllerExtensions.RedirectToAction(this, action);
		}

		[AspMvcView]
		public ActionResult Result<T>(T data) {
			return new ViewPresenterResult<T>(data, PresenterFactory) {
				ViewName = null,
				MasterName = null,
				ViewData = ViewData,
				TempData = TempData,
				ViewEngineCollection = ViewEngineCollection
			};
		}


		public ActionResult Result<T>([AspMvcView] string viewName, T data) {
			return new ViewPresenterResult<T>(data, PresenterFactory) {
				ViewName = viewName,
				MasterName = null,
				ViewData = ViewData,
				TempData = TempData,
				ViewEngineCollection = ViewEngineCollection
			};
		}
	}
}