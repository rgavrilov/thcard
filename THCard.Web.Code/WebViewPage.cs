using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;

namespace THCard.Web {
	public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel> {
		[Inject]
		public ISiteMap SiteMap { get; set; }
	}

	public abstract class WebViewPage : WebViewPage<object> {}
}