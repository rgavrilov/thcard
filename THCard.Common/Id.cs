using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace THCard.Common {

	public abstract class IdBase {
		public abstract bool IsNew { get; }
	}

	[DebuggerDisplay("{0}#{1}")]
	public abstract class Id : IdBase, IEquatable<Id> {
		private readonly string _internalId;
		private readonly string _itemType;

		protected Id(string id, string itemType) {
			_internalId = id;
			_itemType = itemType;
		}

		public bool Equals(Id other) {
			if (other == null || GetType() != other.GetType()) {
				return false;
			}

			return string.Equals(_internalId, other._internalId, StringComparison.Ordinal);
		}

		public override bool Equals(object other) {
			return Equals(other as Id);
		}

		public override int GetHashCode() {
			return _internalId.GetHashCode();
		}

		public override string ToString() {
			return _internalId;
		}
	}
}