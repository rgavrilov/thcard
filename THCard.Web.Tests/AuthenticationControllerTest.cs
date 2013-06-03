using System;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using THCard.AccountManagement;
using THCard.Web.Controllers.Authentication;

namespace THCard.Web.Tests {
	[TestFixture]
	public class AuthenticationControllerTest : ControllerTestBase {
		[SetUp]
		public void SetUpTest() {
			_siteMap = new SiteMapMock();
			_session = new Mock<ISession>();
			_authTempData = new AuthenticationTempData(new TempDataDictionary());
			_account = new Account(new AccountId(Guid.NewGuid()), _username, new UserId(Guid.NewGuid()), new AccountRoles());
			_authService = new Mock<IUserAuthenticationService>();
		}

		private readonly Username _username = new Username("username");
		private readonly Password _password = new Password("password");
		private SiteMapMock _siteMap;
		private Mock<ISession> _session;
		private AuthenticationTempData _authTempData;
		private Account _account;
		private Mock<IUserAuthenticationService> _authService;

		[Test]
		public void RedirectsToLoginPage_WhenLoginFails() {
			_session.SetupGet(it => it.IsAuthenticated).Returns(false);
			_authService.Setup(it => it.Authenticate(_username, _password)).Returns(LoginAttemptResult.UsernameNotFound());

			var controller = new AuthenticationController(_siteMap.Object, _session.Object, _authTempData, _authService.Object);

			ActionResult actionResult = controller.Login(_username.ToString(), _password.ToString());

			AssertRedirectedTo(actionResult, _siteMap.LoginPage);
			Assert.That(_authTempData.ErrorMessages.Get(), Is.Not.Empty);
		}

		[Test]
		public void RedirectsToLoginReturnPageInTempData_WhenLoginSucceeds() {
			_session.SetupGet(it => it.IsAuthenticated).Returns(false);
			_authService.Setup(it => it.Authenticate(_username, _password)).Returns(LoginAttemptResult.Success(_account));
			var loginRedirectPage = new PageRoute("LoginRedirectPage");
			_authTempData.LoginReturnPage.Store(loginRedirectPage);

			var controller = new AuthenticationController(_siteMap.Object, _session.Object, _authTempData, _authService.Object);

			ActionResult actionResult = controller.Login(_username.ToString(), _password.ToString());

			AssertRedirectedTo(actionResult, loginRedirectPage);
		}
	}
}