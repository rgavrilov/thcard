using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using ExpressionHelper = Microsoft.Web.Mvc.Internal.ExpressionHelper;

namespace THCard.Web.Tests {
	public class ControllerTestBase {
		public static string S {
			get { return Guid.NewGuid().ToString(); }
		}


		protected void AssertRedirectedTo(ActionResult actionResult, RouteValueDictionary routeValues) {
			Assert.That(actionResult, Is.Not.Null);
			Assert.That(actionResult, Is.InstanceOf<RedirectToRouteResult>());
			var redirectResult = (RedirectToRouteResult) actionResult;

			Assert.That(redirectResult.RouteValues, Is.EquivalentTo(routeValues));
		}

		protected void AssertRedirectedTo<TController>(ActionResult actionResult,
		                                               Expression<Action<TController>> action)
			where TController : Controller {
			RouteValueDictionary routeValues = ExpressionHelper.GetRouteValuesFromExpression(action);
			AssertRedirectedTo(actionResult, routeValues);
		}
	}
}