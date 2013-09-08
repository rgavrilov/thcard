using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace THCard.AccountManagement {
	[DebuggerDisplay("{Hash}#{Salt}")]
	public sealed class SaltedHash {
		public SaltedHash(string hash, string salt) {
			Hash = hash;
			Salt = salt;
		}

		public string Salt { get; private set; }

		public string Hash { get; private set; }

		public override bool Equals(object obj) {
			return Equals(obj as SaltedHash);
		}

		public bool Equals(SaltedHash that) {
			if (ReferenceEquals(this, that)) {
				return true;
			}
			if (that == null) {
				return false;
			}
			return string.Equals(Hash, that.Hash, StringComparison.Ordinal) &&
			       string.Equals(Salt, that.Salt, StringComparison.Ordinal);
		}

		public override int GetHashCode() {
			return Hash.GetHashCode();
		}
	}

	public class Hasher {
		public SaltedHash Hash(string value) {
			const string salt = "0";
			return Hash(value, salt);
		}

		private static SaltedHash Hash(string value, string salt) {
			return new SaltedHash(value + "|" + salt, salt);
		}

		public bool Matches(string value, SaltedHash hash) {
			return Hash(value, hash.Salt).Equals(hash);
		}
	}
}