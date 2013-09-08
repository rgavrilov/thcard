using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace THCard.AccountManagement {
	[DebuggerDisplay("{Username}:{PasswordHash}")]
	public sealed class Credentials {
		public Credentials(Username username, SaltedHash passwordHashHash) {
			Username = username;
			PasswordHash = passwordHashHash;
		}

		public Username Username { get; private set; }
		public SaltedHash PasswordHash { get; private set; }
	}
}