using System;
using System.Web.Mvc;
using THCard.Web.Infrastructure;

namespace THCard.Web.Infrastructure {
    public class ViewPresenterResult<TActionResult> : ViewResult {
        private readonly TActionResult _actionResult;
	    private readonly IPresenterFactory _presenterFactory;

	    public ViewPresenterResult(TActionResult actionResult, IPresenterFactory presenterFactory) {
	        this._actionResult = actionResult;
	        _presenterFactory = presenterFactory;
        }

	    public override void ExecuteResult(ControllerContext context) {
		    if (string.IsNullOrEmpty(ViewName)) ViewName = context.RouteData.GetRequiredString("action");
            Type controllerType = context.Controller.GetType();
            Func<object, object> presenter = _presenterFactory.GetPresenter(ViewName, controllerType, typeof (TActionResult));
            ViewData.Model = presenter(this._actionResult);
            base.ExecuteResult(context);
        }
    }
}