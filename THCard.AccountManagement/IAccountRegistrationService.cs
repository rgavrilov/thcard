using System;
using System.Collections.Generic;
using System.Linq;

namespace THCard.AccountManagement {
	public interface IAccountRegistrationService {
		bool IsUsernameAvailable(Username username);
		void CreateAccount(AccountRegistration accountRegistration);
	}
}