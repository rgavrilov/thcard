using System;
using THCard.Common;

namespace THCard.AccountManagement {
	public sealed class AccountId : GuidId {
		public AccountId(Guid id) : base(id, "Account") {}
		public static AccountId Parse(string guid) {
			return new AccountId(Guid.Parse(guid));
		}
	}
}