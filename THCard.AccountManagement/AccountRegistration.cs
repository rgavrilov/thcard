namespace THCard.AccountManagement {
	public sealed class AccountRegistration {
		public AccountRegistration(Username username, HashedPassword hashedPassword, UserId userId) {
			Username = username;
			HashedPassword = hashedPassword;
			UserId = userId;
		}

		public UserId UserId { get; private set; }
		public Username Username { get; private set; }
		public HashedPassword HashedPassword { get; private set; }
	}
}