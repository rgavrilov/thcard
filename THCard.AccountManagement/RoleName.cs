using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace THCard.AccountManagement {
	[DebuggerDisplay("{InternalId}")]
	public sealed class RoleName : IEquatable<RoleName> {
		private readonly string _name;

		public RoleName(string name) {
			_name = name;
		}

		public bool Equals(RoleName other) {
			return Equals(this, other);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) {
				return false;
			}
			if (ReferenceEquals(this, obj)) {
				return true;
			}
			return obj is RoleName && Equals((RoleName) obj);
		}

		public override int GetHashCode() {
			return (_name != null ? _name.GetHashCode() : 0);
		}

		public static bool Equals(RoleName left, RoleName right) {
			if (ReferenceEquals(left, right)) {
				return true;
			}
			if (left == null || right == null) {
				return false;
			}
			return string.Equals(left._name.Trim(), right._name.Trim(), StringComparison.OrdinalIgnoreCase);
		}

		public static bool operator ==(RoleName left, RoleName right) {
			return Equals(left, right);
		}

		public static bool operator !=(RoleName left, RoleName right) {
			return !(left == right);
		}

		public override string ToString() {
			return _name;
		}
	}
}