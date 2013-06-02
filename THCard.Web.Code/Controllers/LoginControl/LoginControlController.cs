using System.Web.Mvc;
using THCard.AccountManagement;
using ControllerBase = THCard.Web.Infrastructure.ControllerBase;

namespace THCard.Web.Controllers.LoginControl {
	public class LoginControlController : ControllerBase {
		private readonly ISession _session;

		public LoginControlController(ISession session) {
			_session = session;
		}

		[ChildActionOnly]
		public ActionResult LoginControl() {
			if (_session.IsAuthenticated) {
				Account account = _session.AuthenticatedAccount;
				return Result("Authenticated", account);
			}
			else {
				return View("Unauthenticated");
			}
		}
	}
}