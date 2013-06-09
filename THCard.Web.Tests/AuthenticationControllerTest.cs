using System;
using System.Collections.Generic;
using System.Linq;
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
			_mockSiteMap = new MockSiteMap();
			_session = new Mock<ISession>();
			_createdAccount = new Account(new AccountId(Guid.NewGuid()), _username, new AccountRoles());
			_authService = new Mock<IUserAuthenticationService>();
		}

		private readonly Username _username = new Username("username");
		private readonly Password _password = new Password("password");
		private MockSiteMap _mockSiteMap;
		private Mock<ISession> _session;
		private Account _createdAccount;
		private Mock<IUserAuthenticationService> _authService;

		[Test]
		public void RedirectsToLoginPage_WhenLoginFails() {
			_session.SetupGet(it => it.IsAuthenticated).Returns(false);
			_authService.Setup(it => it.Authenticate(_username, _password)).Returns(LoginAttemptResult.UsernameNotFound());

			var controller = new AuthenticationController(_mockSiteMap, _session.Object, _authService.Object);

			ActionResult actionResult = controller.Login(_username.ToString(), _password.ToString());

			AssertRedirectedTo(actionResult, _mockSiteMap.LoginPage);
			var authTempData = new AuthenticationTempData(controller.TempData);
			Assert.That(authTempData.ErrorMessages.Get(), Is.Not.Empty);
		}

		[Test]
		public void RedirectsToLoginReturnPageInTempData_WhenLoginSucceeds() {
			_session.SetupGet(it => it.IsAuthenticated).Returns(false);
			_authService.Setup(it => it.Authenticate(_username, _password)).Returns(LoginAttemptResult.Success(_createdAccount));
			var loginRedirectPage = new PageRoute("LoginRedirectPage");

			var controller = new AuthenticationController(_mockSiteMap, _session.Object, _authService.Object);
			new AuthenticationTempData(controller.TempData).LoginReturnPage.Store(loginRedirectPage);

			ActionResult actionResult = controller.Login(_username.ToString(), _password.ToString());

			AssertRedirectedTo(actionResult, loginRedirectPage);
		}
	}
}