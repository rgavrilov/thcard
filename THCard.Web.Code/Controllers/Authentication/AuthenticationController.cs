using System;
using System.Web.Mvc;
using THCard.AccountManagement;
using THCard.Web.Controllers.Authentication.Models;
using ControllerBase = THCard.Web.Infrastructure.ControllerBase;

namespace THCard.Web.Controllers.Authentication {
	public class AuthenticationController : ControllerBase {
		private readonly ISession _session;
		private readonly ISiteMap _siteMap;
		private AuthenticationTempData _authTempData;

		public AuthenticationController(ISiteMap siteMap, ISession session, AuthenticationTempData authAuthTempData) {
			_siteMap = siteMap;
			_session = session;
			_authTempData = authAuthTempData;
		}

		private new AuthenticationTempData TempData {
			get { return _authTempData ?? (_authTempData = new AuthenticationTempData(base.TempData)); }
		}

		[HttpGet]
		public ActionResult Login() {
			TempData.LoginReturnPage.Keep();
			return Result(TempData.ErrorMessages.Get() ?? ErrorMessages.Empty);
		}

		[HttpPost]
		public ActionResult Login(string username, string password) {
			if (!_session.IsAuthenticated) {
				LoginAttemptResult loginAttemptResult = _session.Login(new Username(username), new Password(password));
				if (!loginAttemptResult.Succeeded) {
					var errorMessages = new ErrorMessages {GetLoginFailureMessage(loginAttemptResult)};
					TempData.ErrorMessages.Store(errorMessages);
					TempData.LoginReturnPage.Keep();
					return RedirectToRoute(_siteMap.GetLoginPage());
				}
			}
			return RedirectToRoute(TempData.LoginReturnPage ?? _siteMap.GetLandingPage(_session.AuthenticatedAccount));
		}

		private string GetLoginFailureMessage(LoginAttemptResult loginAttemptResult) {
			switch (loginAttemptResult.FailureReason) {
				case LoginFailureReason.AccountLockedOut:
					return "Account is locked out.";
				case LoginFailureReason.AccountNotActivated:
					return "Account has not been activated.";
				case LoginFailureReason.IncorrectPassword:
					return "Username or password is invalid.";
				case LoginFailureReason.UsernameNotFound:
					return "Unknown username.";
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public ActionResult Logout() {
			if (_session.IsAuthenticated) {
				_session.Logout();
			}
			return RedirectToRoute(_siteMap.GetPublicLandingPage());
		}
	}
}