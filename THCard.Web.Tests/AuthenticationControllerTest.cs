using System;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using THCard.AccountManagement;
using THCard.Web.Controllers.Authentication;

namespace THCard.Web.Tests {
	[TestFixture]
	public class AuthenticationControllerTest {
		[SetUp]
		public void SetUpTest() {
			_siteMap = new SiteMapMock();
			_session = new Mock<ISession>();
			_authAuthTempData = new AuthenticationTempData(new TempDataDictionary());
			_account = new Account(new AccountId(Guid.NewGuid()), _username, new UserId(Guid.NewGuid()), new AccountRoles());
			_authService = new Mock<IUserAuthenticationService>();
		}

		private readonly Username _username = new Username("username");
		private readonly Password _password = new Password("password");
		private SiteMapMock _siteMap;
		private Mock<ISession> _session;
		private AuthenticationTempData _authAuthTempData;
		private Account _account;
		private Mock<IUserAuthenticationService> _authService;

		private static void AssertRedirectedTo(ActionResult actionResult, PageRoute expectedPage) {
			Assert.That(actionResult, Is.Not.Null);
			Assert.That(actionResult, Is.InstanceOf<RedirectToRouteResult>());
			var redirectResult = (RedirectToRouteResult) actionResult;
			Assert.That(redirectResult.RouteValues, Is.EqualTo(expectedPage).Using(new PageRoute.PageRouteEqualityComparer()));
		}

		[Test]
		public void RedirectsToLoginPage_WhenLoginFails() {
			_session.SetupGet(it => it.IsAuthenticated).Returns(false);
			_authService.Setup(it => it.Authenticate(_username, _password)).Returns(LoginAttemptResult.UsernameNotFound());

			var controller = new AuthenticationController(_siteMap.Object, _session.Object, _authAuthTempData, _authService.Object);

			ActionResult actionResult = controller.Login(_username.ToString(), _password.ToString());

			AssertRedirectedTo(actionResult, _siteMap.LoginPage);
			Assert.That(_authAuthTempData.ErrorMessages.Get(), Is.Not.Empty);
		}

		[Test]
		public void RedirectsToLoginReturnPageInTempData_WhenLoginSucceeds() {
			_session.SetupGet(it => it.IsAuthenticated).Returns(false);
			_authService.Setup(it => it.Authenticate(_username, _password)).Returns(LoginAttemptResult.Success(_account));
			var loginRedirectPage = new PageRoute("LoginRedirectPage");
			_authAuthTempData.LoginReturnPage.Store(loginRedirectPage);

			var controller = new AuthenticationController(_siteMap.Object, _session.Object, _authAuthTempData, _authService.Object);

			ActionResult actionResult = controller.Login(_username.ToString(), _password.ToString());

			AssertRedirectedTo(actionResult, loginRedirectPage);
		}
	}
}