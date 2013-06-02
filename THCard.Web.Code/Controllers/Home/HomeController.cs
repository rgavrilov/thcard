using System.Web.Mvc;

namespace THCard.Web.Controllers.Home {
	public class HomeController : Controller {
		[AllowAnonymous]
		public ActionResult Index() {
			return View();
		}
	}
}