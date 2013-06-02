using System.Web.Mvc;
using System.Web.Routing;
using THCard.Web.Controllers.Authentication.Models;
using THCard.Web.Infrastructure;

namespace THCard.Web.Controllers.Authentication {
	public sealed class AuthenticationTempData {
		private readonly TempDataDictionary _tempData;

		public AuthenticationTempData(TempDataDictionary tempData) {
			_tempData = tempData;
		}

		public TempDataValue<RouteValueDictionary> LoginReturnPage {
			get { return new TempDataValue<RouteValueDictionary>("LoginReturnPage", _tempData); }
		}

		public TempDataValue<ErrorMessages> ErrorMessages {
			get { return new TempDataValue<ErrorMessages>("ErrorMessages", _tempData); }
		}
	}
}