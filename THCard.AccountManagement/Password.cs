using System;
using System.Diagnostics;

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
			var otherPassword = obj as Password;
			if (otherPassword != null) {
				return string.Equals(_password, otherPassword._password, StringComparison.Ordinal);
			}
			return false;
		}
	}
}