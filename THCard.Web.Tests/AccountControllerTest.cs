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
			_accountRegistrationService = new MockAccountRegistrationService();
			_controller = new AccountController(_accountRegistrationService, _tempData);
		}

		private static readonly Username _username = new Username("username");
		private AccountControllerTempData _tempData;
		private MockAccountRegistrationService _accountRegistrationService;
		private AccountController _controller;

		[Test]
		public void RedirectsToSignUpCompletePage_OnSuccess() {
			_accountRegistrationService.GivenUsernameIsAvailable(_username);

			ActionResult actionResult = _controller.SignUp(new AccountRegistrationInformation {Username = _username.ToString()});

			AssertRedirectedTo(actionResult, (AccountController c) => c.SignUpComplete());
		}

		[Test]
		public void ReturnsError_IfUsernameIsTaken() {
			_accountRegistrationService.GivenUsernameIsUnavailable(_username);

			ActionResult actionResult = _controller.SignUp(new AccountRegistrationInformation {Username = _username.ToString()});

			AssertRedirectedTo(actionResult, (AccountController c) => c.SignUp());
			Assert.That(_tempData.ErrorMessages.Stored && _tempData.ErrorMessages.Get().Any());
		}
	}

	public sealed class MockAccountRegistrationService : IAccountRegistrationService {
		private readonly Mock<IAccountRegistrationService> _mock;

		public MockAccountRegistrationService() {
			_mock = new Mock<IAccountRegistrationService>();
		}

		public bool IsUsernameAvailable(Username username) {
			return _mock.Object.IsUsernameAvailable(username);
		}

		public void GivenUsernameIsAvailable(Username username) {
			_mock.Setup(it => it.IsUsernameAvailable(username)).Returns(true);
		}

		public void GivenUsernameIsUnavailable(Username username) {
			_mock.Setup(it => it.IsUsernameAvailable(username)).Returns(false);
		}
	}
}