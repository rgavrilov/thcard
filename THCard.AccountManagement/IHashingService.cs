using THCard.AccountManagement;

namespace Try {
	public interface IHashingService {
		HashedPassword Hash(Password password);
		bool Equals(Password password, HashedPassword hashedPassword);
	}
}