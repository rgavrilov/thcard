using System;
using System.Diagnostics.Contracts;
using THCard.AccountManagement;

namespace THCard.Web {
	public sealed class LoginAttemptResult {
		private Account _account;
		private int _failedAttemptCount;
		private LoginFailureReason _failureReason;

		public bool Succeeded { get; private set; }

		public Account Account {
			get {
				Contract.Requires(Succeeded);
				return _account;
			}
			private set { _account = value; }
		}

		public int FailedAttemptCount {
			get {
				Contract.Requires(!Succeeded && FailureReason == LoginFailureReason.IncorrectPassword);
				return _failedAttemptCount;
			}
			private set { _failedAttemptCount = value; }
		}

		public LoginFailureReason FailureReason {
			get {
				Contract.Requires(!Succeeded);
				return _failureReason;
			}
			private set { _failureReason = value; }
		}

		public override string ToString() {
			return Succeeded ? ToStringSuccessfullAttempt() : ToStringFailedAttempt();
		}

		private string ToStringFailedAttempt() {
			switch (FailureReason) {
				case LoginFailureReason.AccountLockedOut:
					return string.Format("Failed: account {0} is locked out.");
				case LoginFailureReason.AccountNotActivated:
					return string.Format("Failed: account not activated.");
				case LoginFailureReason.IncorrectPassword:
					return string.Format("Failed: password mismatch.");
				case LoginFailureReason.UsernameNotFound:
					return string.Format("Failed: username not found.");
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private string ToStringSuccessfullAttempt() {
			return string.Format("Logged in as {0}", Account);
		}

		public static LoginAttemptResult Success(Account account) {
			return new LoginAttemptResult {Account = account, Succeeded = true};
		}

		public static LoginAttemptResult AccountLockedOut() {
			return new LoginAttemptResult {Succeeded = false, FailureReason = LoginFailureReason.AccountLockedOut};
		}

		public static LoginAttemptResult AccountNotActivated() {
			return new LoginAttemptResult {Succeeded = false, FailureReason = LoginFailureReason.AccountNotActivated};
		}

		public static LoginAttemptResult IncorrectPassword(int failedAttemptCount) {
			return new LoginAttemptResult {
				Succeeded = false,
				FailedAttemptCount = failedAttemptCount,
				FailureReason = LoginFailureReason.IncorrectPassword
			};
		}

		public static LoginAttemptResult UsernameNotFound() {
			return new LoginAttemptResult {
				Succeeded = false,
				FailureReason = LoginFailureReason.UsernameNotFound
			};
		}
	}

	public enum LoginFailureReason {
		AccountLockedOut,
		AccountNotActivated,
		IncorrectPassword,
		UsernameNotFound
	}
}