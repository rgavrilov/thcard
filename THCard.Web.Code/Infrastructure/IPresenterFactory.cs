using System;

namespace THCard.Web.Infrastructure {
    public interface IPresenterFactory {
        Func<object, object> GetPresenter(string viewName, Type controllerType, Type actionResultType);
    }
}