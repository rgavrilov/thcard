﻿using System.Web.Routing;
using THCard.AccountManagement;

namespace THCard.Web {
	public interface ISiteMap {
		RouteValueDictionary GetLandingPage(Account account);
		RouteValueDictionary PublicLandingPage { get; }
		RouteValueDictionary LoginPage { get; }
	}
}