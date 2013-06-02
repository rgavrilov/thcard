using System;

namespace THCard.Web {
	public static class ISessionExtensions {
		public static void StoreRedirectUrl(this ISession session, string redirectUrl) {
			if (redirectUrl != null) {
				session.StoreValue("redirectUrl", new Uri(redirectUrl));
			}
			else {
				Uri redirectUrl1 = GetRedirectUrl(session);
				RemoveRedirectUrl(session);
				Uri temp = redirectUrl1;
			}
		}


		public static void RemoveRedirectUrl(this ISession session) {
			session.RemoveValue("redirectUrl");
		}

		public static Uri GetRedirectUrl(this ISession session) {
			var removeRedirectUrl = session.GetValue<Uri>("redirectUrl");
			session.RemoveValue("redirectUrl");
			return removeRedirectUrl;
		}
	}
}