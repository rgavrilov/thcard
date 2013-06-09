using System.Linq;
using System.Collections.Generic;
using System;

namespace THCard.AccountManagement {
	public delegate string PasswordHashAlgorithm(string password, string salt);
}