using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;

namespace THCard.AccountManagement.Dal {
	public class AccountRepository : IAccountRepository {
		Credentials IAccountRepository.GetAccountCredentials(Username username) {
			using (var db = new THCard()) {
				string usernameAsString = username.ToString();
				IQueryable<Account> dbAccountQuery = db.Accounts.Where(a => a.Username == usernameAsString);
				var dbCredentials = 
					dbAccountQuery
						.Join(
							db.AccountPasswords, 
							a => a.AccountId, 
							ap => ap.AccountId,
							(account, accountPassword) => new {Account = account, AccountPassword = accountPassword})
						.FirstOrDefault();
				if (dbCredentials == null) return null;
				Credentials credentials = BuildCredentials(dbCredentials.Account, dbCredentials.AccountPassword);
				return credentials;
			}
		}

		private Credentials BuildCredentials(Account account, AccountPassword accountPassword) {
			return new Credentials(new AccountId(account.AccountId), new Username(account.Username), new HashedPassword(accountPassword.PasswordHash.TrimEnd(), accountPassword.Salt.TrimEnd()));
		}

		public AccountManagement.Account GetAccount(AccountId accountId) {
			using (var db = new THCard()) {
				Account dbAccount = FindAccountById(db, accountId.ToGuid());
				if (dbAccount != null) {
					return BuildAccountFromDBAccount(dbAccount);
				}
				else {
					return null;
				}
			}
		}

		private static Account FindAccountById(THCard db, Guid accountId) {
			Account dbAccount = db.Accounts.SingleOrDefault(a => a.AccountId == accountId);
			return dbAccount;
		}

		public AccountManagement.Account CreateAccount(AccountRegistration accountRegistration) {
			using (var db = new THCard()) {
				using (
					var transaction = new TransactionScope()) {
					Account dbAccount = db.Accounts.Add(new Account {
						Username = accountRegistration.Username.ToString(),
						AccountPassword =
							new AccountPassword {
								PasswordHash = accountRegistration.HashedPassword.ToString(),
								Salt = accountRegistration.HashedPassword.Salt
							}
					});
					db.SaveChanges();
					transaction.Complete();
					return BuildAccountFromDBAccount(dbAccount);
				}
			}
		}

		public int IncrementFailedLoginAttemptCount(AccountId accountId) {
			using (var db = new THCard()) {
				using (
					var transaction = new TransactionScope(TransactionScopeOption.Required,
					                                       new TransactionOptions {IsolationLevel = IsolationLevel.RepeatableRead})) {
					FailedLoginAttempt dbFailedLoginAttempt = FindFailedLoginAttempt(db, accountId.ToGuid());
					if (dbFailedLoginAttempt == null) {
						db.FailedLoginAttempts.Add(new FailedLoginAttempt {AccountId = accountId.ToGuid(), FailedLoginAttemptCount = 0});
						db.SaveChanges();
						transaction.Complete();
						return 1;
					}
					else {
						++dbFailedLoginAttempt.FailedLoginAttemptCount;
						db.SaveChanges();
						transaction.Complete();
						return dbFailedLoginAttempt.FailedLoginAttemptCount;
					}
				}
			}
		}

		private static FailedLoginAttempt FindFailedLoginAttempt(THCard db, Guid accountId) {
			return db.FailedLoginAttempts.SingleOrDefault(fla => fla.AccountId == accountId);
		}

		private static AccountManagement.Account BuildAccountFromDBAccount(Account dbAccount) {
			AccountRoles roles = new AccountRoles(dbAccount.Roles.Select(BuildRoleFromDBRole));
			return new AccountManagement.Account(new AccountId(dbAccount.AccountId), new Username(dbAccount.Username), new UserId(dbAccount.UserId), roles);
		}

		private static AccountRole BuildRoleFromDBRole(Dal.Role dbRole) {
			throw new NotImplementedException();
		}
	}
}