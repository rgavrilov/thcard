using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace THCard.AccountManagement {
	[DebuggerDisplay("{PasswordHasd}#{Salt}")]
	public sealed class HashedPassword {
		public HashedPassword(string passwordHash, string salt) {
			PasswordHash = passwordHash;
			Salt = salt;
		}

		public string Salt { get; private set; }

		public string PasswordHash { get; private set; }

		public bool Matches(Password password, PasswordHashAlgorithm hashAlgorithm) {
			string thisHash = PasswordHash;
			string thatHash = Hash(password, hashAlgorithm, Salt);
			return string.Equals(thisHash, thatHash);
		}

		public static HashedPassword Create(Password password, PasswordHashAlgorithm hashAlgorithm,
		                                    string salt) {
			return new HashedPassword(Hash(password, hashAlgorithm, salt), salt);
		}

		private static string Hash(Password password, PasswordHashAlgorithm hashAlgorithm, string salt) {
			return hashAlgorithm(password.ToString(), salt);
		}
	}
}