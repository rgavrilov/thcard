using System.Linq;
using System.Collections.Generic;
using System;

namespace THCard.Dal.Common {
	public abstract class RepositoryBase {
		protected void AssertFound<T>(T dbItem) where T : class {
			if (dbItem == null) {
				throw new InvalidOperationException("Item was not found in the database.");
			}
		}
	}
}