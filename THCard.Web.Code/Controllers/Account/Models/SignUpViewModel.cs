using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace THCard.Web.Controllers.Account.Models {
	public class SignUpViewModel {
		public AccountRegistration Information { get; set; }
		public IEnumerable<string> Errors { get; set; }
		public ModelStateDictionary ModelState { get; set; }
	}
}