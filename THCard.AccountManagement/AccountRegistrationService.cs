using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace THCard.AccountManagement {
	public class AccountRegistrationService : IAccountRegistrationService {
		private readonly IAccountRepository _accountRepository;
		private readonly IUserRepository _userRepository;

		public AccountRegistrationService(IAccountRepository accountRepository, IUserRepository userRepository) {
			_accountRepository = accountRepository;
			_userRepository = userRepository;
		}

		public bool IsUsernameAvailable(Username username) {
			Contract.Requires(username != null);
			Account account = _accountRepository.FindAccount(username);
			return account == null;
		}

		public void CreateAccount(AccountRegistration accountRegistration) {
			var user = new User(UserId.New, accountRegistration.FullName);
			_userRepository.CreateUser(user);
			HashedPassword hashedPassword = HashedPassword.Create(accountRegistration.Password,
			                                                      (password, salt) => password + salt, "1234");
			_accountRepository.CreateAccount(new Account(AccountId.New, accountRegistration.Username, new AccountRoles()),
			                                 hashedPassword, user.Id);
		}
	}
}