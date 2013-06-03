using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THCard.AccountManagement;
using THCard.Web.Controllers.Account.Models;
using THCard.Web.Controllers.Authentication.Models;
using THCard.Web.Infrastructure;
using ControllerBase = THCard.Web.Infrastructure.ControllerBase;

namespace THCard.Web.Controllers.Account {
	public class AccountControllerTempData {
		private readonly TempDataDictionary _tempData;

		public AccountControllerTempData(TempDataDictionary tempData) {
			_tempData = tempData;
		}

		public TempDataValue<ErrorMessages> ErrorMessages {
			get { return new TempDataValue<ErrorMessages>("ErrorMessages", _tempData); }
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
			return View();
		}

		[HttpPost]
		public ActionResult SignUp(AccountRegistrationInformation information) {
			bool usernameIsAvailable = _accountRegistrationService.IsUsernameAvailable(new Username(information.Username));
			if (usernameIsAvailable) {
				return RedirectToAction<AccountController>(c => c.SignUpComplete());
			}
			else {
				_tempData.ErrorMessages.Store(new ErrorMessages {string.Format("Username '{0}' is taken.", information.Username)});
				return RedirectToAction<AccountController>(c => c.SignUp());
			}
		}

		[HttpGet]
		public ActionResult SignUpComplete() {
			return View();
		}
	}
}