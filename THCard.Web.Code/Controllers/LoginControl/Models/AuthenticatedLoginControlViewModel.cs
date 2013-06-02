using System.Web.Routing;
using THCard.AccountManagement;

namespace THCard.Web.Controllers.LoginControl.Models {
	public class AuthenticatedLoginControlViewModel {
		public AuthenticatedLoginControlViewModel(FullName userFullName, RouteValueDictionary dashboardPage) {
			UserFullName = userFullName;
			DashboardPage = dashboardPage;
		}

		public FullName UserFullName { get; private set; }
		public RouteValueDictionary DashboardPage { get; private set; }
	}
}