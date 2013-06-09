using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Web.Mvc;
using THCard.AccountManagement;
using THCard.Web.Controllers.Account.Models;
using THCard.Web.Infrastructure;
using AccountRegistration = THCard.Web.Controllers.Account.Models.AccountRegistration;

namespace THCard.Web.Controllers.Account {
	public class AccountControllerTempData {
		private readonly TempDataDictionary _tempData;

		public AccountControllerTempData(TempDataDictionary tempData) {
			_tempData = tempData;
		}

		public TempDataValue<RegistrationFailureReason> RegistrationFailureReason {
			get { return new TempDataValue<RegistrationFailureReason>("RegistrationFailureReason", _tempData); }
		}

		public TempDataValue<AccountRegistration> AccountRegistrationInformation {
			get { return new TempDataValue<AccountRegistration>("AccountRegistrationInformation", _tempData); }
		}

		public TempDataValue<Username> NewAccountUsername {
			get { return new TempDataValue<Username>("NewAccountUsername", _tempData); }
		}
	}
}