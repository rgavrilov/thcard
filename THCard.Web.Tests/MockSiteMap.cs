using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using JetBrains.Annotations;
using THCard.AccountManagement;

namespace THCard.Web.Tests {
	public sealed class MockSiteMap : ISiteMap {
		[NotNull]
		public RouteValueDictionary LoginPage {
			get { return new PageRoute("Login"); }
		}

		[NotNull]
		public RouteValueDictionary PublicLandingPage {
			get { return new PageRoute("PublicLanding"); }
		}

		[NotNull]
		public RouteValueDictionary GetLandingPage(Account account) {
			return new PageRoute("Landing");
		}
	}
}