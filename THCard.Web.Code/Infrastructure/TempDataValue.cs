using System.Web.Mvc;

namespace THCard.Web.Infrastructure {
	public sealed class TempDataValue<T> {
		private readonly TempDataDictionary _tempData;

		public TempDataValue(string key, TempDataDictionary tempData) {
			_tempData = tempData;
			Key = key;
		}

		public string Key { get; private set; }

		public bool Stored {
			get { return _tempData.ContainsKey(Key); }
		}

		public static implicit operator T(TempDataValue<T> value) {
			return value.Get();
		}

		public T Get() {
			return (T) _tempData[Key];
		}

		public void Store(T value) {
			_tempData[Key] = value;
		}

		public void Keep() {
			_tempData.Keep(Key);
		}
	}
}