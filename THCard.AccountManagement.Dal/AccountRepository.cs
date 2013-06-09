using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Transactions;
using THCard.Dal.Common;

namespace THCard.AccountManagement.Dal {
	public class AccountRepository : RepositoryBase, IAccountRepository {
		public AccountManagement.Account FindAccount(Username username) {
			using (var db = new THCard()) {
				Account dbAccount = FindAccountByUsername(username, db);
				if (dbAccount == null) {
					return null;
				}
				return MapAccount(dbAccount);
			}
		}

		public AccountManagement.Account GetAccount(AccountId accountId) {
			using (var db = new THCard()) {
				Account dbAccount = FindAccountById(db, accountId.ToGuid());
				if (dbAccount != null) {
					return MapAccount(dbAccount);
				}
				else {
					return null;
				}
			}
		}

		public int IncrementFailedLoginAttemptCount(AccountId accountId) {
			using (var db = new THCard()) {
				using (var transaction = new TransactionScope(TransactionScopeOption.Required,
				                                              new TransactionOptions {
					                                              IsolationLevel = IsolationLevel.RepeatableRead
				                                              })) {
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

		public void SaveAccount(AccountManagement.Account account) {
			throw new NotImplementedException();
		}

		public void CreateAccount(AccountManagement.Account account, HashedPassword hashedPassword, UserId userId) {
			Contract.Assert(account.AccountId.IsNew);
			using (var db = new THCard()) {
				using (var transaction = new TransactionScope()) {
					var dbAccount = new Account {
						Username = account.Username.ToString(),
						UserId = userId.ToGuid(),
						AccountPassword = new AccountPassword {
							PasswordHash = hashedPassword.PasswordHash,
							Salt = hashedPassword.Salt
						}
					};
					db.Accounts.Add(dbAccount);
					db.SaveChanges();
					transaction.Complete();
					account.AccountId = new AccountId(dbAccount.AccountId);
				}
			}
		}

		public HashedPassword GetAccountPassword(AccountId accountId) {
			using (var db = new THCard()) {
				Account dbAccount = db.Accounts.Find(accountId);
				AssertFound(dbAccount);
				return MapToHashedPassword(dbAccount);
			}
		}

		private static HashedPassword MapToHashedPassword(Account dbAccount) {
			return new HashedPassword(dbAccount.AccountPassword.PasswordHash.TrimEnd(), dbAccount.AccountPassword.Salt.TrimEnd());
		}

		private static Account FindAccountByUsername(Username username, THCard db) {
			string usernameAsString = username.ToString();
			return db.Accounts.FirstOrDefault(a => a.Username == usernameAsString);
		}

		private static Account FindAccountById(THCard db, Guid accountId) {
			Account dbAccount = db.Accounts.SingleOrDefault(a => a.AccountId == accountId);
			return dbAccount;
		}

		private static FailedLoginAttempt FindFailedLoginAttempt(THCard db, Guid accountId) {
			return db.FailedLoginAttempts.SingleOrDefault(fla => fla.AccountId == accountId);
		}

		private static AccountManagement.Account MapAccount(Account dbAccount) {
			var roles = new AccountRoles(dbAccount.Roles.Select(BuildRoleFromDBRole));
			return new AccountManagement.Account(new AccountId(dbAccount.AccountId), new Username(dbAccount.Username), roles);
		}

		private static AccountRole BuildRoleFromDBRole(Role dbRole) {
			throw new NotImplementedException();
		}
	}
}