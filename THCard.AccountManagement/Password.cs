using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace THCard.AccountManagement {
	[DebuggerStepThrough]
	public sealed class Password {
		private readonly string _password;

		[DebuggerStepThrough]
		public Password(string password) {
			_password = password;
		}

		[DebuggerStepThrough]
		public override int GetHashCode() {
			return (_password != null ? _password.GetHashCode() : 0);
		}

		[DebuggerStepThrough]
		public override string ToString() {
			return _password;
		}

		[DebuggerStepThrough]
		public override bool Equals(object obj) {
			if (ReferenceEquals(this, obj)) {
				return true;
			}
			if (ReferenceEquals(null, obj)) {
				return false;
			}
			return obj is Password &&
			       string.Equals(_password, ((Password) obj)._password, StringComparison.Ordinal);
		}
	}
}