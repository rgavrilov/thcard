using System;

namespace THCard.Common {
	public abstract class Id : IEquatable<Id> {
		private readonly string _internalId;
		private readonly string _itemType;

		protected Id(string id, string itemType) {
			_internalId = id;
			_itemType = itemType;
		}

		protected string InternalId {
			get { return _internalId; }
		}

		public bool Equals(Id other) {
			if (other == null || GetType() != other.GetType()) {
				return false;
			}

			return string.Equals(_internalId, other._internalId, StringComparison.Ordinal);
		}

		public override bool Equals(object other) {
			return this.Equals(other as Id);
		}

		public override int GetHashCode() {
			return _internalId.GetHashCode();
		}

		public override string ToString() {
			return string.Format("{0} #{1}", _itemType, _internalId);
		}
	}
}