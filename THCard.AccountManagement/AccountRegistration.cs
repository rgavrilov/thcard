using System;
using System.Linq;
using System.Collections.Generic;

namespace THCard.AccountManagement {
	public sealed class AccountRegistration {
		public static readonly IEqualityComparer<AccountRegistration> EqualityComparer =
			new AccountRegistrationEqualityComparer();

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

		public override bool Equals(object obj) {
			return EqualityComparer.Equals(this, obj as AccountRegistration);
		}

		public override int GetHashCode() {
			return EqualityComparer.GetHashCode();
		}

		private sealed class AccountRegistrationEqualityComparer : IEqualityComparer<AccountRegistration> {
			public bool Equals(AccountRegistration x, AccountRegistration y) {
				if (ReferenceEquals(x, y)) {
					return true;
				}
				if (x == null || y == null) {
					return false;
				}
				if (x.GetType() != y.GetType()) {
					return false;
				}
				return x.Email.Equals(y.Email) && x.Username.Equals(y.Username) && x.Password.Equals(y.Password) &&
				       x.FullName.Equals(y.FullName);
			}

			public int GetHashCode(AccountRegistration obj) {
				unchecked {
					int hashCode = obj.Password.GetHashCode();
					hashCode = (hashCode*397) ^ obj.Username.GetHashCode();
					hashCode = (hashCode*397) ^ obj.Email.GetHashCode();
					hashCode = (hashCode*397) ^ obj.FullName.GetHashCode();
					return hashCode;
				}
			}
		}
	}
}