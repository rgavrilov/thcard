using System;
using System.Linq;

namespace THCard.AccountManagement.Dal {
	public class UserRepository : IUserRepository {
		public AccountManagement.User GetUser(UserId userId) {
			using (var db = new THCard()) {
				Guid userIdAsGuid = userId.ToGuid();
				User user = db.Users.SingleOrDefault(u => u.UserId == userIdAsGuid);
				var fullName = new FullName {
					FamilyName = user.LastName,
					GivenNames = new GivenNames(user.FirstName, user.MiddleName)
				};
				return new AccountManagement.User(new UserId(user.UserId), fullName);
			}
		}

		public AccountManagement.User CreateUser(FullName fullName) {
			using (var db = new THCard()) {
				var newUser = new User();
				newUser.FirstName = fullName.FirstName.ToString();
				newUser.MiddleName = fullName.MiddleName.ToString();
				newUser.LastName = fullName.FamilyName;
				db.Users.Add(newUser);
				db.SaveChanges();
				return new AccountManagement.User(new UserId(newUser.UserId), fullName);
			}
		}
	}
}