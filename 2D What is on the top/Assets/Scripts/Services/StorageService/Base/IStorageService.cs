using System;

namespace Services.StorageService
{
    public interface IStorageService
    {
        public void Save(StorageKeysType key, object data, Action<bool> callBack = null);
        public void Load<T>(StorageKeysType key, Action<T> callBack);
    }
}