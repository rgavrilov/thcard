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

		private readonly Username _username = new Username("username");
		private AccountControllerTempData _tempData;
		private MockAccountRegistrationService _accountRegistrationService;
		private AccountController _controller;

		public static string S {
			get { return Guid.NewGuid().ToString(); }
		}

		[Test]
		public void CreatseNewAccount() {
			_accountRegistrationService.ExpectAccountCreated(_username);
			_controller.SignUp(new AccountRegistrationInformation {Username = _username.ToString()});

		}

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
			Assert.That(_tempData.RegistrationFailureReason.Stored, "Failure reason was not stored in temp data.");
			Assert.That(_tempData.RegistrationFailureReason.Get(), Is.EqualTo(RegistrationFailureReason.UsernameNotAvailable));
		}

		[Test]
		public void StoresEnteredDataInTempData_OnRedirectOnFailure() {
			_accountRegistrationService.GivenUsernameIsUnavailable(_username);

			var orgRegistration = new AccountRegistrationInformation {
				Email = S,
				EmailConfirmation = S,
				Name = S,
				Password = S,
				PasswordConfirmation = S,
				Username = S
			};
			_controller.SignUp(orgRegistration);
			Assert.That(_tempData.AccountRegistrationInformation.Get().Email, Is.EqualTo(orgRegistration.Email));
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

		public void ExpectAccountCreated(Username username) {
			_mock.Setup(it=>it.CreateAccount(new AccountRegistration(username, )))
		}
	}
}