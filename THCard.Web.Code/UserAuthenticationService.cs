using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using THCard.AccountManagement;

namespace THCard.Web {
	public class UserAuthenticationService : IUserAuthenticationService {
		private readonly IAccountRepository _accountRepository;

		public UserAuthenticationService(IAccountRepository accountRepository) {
			_accountRepository = accountRepository;
		}

		public LoginAttemptResult Authenticate(Username username, Password password) {
			Contract.Ensures(Contract.Result<LoginAttemptResult>().Succeeded == false ||
			                 Contract.Result<LoginAttemptResult>().Account != null);

			Account account = _accountRepository.FindAccount(username);
			if (account == null) {
				return LoginAttemptResult.UsernameNotFound();
			}

			SaltedHash accountPasswordHash = _accountRepository.GetAccountPassword(account.AccountId);

			bool passwordMatches = new Hasher().Matches(password.ToString(), accountPasswordHash);
			if (!passwordMatches) {
				int failedLoginAttemptCount = _accountRepository.IncrementFailedLoginAttemptCount(account.AccountId);
				return LoginAttemptResult.IncorrectPassword(failedLoginAttemptCount);
			}

			return LoginAttemptResult.Success(account);
		}
	}
}