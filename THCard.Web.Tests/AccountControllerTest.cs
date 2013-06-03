using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using THCard.AccountManagement;
using THCard.Web.Controllers.Account;
using THCard.Web.Controllers.Account.Models;

namespace THCard.Web.Tests {
	[TestFixture]
	internal class AccountControllerTest : ControllerTestBase {
		[SetUp]
		public void SetUp() {
			_tempData = new AccountControllerTempData(new TempDataDictionary());
			_accountRegistrationService = new Mock<IAccountRegistrationService>();
		}

		private static readonly Username _username = new Username("username");
		private AccountControllerTempData _tempData;
		private Mock<IAccountRegistrationService> _accountRegistrationService;

		[Test]
		public void RedirectsToSignUpCompletePage_OnSuccess() {
			_accountRegistrationService.Setup(it => it.IsUsernameAvailable(_username)).Returns(true).Verifiable();

			var controller = new AccountController(_accountRegistrationService.Object, _tempData);
			ActionResult actionResult = controller.SignUp(new AccountRegistrationInformation {Username = _username.ToString()});

			AssertRedirectedTo<AccountController>(actionResult, c => c.SignUpComplete());
		}

		[Test]
		public void ReturnsError_IfUsernameIsTaken() {
			_accountRegistrationService.Setup(it => it.IsUsernameAvailable(_username)).Returns(false).Verifiable();

			var controller = new AccountController(_accountRegistrationService.Object, _tempData);
			ActionResult actionResult = controller.SignUp(new AccountRegistrationInformation {Username = _username.ToString()});
			
			AssertRedirectedTo<AccountController>(actionResult, c => c.SignUp());
			Assert.That(_tempData.ErrorMessages.Stored && _tempData.ErrorMessages.Get().Any());
		}
	}
}