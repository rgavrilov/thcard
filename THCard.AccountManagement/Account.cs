using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace THCard.AccountManagement {
	[DebuggerDisplay("{Username}#{AccountId}")]
	public sealed class Account {
		private readonly AccountRoles _roles;
		private AccountId _accountId;

		public Account(AccountId accountId, Username username, AccountRoles roles) {
			Contract.Requires(accountId != null);
			Contract.Requires(username != null);
			Contract.Requires(roles != null);

			AccountId = accountId;
			Username = username;
			_roles = roles;
		}

		public AccountId AccountId {
			get { return _accountId; }
			set {
				Contract.Requires(_accountId.IsNew && value != null && !value.IsNew);
				_accountId = value;
			}
		}

		public Username Username { get; private set; }

		public bool IsInRole(AccountRole role) {
			return _roles.Matches(role);
		}
	}
}