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
			_sessionMock = new Mock<ISession>();
			_authAuthTempData = new AuthenticationTempData(new TempDataDictionary());
			_account = new Account(new AccountId(Guid.NewGuid()), _username, new UserId(Guid.NewGuid()), new AccountRoles());
		}

		private readonly Username _username = new Username("username");
		private readonly Password _password = new Password("password");
		private SiteMapMock _siteMap;
		private Mock<ISession> _sessionMock;
		private AuthenticationTempData _authAuthTempData;
		private Account _account;

		private static void AssertRedirectedTo(ActionResult actionResult, PageRoute expectedPage) {
			Assert.That(actionResult, Is.Not.Null);
			Assert.That(actionResult, Is.InstanceOf<RedirectToRouteResult>());
			var redirectResult = (RedirectToRouteResult) actionResult;
			Assert.That(redirectResult.RouteValues, Is.EqualTo(expectedPage).Using(new PageRoute.PageRouteEqualityComparer()));
		}

		[Test]
		public void RedirectsToLoginPage_WhenLoginFails() {
			_sessionMock.SetupGet(it => it.IsAuthenticated).Returns(false);
			_sessionMock.Setup(it => it.Login(_username, _password)).Returns(LoginAttemptResult.UsernameNotFound());

			var controller = new AuthenticationController(_siteMap.Object, _sessionMock.Object, _authAuthTempData);

			ActionResult actionResult = controller.Login(_username.ToString(), _password.ToString());

			AssertRedirectedTo(actionResult, _siteMap.LoginPage);
			Assert.That(_authAuthTempData.ErrorMessages.Get(), Is.Not.Empty);
		}

		[Test]
		public void RedirectsToLoginReturnPageInTempData_WhenLoginSucceeds() {
			_sessionMock.SetupGet(it => it.IsAuthenticated).Returns(false);
			_sessionMock.Setup(it => it.Login(_username, _password)).Returns(LoginAttemptResult.Success(_account));
			var loginRedirectPage = new PageRoute("LoginRedirectPage");
			_authAuthTempData.LoginReturnPage.Store(loginRedirectPage);

			var controller = new AuthenticationController(_siteMap.Object, _sessionMock.Object, _authAuthTempData);

			ActionResult actionResult = controller.Login(_username.ToString(), _password.ToString());

			AssertRedirectedTo(actionResult, loginRedirectPage);
		}
	}
}