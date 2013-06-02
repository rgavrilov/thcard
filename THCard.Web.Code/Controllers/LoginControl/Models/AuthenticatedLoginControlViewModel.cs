using THCard.AccountManagement;

namespace THCard.Web.Controllers.LoginControl.Models {
	public class AuthenticatedLoginControlViewModel {
		public AuthenticatedLoginControlViewModel(FullName userFullName) {
			UserFullName = userFullName;
		}

		public FullName UserFullName { get; private set; }
	}
}