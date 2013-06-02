using System;
using System.Collections.Generic;
using System.Linq;
using THCard.Common;

namespace THCard.AccountManagement {
	public interface IUserRepository {
		User GetUser(UserId userId);
		User CreateUser(FullName fullName);
	}

	public sealed class UserId : GuidId {
		public UserId(Guid id) : base(id, "User") {}
	}

	public sealed class User {
		public User(UserId userId, FullName fullName) {
			Id = userId;
			FullName = fullName;
		}

		public UserId Id { get; private set; }
		public FullName FullName { get; private set; }
	}

	public sealed class Name {
		public static readonly Name Empty = new Name(string.Empty);
		private readonly string _name;

		public Name(string name) {
			_name = name;
		}

		public override string ToString() {
			return _name;
		}
	}

	public sealed class FullName {
		public Name FamilyName { get; set; }
		public GivenNames GivenNames { get; set; }

		public Name FirstName {
			get { return GivenNames.FirstName; }
		}

		public Name MiddleName {
			get { return GivenNames.MiddleName; }
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
	}
}