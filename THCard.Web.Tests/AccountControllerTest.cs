using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using THCard.AccountManagement;
using THCard.Web.Controllers.Account;
using THCard.Web.Controllers.Authentication;
using THCard.Web.Infrastructure;
using AccountRegistration = THCard.Web.Controllers.Account.Models.AccountRegistration;

namespace THCard.Web.Tests {
	[TestFixture]
	internal class AccountControllerTest : ControllerTestBase {
		#region Tests
		[Test]
		public void CreatesNewAccount() {
			this._accountRegistrationService.GivenUsernameIsAvailable(_bwilsonUsername);

			var expectedAccountRegistration = new AccountManagement.AccountRegistration(_bwilsonUsername, _password, _bwilsonFullName, _bwilsonEmail);
			this._accountRegistrationService.ExpectCreateAccount(expectedAccountRegistration);

			this._controller.SignUp(TestAccountRegistration);

			this._accountRegistrationService.Verify();
		}

		[Test]
		public void RedirectsBackToSignUpPageWithError_WhenEmailDoesNotMatch_OnSignUp() {
			this._accountRegistrationService.GivenUsernameIsAvailable(_bwilsonUsername);

			AccountRegistration accountRegistration = TestAccountRegistration;
			accountRegistration.EmailConfirmation = "NOT" + accountRegistration.Email;
			ActionResult result = this._controller.SignUp(accountRegistration);

			AssertRedirectedTo<AccountController>(result, c => c.SignUp());
			this._tempData.RegistrationFailureReason.AssertEqual(RegistrationFailureReason.EmailsDoNotMatch);
		}

		[Test]
		public void RedirectsBackToSignUpPageWithError_WhenPasswordsDoNotMatch_OnSignUp() {
			this._accountRegistrationService.GivenUsernameIsAvailable(_bwilsonUsername);

			AccountRegistration accountRegistration = TestAccountRegistration;
			accountRegistration.PasswordConfirmation = "NOT" + accountRegistration.Password;
			ActionResult result = this._controller.SignUp(accountRegistration);

			AssertRedirectedTo<AccountController>(result, c => c.SignUp());
			this._tempData.RegistrationFailureReason.AssertEqual(RegistrationFailureReason.PasswordsDoNotMatch);
		}

		[Test]
		public void RedirectsToLoginPage_WhenSignUpCompletePageIsRefreshed() {
			Assert.That(!this._tempData.NewAccountUsername.Stored);
			ActionResult result = this._controller.SignUpComplete();
			AssertRedirectedTo<AuthenticationController>(result, c => c.Login());
		}

		[Test]
		public void RedirectsToSignUpCompletePage_OnSuccess() {
			this._accountRegistrationService.GivenUsernameIsAvailable(_bwilsonUsername);

			ActionResult actionResult = this._controller.SignUp(new AccountRegistration { Username = _bwilsonUsername.ToString() });

			AssertRedirectedTo(actionResult, (AccountController c) => c.SignUpComplete());
		}

		[Test]
		public void ReturnsError_IfUsernameIsTaken() {
			this._accountRegistrationService.GivenUsernameIsUnavailable(_bwilsonUsername);

			ActionResult actionResult = this._controller.SignUp(new AccountRegistration { Username = _bwilsonUsername.ToString() });

			AssertRedirectedTo(actionResult, (AccountController c) => c.SignUp());
			Assert.That(this._tempData.RegistrationFailureReason.Stored, "Failure reason was not stored in temp data.");
			Assert.That(this._tempData.RegistrationFailureReason.Get(), Is.EqualTo(RegistrationFailureReason.UsernameNotAvailable));
		}

		[Test]
		public void StoresEnteredDataInTempData_OnRedirectOnFailure() {
			this._accountRegistrationService.GivenUsernameIsUnavailable(_bwilsonUsername);

			var orgRegistration = new AccountRegistration {
					Email = S,
					EmailConfirmation = S,
					FirstName = S,
					LastName = S,
					Password = S,
					PasswordConfirmation = S,
					Username = S
			};
			this._controller.SignUp(orgRegistration);
			Assert.That(this._tempData.AccountRegistrationInformation.Get().Email, Is.EqualTo(orgRegistration.Email));
		}
		#endregion

		#region Setup/Teardown
		[SetUp]
		public void SetUp() {
			this._accountRegistrationService = new MockAccountRegistrationService();
			this._siteMap = new MockSiteMap();
			this._controller = new AccountController(this._accountRegistrationService, this._siteMap);
			this._tempData = new AccountControllerTempData(this._controller.TempData);
		}
		#endregion

		#region Helpers
		private MockAccountRegistrationService _accountRegistrationService;
		private AccountController _controller;

		private static readonly FullName _bwilsonFullName = new FullName(new Name("Wilson"), new GivenNames("Bob"));
		private static readonly Password _password = new Password("password");
		private static readonly Username _bwilsonUsername = new Username("username");
		private static readonly EmailAddress _bwilsonEmail = new EmailAddress("bwilson@wall.com");
		private AccountControllerTempData _tempData;
		private MockSiteMap _siteMap;

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
		#endregion
	}

	public static class TempDataExtensions {
		public static void AssertEqual<T>(this TempDataValue<T> value, T expected) {
			if (!value.Stored) Assert.Fail("TempData does not contain '{0}'.", value.Key);
			Assert.That(value.Get(), Is.EqualTo(expected), "TempData[{0}]", value.Key);
		}
	}

	public sealed class MockAccountRegistrationService : IAccountRegistrationService {
		private readonly Mock<IAccountRegistrationService> _mock;

		public MockAccountRegistrationService() {
			this._mock = new Mock<IAccountRegistrationService>();
		}

		public bool IsUsernameAvailable(Username username) {
			return this._mock.Object.IsUsernameAvailable(username);
		}

		public void CreateAccount(AccountManagement.AccountRegistration accountRegistration) {
			this._mock.Object.CreateAccount(accountRegistration);
		}

		public void GivenUsernameIsAvailable(Username username) {
			this._mock.Setup(it => it.IsUsernameAvailable(username)).Returns(true);
		}

		public void GivenUsernameIsUnavailable(Username username) {
			this._mock.Setup(it => it.IsUsernameAvailable(username)).Returns(false);
		}

		public void ExpectCreateAccount(AccountManagement.AccountRegistration accountRegistration) {
			this._mock.Setup(
					it =>
					it.CreateAccount(
							It.Is<AccountManagement.AccountRegistration>(
									actual => AccountRegistrationEqualityComparer.Instance.Equals(accountRegistration, actual)))).Verifiable();
		}

		public void Verify() {
			this._mock.Verify();
		}
	}

	public sealed class AccountRegistrationEqualityComparer : IEqualityComparer<AccountManagement.AccountRegistration> {
		public static readonly AccountRegistrationEqualityComparer Instance = new AccountRegistrationEqualityComparer();
		private AccountRegistrationEqualityComparer() {}

		public bool Equals(AccountManagement.AccountRegistration x, AccountManagement.AccountRegistration y) {
			return Equals(x.FullName, y.FullName) && Equals(x.Email, y.Email);
		}

		public int GetHashCode(AccountManagement.AccountRegistration obj) {
			return obj.Username.GetHashCode();
		}
	}
}