using THCard.AccountManagement;
using THCard.AccountManagement.Dal;
using THCard.Web.Controllers.LoginControl.Models;
using THCard.Web.Infrastructure;
using Account = THCard.AccountManagement.Account;
using User = THCard.AccountManagement.User;

namespace THCard.Web.Controllers.LoginControl {
	[PresenterFor(typeof (LoginControlController))]
	public sealed class LoginControlPresenter {
		private readonly IUserRepository _userRepository;

		public LoginControlPresenter(IUserRepository userRepository) {
			_userRepository = userRepository;
		}

		public AuthenticatedLoginControlViewModel Authenticated(Account account) {
			User user = _userRepository.GetUser(account.UserId);
			var viewModel = new AuthenticatedLoginControlViewModel(user.FullName);
			return viewModel;
		}
	}
}