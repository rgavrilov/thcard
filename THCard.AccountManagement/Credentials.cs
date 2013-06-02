using System.Diagnostics;

namespace THCard.AccountManagement {

	[DebuggerDisplay("{Username}:{HashedPassword.PasswordHash}#{HashedPassword.Salt} for {AccountId}")]
	public sealed class Credentials {
		public Credentials(AccountId accountId, Username username, HashedPassword hashedPassword) {
			
			AccountId = accountId;
			Username = username;
			HashedPassword = hashedPassword;
		}

		public Username Username { get; private set; }
		public HashedPassword HashedPassword { get; private set; }
		public AccountId AccountId { get; private set; }
	}
}