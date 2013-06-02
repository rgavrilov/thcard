using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using THCard.Web.Controllers.Authentication;

namespace THCard.Web.Infrastructure {
	public class AuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute {
		
		[Inject]
		public ISiteMap SiteMap { get; set; }

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext) {
			var authenticationControllerTempData = new AuthenticationTempData(filterContext.Controller.TempData);
			RouteValueDictionary returnPage = filterContext.RouteData.Values;
			authenticationControllerTempData.LoginReturnPage.Store(returnPage);
			filterContext.Result = new RedirectToRouteResult(SiteMap.GetLoginPage());
		}
	}
}