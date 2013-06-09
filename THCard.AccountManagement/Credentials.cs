using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace THCard.AccountManagement {
	[DebuggerDisplay("{Username}:{HashedPassword}")]
	public sealed class Credentials {
		public Credentials(Username username, HashedPassword hashedPassword) {
			Username = username;
			HashedPassword = hashedPassword;
		}

		public Username Username { get; private set; }
		public HashedPassword HashedPassword { get; private set; }
	}
}