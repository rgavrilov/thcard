using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace THCard.AccountManagement {

	[DebuggerDisplay("{_username}")]
	public sealed class Username : IEquatable<Username> {
		public override int GetHashCode() {
			return (_username != null ? _username.GetHashCode() : 0);
		}

		private readonly string _username;

		[DebuggerStepThrough]
		public Username(string username) {
			Contract.Assert(!string.IsNullOrWhiteSpace(username));
			_username = username.Trim();
		}

		[DebuggerStepThrough]
		public bool Equals(Username other) {
			return Equals(this, other);
		}

		[DebuggerStepThrough]
		public override string ToString() {
			return _username;
		}

		[DebuggerStepThrough]
		public override bool Equals(object other) {
			return Equals(this, other as Username);
		}

		[DebuggerStepThrough]
		public static bool Equals(Username left, Username right) {
			if (object.ReferenceEquals(left, right)) {
				return true;
			}
			if (left == null || right == null) {
				return false;
			}
			return string.Equals(left._username, right._username, StringComparison.OrdinalIgnoreCase);
		}
	}
}