using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace THCard.AccountManagement {
	[DebuggerDisplay("{_email}")]
	public sealed class EmailAddress : IEquatable<EmailAddress> {
		private readonly string _email;

		public EmailAddress(string email) {
			_email = email;
		}

		public bool Equals(EmailAddress other) {
			if (other == null) {
				return false;
			}
			return string.Equals(_email, other._email, StringComparison.OrdinalIgnoreCase);
		}

		public override string ToString() {
			return _email;
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(this, obj)) {
				return true;
			}
			if (obj == null || obj.GetType() != GetType()) {
				return false;
			}
			var other = (EmailAddress) obj;
			return Equals(other);
		}

		public override int GetHashCode() {
			return _email.GetHashCode();
		}
	}
}