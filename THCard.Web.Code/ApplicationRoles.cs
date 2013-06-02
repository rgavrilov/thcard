using THCard.AccountManagement;

namespace THCard.Web.Controllers.Authentication {
	public class ApplicationRoles {
		public static readonly AccountRole Member = new AccountRole(new RoleName("Member"));
	}
}