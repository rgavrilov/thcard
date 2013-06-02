namespace THCard.AccountManagement {
	public sealed class Account {
		private readonly AccountRoles _roles;
		private readonly Username _username;

		public Account(AccountId accountId, Username username, UserId userId, AccountRoles roles) {
			UserId = userId;
			AccountId = accountId;
			_username = username;
			_roles = roles;
		}

		public AccountId AccountId { get; private set; }

		public UserId UserId { get; private set; }

		public bool IsInRole(AccountRole role) {
			return _roles.Matches(role);
		}

		public override string ToString() {
			return _username.ToString();
		}
	}
}