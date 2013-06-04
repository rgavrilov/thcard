using System;
using System.Collections.Generic;
using System.Linq;

namespace THCard.AccountManagement {
	public class AccountRegistrationService : IAccountRegistrationService {
		private readonly IAccountRepository _accountRepository;

		public AccountRegistrationService(IAccountRepository accountRepository) {
			_accountRepository = accountRepository;
		}

		public bool IsUsernameAvailable(Username username) {
			Credentials accountCredentials = _accountRepository.GetAccountCredentials(username);
			return accountCredentials == null;
		}
	}
}