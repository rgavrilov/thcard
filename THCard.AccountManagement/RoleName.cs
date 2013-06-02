using System;
using System.Diagnostics;
using THCard.Common;

namespace THCard.AccountManagement {
	[DebuggerDisplay("{InternalId}")]
	public sealed class RoleName : Id, IEquatable<RoleName> {
		public RoleName(string name) : base(name, "Account roles") {}

		public bool Equals(RoleName other) {
			return Equals(this, other);
		}

		public static bool Equals(RoleName left, RoleName right) {
			if (ReferenceEquals(left, right)) {
				return true;
			}
			if (left == null ^ right == null) {
				return false;
			}
			return string.Equals(left.InternalId.Trim(), right.InternalId.Trim(), StringComparison.OrdinalIgnoreCase);
		}

		public static bool operator ==(RoleName left, RoleName right) {
			return Equals(left, right);
		}

		public static bool operator !=(RoleName left, RoleName right) {
			return !(left == right);
		}

		public override string ToString() {
			return base.InternalId;
		}
	}
}