using System.Diagnostics.Contracts;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using THCard.AccountManagement;

namespace THCard.Web {
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
				Contract.Requires(UserIdentity.IsAuthenticated);
				AccountId accountId = AccountId.Parse(UserIdentity.Name);
				Contract.Assert(accountId != null);
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

		public void RemoveValue(string key) {
			HttpContext.Current.Session.Remove(key);
		}

		public void BeginAuthenticatedSession(Account account) {
			AuthenticatedAccount = account;
			UserIdentity = new GenericIdentity(account.AccountId.ToString(), "THCardAuthentication");
			FormsAuthentication.SetAuthCookie(account.AccountId.ToString(), false);
		}

		public void EndAuthenticatedSession() {
			UserIdentity = new GenericIdentity(string.Empty);
		}
	}
}