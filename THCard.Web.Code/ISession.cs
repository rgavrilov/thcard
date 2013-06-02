using THCard.AccountManagement;

namespace THCard.Web {
	public interface ISession {
		bool IsAuthenticated { get; }
		Account AuthenticatedAccount { get; }
		void StoreValue<T>(string key, T value);
		T GetValue<T>(string key);
		void RemoveValue(string key);

		void BeginAuthenticatedSession(Account account);
		void EndAuthenticatedSession();
	}
}