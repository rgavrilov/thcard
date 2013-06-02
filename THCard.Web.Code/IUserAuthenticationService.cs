using THCard.AccountManagement;

namespace THCard.Web {
	public interface IUserAuthenticationService {
		LoginAttemptResult Authenticate(Username username, Password password);
	}
}