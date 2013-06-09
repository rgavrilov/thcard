using System.Linq;
using System.Collections.Generic;
using System;

namespace THCard.Web.Controllers.Account.Models {
	public class AccountRegistration {
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public string EmailConfirmation { get; set; }
		public string Password { get; set; }
		public string PasswordConfirmation { get; set; }
	}
}