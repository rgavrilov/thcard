using System;
using System.Linq;
using System.Collections.Generic;

namespace THCard.AccountManagement {
	public sealed class AccountRegistration {
		public AccountRegistration(Username username, Password password, FullName fullName, EmailAddress email) {
			Email = email;
			Username = username;
			Password = password;
			FullName = fullName;
		}

		public Password Password { get; private set; }
		public Username Username { get; private set; }
		public EmailAddress Email { get; set; }
		public FullName FullName { get; set; }
	}
}