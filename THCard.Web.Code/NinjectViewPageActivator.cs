using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;

namespace THCard.Web {
	public class NinjectViewPageActivator : IViewPageActivator {
		private readonly StandardKernel _kernel;

		public NinjectViewPageActivator(StandardKernel kernel) {
			_kernel = kernel;
		}

		public object Create(ControllerContext controllerContext, Type type) {
			return _kernel.Get(type);
		}
	}
}