using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Services.StorageService
{
    public class JsonToFileStorageService : IStorageService
    {
        public void Save(StorageKeysType keyType, object data, Action<bool> callBack = null)
        {
            var path = BuildPath(keyType.ToString());
            var json = JsonConvert.SerializeObject(data);

            using (var filestream = new StreamWriter(path))
            {
                filestream.Write(json);
            }
            
            callBack?.Invoke(true);
        }

        public void Load<T>(StorageKeysType keyType, Action<T> callBack)
        {
            var path = BuildPath(keyType.ToString());

            if (File.Exists(path) == false)
            {
                callBack?.Invoke(default(T));
                Debug.LogWarning("load file couldn't fount, be carefuly");
                return;
            }

            using (var fileReader = new StreamReader(path))
            {
                var json = fileReader.ReadToEnd();
                var data = JsonConvert.DeserializeObject<T>(json);
                callBack.Invoke(data);
            }
        }

        private string BuildPath(string key) => Path.Combine(Application.persistentDataPath, key);
    }
}