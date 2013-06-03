using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THCard.Web.Controllers.Account.Models {
	public class SignUpViewModel {
		public AccountRegistrationInformation Information { get; set; }
	}

	public class AccountRegistrationInformation {
		public string Name { get; set; }
		public string Username { get; set; }
		public string EMail { get; set; }
		public string EmailConfirmation { get; set; }
		public string Password { get; set; }
		public string PasswordConfirmation { get; set; }
	}
}