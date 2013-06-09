using System;
using System.Collections.Generic;
using System.Linq;

namespace THCard.AccountManagement {
	public interface IAccountRepository {
		Account FindAccount(Username username);
		Account GetAccount(AccountId accountId);
		void SaveAccount(Account account);
		int IncrementFailedLoginAttemptCount(AccountId accountId);
		void CreateAccount(Account account, HashedPassword hashedPassword, UserId userId);
		HashedPassword GetAccountPassword(AccountId accountId);
	}
}