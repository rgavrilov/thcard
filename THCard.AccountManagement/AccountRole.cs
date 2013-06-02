using System.Collections.Generic;
using System.Linq;

namespace THCard.AccountManagement {
	public sealed class AccountRoles : List<AccountRole> {
		public AccountRoles(IEnumerable<AccountRole> roles)
			: base(roles) {}

		public AccountRoles(params AccountRole[] roles)
			: base(roles) {}

		public bool Matches(AccountRole role) {
			return this.Any(r => r.Matches(role));
		}
	}

	public sealed class AccountRole {
		public AccountRole(RoleName roleName) {
			RoleName = roleName;
		}

		public RoleName RoleName { get; private set; }

		public bool Matches(AccountRole other) {
			return RoleName == other.RoleName;
		}
	}
}