using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THCard.AccountManagement;
using THCard.Web.Controllers.Authentication;
using AccountRegistration = THCard.Web.Controllers.Account.Models.AccountRegistration;
using ControllerBase = THCard.Web.Infrastructure.ControllerBase;

namespace THCard.Web.Controllers.Account {
	public class AccountController : ControllerBase {
		private readonly IAccountRegistrationService _accountRegistrationService;
		private readonly ISiteMap _siteMap;

		public AccountController(IAccountRegistrationService accountRegistrationService, ISiteMap siteMap) {
			Contract.Requires(accountRegistrationService != null);
			_accountRegistrationService = accountRegistrationService;
			_siteMap = siteMap;
		}

		private new AccountControllerTempData TempData {
			get { return new AccountControllerTempData(base.TempData); }
		}

		[HttpGet]
		public ActionResult SignUp() {
			var result = new SignUpActionData();
			result.RegistrationFailureReasons = TempData.RegistrationFailureReason.Stored
				                                    ? new[] {TempData.RegistrationFailureReason.Get()}
				                                    : new RegistrationFailureReason[0];
			result.AccountRegistration = TempData.AccountRegistrationInformation.Get();
			return Result(result);
		}

		[HttpPost]
		public ActionResult SignUp(AccountRegistration information) {
			var username = new Username(information.Username);

			bool usernameIsAvailable = _accountRegistrationService.IsUsernameAvailable(username);
			if (!usernameIsAvailable) {
				TempData.RegistrationFailureReason.Store(RegistrationFailureReason.UsernameNotAvailable);
				TempData.AccountRegistrationInformation.Store(information);
				return RedirectToAction<AccountController>(c => c.SignUp());
			}

			var password = new Password(information.Password);
			var passwordConfirmation = new Password(information.PasswordConfirmation);
			if (!password.Equals(passwordConfirmation)) {
				TempData.RegistrationFailureReason.Store(RegistrationFailureReason.PasswordsDoNotMatch);
				return RedirectToAction<AccountController>(c => c.SignUp());
			}

			var fullName = new FullName(new Name(information.LastName), new GivenNames(information.FirstName));
			var accountRegistration = new AccountManagement.AccountRegistration(username, password, fullName, new EmailAddress(information.Email));
			_accountRegistrationService.CreateAccount(accountRegistration);

			TempData.NewAccountUsername.Store(accountRegistration.Username);
			return RedirectToAction<AccountController>(c => c.SignUpComplete());
		}

		[HttpGet]
		public ActionResult SignUpComplete() {
			if (TempData.NewAccountUsername.Stored) {
				TempData.NewAccountUsername.Keep();
				return Result(TempData.NewAccountUsername.Get());
			}
			else {
				return RedirectToAction<AuthenticationController>(c => c.Login());
			}
		}
	}

	public sealed class SignUpActionData {
		public AccountRegistration AccountRegistration { get; set; }
		public ICollection<RegistrationFailureReason> RegistrationFailureReasons { get; set; }
	}

	public enum RegistrationFailureReason {
		UsernameNotAvailable,
		PasswordsDoNotMatch,
		EmailsDoNotMatch
	}
}