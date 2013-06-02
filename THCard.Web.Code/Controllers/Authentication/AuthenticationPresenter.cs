using THCard.Web.Controllers.Authentication.Models;
using THCard.Web.Infrastructure;

namespace THCard.Web.Controllers.Authentication {
	[PresenterFor(typeof (AuthenticationController))]
	public class AuthenticationPresenter {
		public LoginViewModel Login(ErrorMessages errorMessages) {
			return new LoginViewModel {
				ErrorMessages = errorMessages ?? ErrorMessages.Empty
			};
		}
	}
}