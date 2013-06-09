using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Web.Mvc;
using THCard.Web.Controllers.Account.Models;
using THCard.Web.Infrastructure;

namespace THCard.Web.Controllers.Account {
	[PresenterFor(typeof (AccountController))]
	public class AccountPresenter {
		public SignUpViewModel SignUp(SignUpActionData actionData) {
			var viewModel = new SignUpViewModel();

			var modelStates = new ModelStateDictionary();

			foreach (var reason in actionData.RegistrationFailureReasons) {
				switch (reason) {
					case RegistrationFailureReason.UsernameNotAvailable:
						modelStates.AddModelError("username", GetRegistrationFailureReasonText(RegistrationFailureReason.UsernameNotAvailable));
						break;
					case RegistrationFailureReason.PasswordsDoNotMatch:
						modelStates.AddModelError("passwordConfirmation", GetRegistrationFailureReasonText(RegistrationFailureReason.PasswordsDoNotMatch));
						break;
					case RegistrationFailureReason.EmailsDoNotMatch:
						modelStates.AddModelError("email", GetRegistrationFailureReasonText(RegistrationFailureReason.EmailsDoNotMatch));
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			IEnumerable<string> errors = actionData.RegistrationFailureReasons.Select(GetRegistrationFailureReasonText);
			viewModel.Errors = errors;
			viewModel.ModelState = modelStates;
			return viewModel;
		}

		private string GetRegistrationFailureReasonText(RegistrationFailureReason reason) {
			switch (reason) {
				case RegistrationFailureReason.UsernameNotAvailable:
					return "Username is not available.";
				case RegistrationFailureReason.PasswordsDoNotMatch:
					return "Passwords must match.";
				case RegistrationFailureReason.EmailsDoNotMatch:
					return "E-mails must match.";
				default:
					throw new ArgumentOutOfRangeException("reason");
			}
		}
	}
}