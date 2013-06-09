using System;
using System.Collections.Generic;
using System.Linq;
using THCard.Common;

namespace THCard.AccountManagement {
	public sealed class AccountId : GuidId {
		public static readonly AccountId New = new AccountId(Guid.Empty);

		public AccountId(Guid id) : base(id, "Account") {}

		public static AccountId Parse(string guid) {
			return new AccountId(Guid.Parse(guid));
		}
	}
}