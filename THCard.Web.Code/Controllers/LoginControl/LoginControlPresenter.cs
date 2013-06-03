using THCard.AccountManagement;
using THCard.Web.Controllers.LoginControl.Models;
using THCard.Web.Infrastructure;

namespace THCard.Web.Controllers.LoginControl {
	[PresenterFor(typeof (LoginControlController))]
	public sealed class LoginControlPresenter {
		private readonly ISiteMap _siteMap;
		private readonly IUserRepository _userRepository;

		public LoginControlPresenter(IUserRepository userRepository, ISiteMap siteMap) {
			_userRepository = userRepository;
			_siteMap = siteMap;
		}

		public AuthenticatedLoginControlViewModel Authenticated(AccountManagement.Account account) {
			User user = _userRepository.GetUser(account.UserId);
			var viewModel = new AuthenticatedLoginControlViewModel(user.FullName, _siteMap.GetLandingPage(account));
			return viewModel;
		}
	}
}