using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using THCard.Web.Infrastructure;
using ControllerBase = THCard.Web.Infrastructure.ControllerBase;

namespace THCard.Web.Controllers.LoginControl {
	public class LoginControlController : ControllerBase {
		private readonly IEnumerable<Widget> _widgetVariations;
		private readonly ISession _session;

		public LoginControlController(ISession session) {
			_session = session;

			_widgetVariations = new[] {
				new Widget(s => s.IsAuthenticated, s => Result("Authenticated", s.AuthenticatedAccount)),
				new Widget(s => !s.IsAuthenticated, s => View("Unauthenticated"))
			};
		}

		[ChildActionOnly]
		public ActionResult LoginControl() {
			Widget widget = _widgetVariations.First(w => w.CanHandle(_session));
			return widget.Render(_session);
		}

		private class Widget {
			private readonly Func<ISession, bool> _canHandle;
			private readonly Func<ISession, ActionResult> _render;

			public Widget(Func<ISession, bool> canHandle, Func<ISession, ActionResult> render) {
				_canHandle = canHandle;
				_render = render;
			}

			public bool CanHandle(ISession session) {
				return _canHandle(session);
			}

			public ActionResult Render(ISession session) {
				return _render(session);
			}
		}
	}
}