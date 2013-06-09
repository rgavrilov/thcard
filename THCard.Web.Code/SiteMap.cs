using System.Web.Routing;
using Microsoft.Web.Mvc.Internal;
using THCard.AccountManagement;
using THCard.Web.Controllers.Account;
using THCard.Web.Controllers.Authentication;
using THCard.Web.Controllers.Home;
using THCard.Web.Controllers.Member;

namespace THCard.Web {
	public class SiteMap : ISiteMap {
		public RouteValueDictionary GetLandingPage(Account account) {
			if (account.IsInRole(ApplicationRoles.Member)) {
				return ExpressionHelper.GetRouteValuesFromExpression<MemberController>(c => c.Dashboard());
			}
			else {
				return PublicLandingPage;
			}
		}

		public RouteValueDictionary PublicLandingPage {
			get { return ExpressionHelper.GetRouteValuesFromExpression<HomeController>(c => c.Index()); }
		}

		public RouteValueDictionary LoginPage {
			get { return ExpressionHelper.GetRouteValuesFromExpression<AuthenticationController>(c => c.Login()); }
		}

		public RouteValueDictionary SignUpPage {
			get { return ExpressionHelper.GetRouteValuesFromExpression<AccountController>(c => c.SignUp()); }
		}
	}
}