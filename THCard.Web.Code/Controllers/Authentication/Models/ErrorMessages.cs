using System;
using System.Collections.Generic;

namespace THCard.Web.Controllers.Authentication.Models {
	[Serializable]
	public class ErrorMessages : List<string> {
		public static readonly ErrorMessages Empty = new ErrorMessages();
	}
}