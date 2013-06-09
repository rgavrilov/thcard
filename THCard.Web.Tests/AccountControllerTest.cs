using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using THCard.AccountManagement;
using THCard.Web.Controllers.Account;
using THCard.Web.Controllers.Account.Models;
using THCard.Web.Controllers.Authentication;
using THCard.Web.Infrastructure;
using AccountRegistration = THCard.Web.Controllers.Account.Models.AccountRegistration;

namespace THCard.Web.Tests {
	[TestFixture]
	internal class AccountControllerTest : ControllerTestBase {
		[SetUp]
		public void SetUp() {
			_accountRegistrationService = new MockAccountRegistrationService();
			_siteMap = new MockSiteMap();
			_controller = new AccountController(_accountRegistrationService, _siteMap);
			_tempData = new AccountControllerTempData(_controller.TempData);
		}

		private MockAccountRegistrationService _accountRegistrationService;
		private AccountController _controller;

		private static readonly FullName _bwilsonFullName = new FullName(new Name("Wilson"), new GivenNames("Bob"));
		private static readonly Password _password = new Password("password");
		private static readonly Username _bwilsonUsername = new Username("username");
		private static readonly EmailAddress _bwilsonEmail = new EmailAddress("bwilson@wall.com");
		private AccountControllerTempData _tempData;
		private MockSiteMap _siteMap;

		[Test]
		public void CreatesNewAccount() {
			_accountRegistrationService.GivenUsernameIsAvailable(_bwilsonUsername);

			var expectedAccountRegistration = new AccountManagement.AccountRegistration(_bwilsonUsername, _password, _bwilsonFullName, _bwilsonEmail);
			_accountRegistrationService.ExpectCreateAccount(expectedAccountRegistration);

			_controller.SignUp(TestAccountRegistration);

			_accountRegistrationService.Verify();
		}

		private AccountRegistration TestAccountRegistration {
			get {
				return new AccountRegistration {
					Username = _bwilsonUsername.ToString(),
					Password = _password.ToString(),
					PasswordConfirmation = _password.ToString(),
					Email = _bwilsonEmail.ToString(),
					EmailConfirmation = _bwilsonEmail.ToString(),
					FirstName = _bwilsonFullName.FirstName.ToString(),
					LastName = _bwilsonFullName.FamilyName.ToString()
				};
			}
		}

		[Test]
		public void RedirectsBackToSignUpPageWithError_WhenPasswordsDoNotMatch_OnSignUp() {
			_accountRegistrationService.GivenUsernameIsAvailable(_bwilsonUsername);

			AccountRegistration accountRegistration = TestAccountRegistration;
			accountRegistration.PasswordConfirmation = "NOT" + accountRegistration.Password;
			ActionResult result = _controller.SignUp(accountRegistration);

			AssertRedirectedTo<AccountController>(result, c => c.SignUp());
			_tempData.RegistrationFailureReason.AssertEqual(RegistrationFailureReason.PasswordsDoNotMatch);
		}

		[Test]
		public void RedirectsBackToSignUpPageWithError_WhenEmailDoesNotMatch_OnSignUp() {
			_accountRegistrationService.GivenUsernameIsAvailable(_bwilsonUsername);

			var accountRegistration = TestAccountRegistration;
			accountRegistration.EmailConfirmation = "NOT" + accountRegistration.Email;
			ActionResult result = _controller.SignUp(accountRegistration);

			AssertRedirectedTo<AccountController>(result, c => c.SignUp());
			_tempData.RegistrationFailureReason.AssertEqual(RegistrationFailureReason.EmailsDoNotMatch);
		}

		[Test]
		public void RedirectsToLoginPage_WhenSignUpCompletePageIsRefreshed() {
			Assert.That(!_tempData.NewAccountUsername.Stored);
			ActionResult result = _controller.SignUpComplete();
			AssertRedirectedTo<AuthenticationController>(result, c => c.Login());
		}

		[Test]
		public void RedirectsToSignUpCompletePage_OnSuccess() {
			_accountRegistrationService.GivenUsernameIsAvailable(_bwilsonUsername);

			ActionResult actionResult =
				_controller.SignUp(new AccountRegistration {Username = _bwilsonUsername.ToString()});

			AssertRedirectedTo(actionResult, (AccountController c) => c.SignUpComplete());
		}

		[Test]
		public void ReturnsError_IfUsernameIsTaken() {
			_accountRegistrationService.GivenUsernameIsUnavailable(_bwilsonUsername);

			ActionResult actionResult =
				_controller.SignUp(new AccountRegistration {Username = _bwilsonUsername.ToString()});

			AssertRedirectedTo(actionResult, (AccountController c) => c.SignUp());
			Assert.That(_tempData.RegistrationFailureReason.Stored, "Failure reason was not stored in temp data.");
			Assert.That(_tempData.RegistrationFailureReason.Get(), Is.EqualTo(RegistrationFailureReason.UsernameNotAvailable));
		}

		[Test]
		public void StoresEnteredDataInTempData_OnRedirectOnFailure() {
			_accountRegistrationService.GivenUsernameIsUnavailable(_bwilsonUsername);

			var orgRegistration = new AccountRegistration {
				Email = S,
				EmailConfirmation = S,
				FirstName = S,
				LastName = S,
				Password = S,
				PasswordConfirmation = S,
				Username = S
			};
			_controller.SignUp(orgRegistration);
			Assert.That(_tempData.AccountRegistrationInformation.Get().Email, Is.EqualTo(orgRegistration.Email));
		}
	}

	public static class TempDataExtensions {
		public static void AssertEqual<T>(this TempDataValue<T> value, T expected) {
			if (!value.Stored) {
				Assert.Fail("TempData does not contain '{0}'.", value.Key);
			}
			Assert.That(value.Get(), Is.EqualTo(expected), "TempData[{0}]", value.Key);
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

		public void CreateAccount(AccountManagement.AccountRegistration accountRegistration) {
			_mock.Object.CreateAccount(accountRegistration);
		}


		public void GivenUsernameIsAvailable(Username username) {
			_mock.Setup(it => it.IsUsernameAvailable(username)).Returns(true);
		}

		public void GivenUsernameIsUnavailable(Username username) {
			_mock.Setup(it => it.IsUsernameAvailable(username)).Returns(false);
		}

		public void ExpectCreateAccount(AccountManagement.AccountRegistration accountRegistration) {
			_mock.Setup(
				it =>
				it.CreateAccount(
					It.Is<AccountManagement.AccountRegistration>(actual => AccountManagement.AccountRegistration.EqualityComparer.Equals(accountRegistration, actual))))
			     .Verifiable();
		}

		public void Verify() {
			_mock.Verify();
		}
	}
}