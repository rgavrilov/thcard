using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using THCard.Common;

namespace THCard.AccountManagement {
	public interface IUserRepository {
		User GetUser(UserId userId);
		User GetUser(AccountId accountId);
		void SaveUser(User user);
		void CreateUser(AccountManagement.User user);
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
			get { return _id; }
			set {
				Contract.Requires(value != null && !value.IsNew);
				_id = value;
			}
		}

		public FullName FullName { get; private set; }
	}

	public sealed class Name {
		public static readonly Name Empty = new Name(string.Empty);
		private readonly string _name;

		public Name(string name) {
			Contract.Requires(!string.IsNullOrWhiteSpace(name));
			_name = name;
		}

		public override string ToString() {
			return _name;
		}

		private bool Equals(Name other) {
			return string.Equals(_name, other._name, StringComparison.InvariantCultureIgnoreCase);
		}

		public static bool Equals(Name x, Name y) {
			if (ReferenceEquals(x, y)) {
				return true;
			}
			if (x == null || y == null) {
				return false;
			}
			return x.Equals(y);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) {
				return false;
			}
			if (ReferenceEquals(this, obj)) {
				return true;
			}
			return obj is Name && Equals((Name) obj);
		}

		public override int GetHashCode() {
			return _name.GetHashCode();
		}
	}

	public sealed class FullName {
		public FullName(Name familyName, GivenNames givenNames = null) {
			Contract.Requires(familyName != null);
			FamilyName = familyName;
			GivenNames = givenNames;
		}

		public Name FamilyName { get; set; }
		public GivenNames GivenNames { get; set; }

		public Name FirstName {
			get { return GivenNames.FirstName; }
		}

		public Name MiddleName {
			get { return GivenNames.MiddleName; }
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) {
				return false;
			}
			if (ReferenceEquals(this, obj)) {
				return true;
			}
			return obj is FullName && Equals((FullName) obj);
		}

		private bool Equals(FullName obj) {
			return Name.Equals(FamilyName, obj.FamilyName) && GivenNames.Equals(GivenNames, obj.GivenNames);
		}

		public override string ToString() {
			return string.Join(" ", GivenNames.ToString(), FamilyName.ToString());
		}
	}

	public class GivenNames {
		private readonly IList<Name> _names;

		public GivenNames(IEnumerable<string> names) {
			_names = new List<Name>(names.Select(n => new Name(n)));
		}

		public GivenNames(params string[] names) : this((IEnumerable<string>) names) {}

		public Name FirstName {
			get { return _names.Count >= 1 ? _names[0] : Name.Empty; }
		}

		public Name MiddleName {
			get { return _names.Count >= 2 ? _names[1] : Name.Empty; }
		}

		public static bool Equals(GivenNames x, GivenNames y) {
			if (ReferenceEquals(x, y)) {
				return true;
			}
			if (x == null || y == null) {
				return false;
			}
			if (x._names.Count != y._names.Count) {
				return false;
			}
			for (int index = 0; index < x._names.Count; ++index) {
				if (!Name.Equals(x._names[index], y._names[index])) {
					return false;
				}
			}
			return true;
		}

		public override string ToString() {
			return string.Join(" ", _names);
		}
	}
}