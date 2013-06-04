using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THCard.AccountManagement;
using THCard.Web.Controllers.Account.Models;
using THCard.Web.Infrastructure;
using ControllerBase = THCard.Web.Infrastructure.ControllerBase;

namespace THCard.Web.Controllers.Account {
	public class AccountControllerTempData {
		private readonly TempDataDictionary _tempData;

		public AccountControllerTempData(TempDataDictionary tempData) {
			_tempData = tempData;
		}

		public TempDataValue<RegistrationFailureReason> RegistrationFailureReason {
			get { return new TempDataValue<RegistrationFailureReason>("RegistrationFailureReason", _tempData); }
		}

		public TempDataValue<AccountRegistrationInformation> AccountRegistrationInformation {
			get { return new TempDataValue<AccountRegistrationInformation>("AccountRegistrationInformation", _tempData); }
		}
	}

	[PresenterFor(typeof (AccountController))]
	public class AccountPresenter {
		public SignUpViewModel SignUp(SignUpActionData actionData) {
			var viewModel = new SignUpViewModel();
			IEnumerable<string> errors = actionData.RegistrationFailureReasons.Select(GetRegistrationFailureReasonText);
			viewModel.Errors = errors;
			return viewModel;
		}

		private string GetRegistrationFailureReasonText(RegistrationFailureReason reason) {
			switch (reason) {
				case RegistrationFailureReason.UsernameNotAvailable:
					return "Username is not available.";
				default:
					throw new ArgumentOutOfRangeException("reason");
			}
		}
	}

	public class AccountController : ControllerBase {
		private readonly IAccountRegistrationService _accountRegistrationService;
		private readonly AccountControllerTempData _tempData;

		public AccountController(IAccountRegistrationService accountRegistrationService, AccountControllerTempData tempData) {
			Contract.Assert(accountRegistrationService != null);
			Contract.Assert(tempData != null);
			_accountRegistrationService = accountRegistrationService;
			_tempData = tempData;
		}

		[HttpGet]
		public ActionResult SignUp() {
			var result = new SignUpActionData();
			result.RegistrationFailureReasons = _tempData.RegistrationFailureReason.Stored
				                                    ? new[] {_tempData.RegistrationFailureReason.Get()}
				                                    : new RegistrationFailureReason[0];
			result.AccountRegistrationInformation = _tempData.AccountRegistrationInformation.Get();
			return Result(result);
		}

		[HttpPost]
		public ActionResult SignUp(AccountRegistrationInformation information) {
			bool usernameIsAvailable = _accountRegistrationService.IsUsernameAvailable(new Username(information.Username));
			if (usernameIsAvailable) {
				return RedirectToAction<AccountController>(c => c.SignUpComplete());
			}
			else {
				_tempData.RegistrationFailureReason.Store(RegistrationFailureReason.UsernameNotAvailable);
				_tempData.AccountRegistrationInformation.Store(information);
				return RedirectToAction<AccountController>(c => c.SignUp());
			}
		}

		[HttpGet]
		public ActionResult SignUpComplete() {
			return View();
		}
	}

	public sealed class SignUpActionData {
		public AccountRegistrationInformation AccountRegistrationInformation { get; set; }
		public ICollection<RegistrationFailureReason> RegistrationFailureReasons { get; set; }
	}

	public enum RegistrationFailureReason {
		UsernameNotAvailable
	}
}