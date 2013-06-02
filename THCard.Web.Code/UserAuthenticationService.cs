using System.Diagnostics.Contracts;
using System.Web.Security;
using THCard.AccountManagement;

namespace THCard.Web {
	public class UserAuthenticationService : IUserAuthenticationService {
		private readonly IAccountRepository _accountRepository;

		public UserAuthenticationService(IAccountRepository accountRepository) {
			_accountRepository = accountRepository;
		}

		public LoginAttemptResult Authenticate(Username username, Password password) {
			Contract.Ensures(!Contract.Result<LoginAttemptResult>().Succeeded ||
			                 Contract.Result<LoginAttemptResult>().Account != null);

			Credentials credentials = _accountRepository.GetAccountCredentials(username);
			if (credentials == null) {
				return LoginAttemptResult.UsernameNotFound();
			}


			bool passwordMatches = credentials.HashedPassword.Matches(password, (value, salt) => value + salt);
			if (!passwordMatches) {
				int failedLoginAttemptCount = _accountRepository.IncrementFailedLoginAttemptCount(credentials.AccountId);
				return LoginAttemptResult.IncorrectPassword(failedLoginAttemptCount);
			}

			FormsAuthentication.SetAuthCookie(credentials.AccountId.ToString(), false);
			Account account = _accountRepository.GetAccount(credentials.AccountId);

			return LoginAttemptResult.Success(account);
		}
	}
}