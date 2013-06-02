using System.Web.Mvc;

namespace THCard.Web.Controllers.Home {
	public class HomeController : Controller {
		[THCard.Web.Infrastructure.Authorize]
		public ActionResult Index() {
			return View();
		}
	}
}