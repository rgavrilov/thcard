using System.Web.Mvc;

namespace THCard.Web.Controllers.Member {
	public class MemberController : Controller {
		public ActionResult Dashboard() {
			return Content("Member Dashboard");
		}
	}
}