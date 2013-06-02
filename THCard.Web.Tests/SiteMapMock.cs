using Moq;
using THCard.AccountManagement;

namespace THCard.Web.Tests {
	public sealed class SiteMapMock {
		private readonly Mock<ISiteMap> _siteMapMock;

		public SiteMapMock() {
			_siteMapMock = new Mock<ISiteMap>();
			_siteMapMock.Setup(it => it.GetLandingPage(It.IsAny<Account>())).Returns(AccountLandingPage);
			_siteMapMock.Setup(it => it.GetLoginPage()).Returns(LoginPage);
			_siteMapMock.Setup(it => it.GetPublicLandingPage()).Returns(PublicLandingPage);
		}

		public ISiteMap Object {
			get { return _siteMapMock.Object; }
		}

		public PageRoute LoginPage {
			get { return new PageRoute("Login"); }
		}

		public PageRoute AccountLandingPage {
			get { return new PageRoute("AccountLandingPage"); }
		}

		public PageRoute PublicLandingPage {
			get { return new PageRoute("PublicLandingPage"); }
		}
	}
}