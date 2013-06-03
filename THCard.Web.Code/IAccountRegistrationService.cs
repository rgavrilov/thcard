using System.Collections.Generic;
using System.Linq;
using System;
using THCard.AccountManagement;

namespace THCard.Web {
	public interface IAccountRegistrationService
	{
		bool IsUsernameAvailable(Username username);
	}
}