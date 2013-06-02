using System;
using System.Diagnostics;

namespace THCard.Common {
	[DebuggerDisplay("{_itemType} #{_id}")]
	public abstract class GuidId : IEquatable<GuidId> {
		private readonly Guid _id;
		private readonly string _itemType;

		protected GuidId(Guid id, string itemType) {
			_id = id;
			_itemType = itemType;
		}

		public bool Equals(GuidId other) {
			if (other == null || GetType() != other.GetType()) {
				return false;
			}

			return _id == other._id;
		}

		public override bool Equals(object other) {
			if (other == null || GetType() != other.GetType()) {
				return false;
			}

			var that = (GuidId) other;

			return _id == that._id;
		}

		public override int GetHashCode() {
			return _id.GetHashCode();
		}

		public override string ToString() {
			return _id.ToString();
		}

		public Guid ToGuid() {
			return _id;
		}
	}
}