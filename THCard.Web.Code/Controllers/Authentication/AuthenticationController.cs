using System;
using System.Web.Mvc;
using THCard.AccountManagement;
using THCard.Web.Controllers.Authentication.Models;
using THCard.Web.Infrastructure;
using ControllerBase = THCard.Web.Infrastructure.ControllerBase;

namespace THCard.Web.Controllers.Authentication {
	public class AuthenticationController : ControllerBase {
		private readonly ISession _session;
		private readonly ISiteMap _siteMap;
		private readonly IUserAuthenticationService _userAuthenticationService;

		public AuthenticationController(ISiteMap siteMap, ISession session, IUserAuthenticationService userAuthenticationService) {
			_siteMap = siteMap;
			_session = session;
			_userAuthenticationService = userAuthenticationService;
		}

		private new AuthenticationTempData TempData {
			get { return new AuthenticationTempData(base.TempData); }
		}

		[HttpGet]
		public ActionResult Login() {
			TempData.LoginReturnPage.Keep();
			return Result(TempData.ErrorMessages.Get() ?? ErrorMessages.Empty);
		}

		[HttpPost]
		public ActionResult Login(string username, string password) {
			if (!_session.IsAuthenticated) {
				LoginAttemptResult loginAttemptResult = _userAuthenticationService.Authenticate(new Username(username),
				                                                                                new Password(password));
				if (!loginAttemptResult.Succeeded) {
					var errorMessages = new ErrorMessages {GetLoginFailureMessage(loginAttemptResult)};
					TempData.ErrorMessages.Store(errorMessages);
					TempData.LoginReturnPage.Keep();
					return RedirectToRoute(_siteMap.LoginPage);
				}
				_session.BeginAuthenticatedSession(loginAttemptResult.Account);
			}
			return RedirectToRoute(TempData.LoginReturnPage.Get() ?? _siteMap.GetLandingPage(_session.AuthenticatedAccount));
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

		[HttpPost]
		public ActionResult Logout() {
			_session.EndAuthenticatedSession();
			return RedirectToRoute(_siteMap.PublicLandingPage);
		}
	}
}