using System.Web.Mvc;
using Ninject;
using Ninject.Web.Common;

namespace THCard.Web {
	public class NinjectModelBinder : IModelBinder {
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
			return new Bootstrapper().Kernel.Get(bindingContext.ModelType);
		}
	}
}