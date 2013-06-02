using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Web.Mvc;
using Ninject;
using Ninject.Web.Common;
using THCard.AccountManagement;
using THCard.AccountManagement.Dal;
using THCard.Web.Infrastructure;

namespace THCard.Web {
	public class MvcApplication : NinjectHttpApplication {
		protected override IKernel CreateKernel() {
			var kernel = new StandardKernel();

			kernel.Bind<ISession>().To<Session>().InRequestScope();
			kernel.Bind<IAccountRepository>().To<AccountRepository>();
			kernel.Bind<IUserRepository>().To<UserRepository>();
			kernel.Bind<ISiteMap>().To<SiteMap>();
			kernel.Bind<IPresenterFactory>().To<PresenterFactory>();
			kernel.Bind<IPresenterActivator>()
			      .ToConstant<IPresenterActivator>(new DelegatePresenterActivator(type => kernel.Get(type)));

			return kernel;
		}

		protected override void OnApplicationStarted() {
			base.OnApplicationStarted();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			PresenterFactory.RegisterPresenters();
		}
	}
}