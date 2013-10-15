using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using THCard.Common;

namespace THCard.AccountManagement {
	public interface IUserRepository {
		User GetUser(UserId userId);
		User GetUser(AccountId accountId);
		void SaveUser(User user);
		void CreateUser(User user);
	}

	public sealed class UserId : GuidId {
		public static readonly UserId New = new UserId(Guid.Empty);
		public UserId(Guid id) : base(id, "User") {}
	}

	public sealed class User {
		private UserId _id;

		public User(UserId userId, FullName fullName) {
			Id = userId;
			FullName = fullName;
		}

		public UserId Id {
			get { return this._id; }
			set {
				Contract.Requires(value != null && !value.IsNew);
				this._id = value;
			}
		}

		public FullName FullName { get; private set; }
	}

	public sealed class Name {
		private readonly string _name;

		public Name(string name) {
			Contract.Requires(!string.IsNullOrWhiteSpace(name));
			this._name = name;
		}

		public override string ToString() {
			return this._name;
		}

		private bool Equals(Name other) {
			return string.Equals(this._name, other._name, StringComparison.InvariantCultureIgnoreCase);
		}

		public override bool Equals(object other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other is Name && Equals((Name) other);
		}

		public override int GetHashCode() {
			return (this._name != null ? this._name.GetHashCode() : 0);
		}
	}

	public sealed class FullName {
		public FullName(Name familyName, GivenNames givenNames = null) {
			Contract.Requires(familyName != null);
			FamilyName = familyName;
			GivenNames = givenNames;
		}

		public Name FamilyName { get; private set; }
		public GivenNames GivenNames { get; private set; }

		public Name FirstName {
			get { return GivenNames.FirstName; }
		}

		public Name MiddleName {
			get { return GivenNames.MiddleName; }
		}

		public override int GetHashCode() {
			unchecked {
				return (FamilyName.GetHashCode() * 397) ^ (GivenNames != null ? GivenNames.GetHashCode() : 0);
			}
		}

		public override bool Equals(object other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other is FullName && Equals((FullName) other);
		}

		private bool Equals(FullName other) {
			return Equals(this.FamilyName, other.FamilyName) && Equals(this.GivenNames, other.GivenNames);
		}

		public override string ToString() {
			return string.Join(" ", GivenNames.ToString(), FamilyName.ToString());
		}
	}

	public sealed class GivenNames {
		private readonly IList<Name> _names;

		public GivenNames([NotNull] IEnumerable<string> names) {
			Contract.Requires(names != null);
			this._names = new List<Name>(names.Select(n => new Name(n)));
		}

		public GivenNames([NotNull] params string[] names) : this((IEnumerable<string>) names) {}

		public Name FirstName {
			get { return this._names.FirstOrDefault(); }
		}

		public Name MiddleName {
			get { return this._names.Skip(1).FirstOrDefault(); }
		}

		private bool Equals([NotNull] GivenNames other) {
			if (this._names.Count != other._names.Count) return false;
			var thisEnumerator = this._names.GetEnumerator();
			var otherEnumerator = other._names.GetEnumerator();
			while (thisEnumerator.MoveNext() && otherEnumerator.MoveNext()) {
				if (!Equals(thisEnumerator.Current, otherEnumerator.Current)) return false;
			}
			return true;
		}

		public override bool Equals(object other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other is GivenNames && Equals((GivenNames) other);
		}

		public override int GetHashCode() {
			return (this._names != null ? this._names.GetHashCode() : 0);
		}

		public override string ToString() {
			return string.Join(" ", this._names);
		}
	}
}