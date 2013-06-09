using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Transactions;

namespace THCard.AccountManagement.Dal {
	public class UserRepository : IUserRepository {
		public AccountManagement.User GetUser(UserId userId) {
			using (var db = new THCard()) {
				Guid userIdAsGuid = userId.ToGuid();
				User dbUser = db.Users.SingleOrDefault(u => u.UserId == userIdAsGuid);
				AccountManagement.User user = MapToUser(dbUser);
				return user;
			}
		}

		public AccountManagement.User GetUser(AccountId accountId) {
			Contract.Requires(accountId != null && !accountId.IsNew);
			using (var db = new THCard()) {
				Account dbAccount = db.Accounts.Find(accountId.ToGuid());
				if (dbAccount == null) {
					throw new InvalidOperationException("Account not found.");
				}
				return MapToUser(dbAccount.User);
			}
		}

		public void SaveUser(AccountManagement.User user) {
			Contract.Requires(user != null && !user.Id.IsNew);
			using (var db = new THCard()) {
				using (var transaction = new TransactionScope()) {
					User dbUser = db.Users.Find(user.Id.ToGuid());
					dbUser.FirstName = user.FullName.FirstName.ToString();
					dbUser.MiddleName = user.FullName.MiddleName.ToString();
					dbUser.LastName = user.FullName.FamilyName.ToString();
					db.SaveChanges();
					transaction.Complete();
				}
			}
		}

		public void CreateUser(AccountManagement.User user) {
			Contract.Requires(user != null && user.Id.IsNew);
			using (var db = new THCard()) {
				using (var transaction = new TransactionScope()) {
					var dbUser = new User();
					dbUser.FirstName = user.FullName.FirstName.ToString();
					dbUser.MiddleName = user.FullName.MiddleName.ToString();
					dbUser.LastName = user.FullName.FamilyName.ToString();
					db.Users.Add(dbUser);
					db.SaveChanges();
					transaction.Complete();
					user.Id = new UserId(dbUser.UserId);
				}
			}
		}

		private static AccountManagement.User MapToUser(User dbUser) {
			var fullName = new FullName(new Name(dbUser.LastName), new GivenNames(dbUser.FirstName, dbUser.MiddleName));
			var user = new AccountManagement.User(new UserId(dbUser.UserId), fullName);
			return user;
		}
	}
}