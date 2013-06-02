namespace THCard.AccountManagement {
	public interface IAccountRepository {
		Credentials GetAccountCredentials(Username username);
		Account GetAccount(AccountId accountId);
		Account CreateAccount(AccountRegistration accountRegistration);
		int IncrementFailedLoginAttemptCount(AccountId accountId);
	}
}