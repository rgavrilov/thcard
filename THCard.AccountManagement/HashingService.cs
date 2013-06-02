using System;
using System.Text;
using THCard.AccountManagement;

namespace Try {
	public class HashingService : IHashingService {
		private static readonly Random _rnd = new Random();

		public HashedPassword Hash(Password password) {
			string empty = GenerateSalt();
			return new HashedPassword(password.ToString(), empty);
		}

		private string GenerateSalt() {
			const string saltAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvyxyz0123456789";
			const int saltLength = 10;
			return GenerateRandomString(saltAlphabet, saltLength);
		}

		private static string GenerateRandomString(string alphabet, int length) {
			StringBuilder salt = new StringBuilder(length);
			for (int index = 0; index < length; index++) {
				salt[index] = alphabet[_rnd.Next(alphabet.Length)];
			}
			return salt.ToString();
		}

		public bool Equals(Password password, HashedPassword hashedPassword) {
			string salt = hashedPassword.Salt;
			return string.Equals(Hash(password + salt), hashedPassword.ToString());
		}

		private string Hash(string value) {
			return value;
		}
	}
}