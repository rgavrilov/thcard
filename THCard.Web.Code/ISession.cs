using System;
using System.Diagnostics.Contracts;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using THCard.AccountManagement;

namespace THCard.Web {
	public interface IKeyValueStorage {
		void StoreValue<T>(string key, T value);
		T GetValue<T>(string key);
		void Remove(string key);
	}

	public interface ISession : IKeyValueStorage {
		bool IsAuthenticated { get; }
		Account AuthenticatedAccount { get; }
		LoginAttemptResult Login(Username username, Password password);
		void Logout();
	}

	public class Session : ISession {
		private readonly IAccountRepository _accountRepository;
		private Account _authenticatedAccount;

		public Session(IAccountRepository accountRepository) {
			_accountRepository = accountRepository;
		}

		private static IIdentity UserIdentity {
			get { return HttpContext.Current.User.Identity; }
			set { HttpContext.Current.User = new GenericPrincipal(value, new string[0]); }
		}

		public bool IsAuthenticated {
			get { return UserIdentity.IsAuthenticated; }
		}

		public Account AuthenticatedAccount {
			get {
				AccountId accountId = AccountId.Parse(UserIdentity.Name);
				return _authenticatedAccount ?? (_authenticatedAccount = _accountRepository.GetAccount(accountId));
			}
			private set { _authenticatedAccount = value; }
		}

		public void StoreValue<T>(string key, T value) {
			HttpContext.Current.Session[key] = value;
		}

		public T GetValue<T>(string key) {
			return (T) HttpContext.Current.Session[key];
		}

		public void Remove(string key) {
			HttpContext.Current.Session.Remove(key);
		}

		public LoginAttemptResult Login(Username username, Password password) {
			Contract.Ensures(!Contract.Result<LoginAttemptResult>().Succeeded || AuthenticatedAccount != null);

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
			UserIdentity = new GenericIdentity(account.AccountId.ToString(), "THCardAuthentication");

			AuthenticatedAccount = account;
			return LoginAttemptResult.Success(account);
		}

		public void Logout() {
			throw new NotImplementedException();
		}
	}
}