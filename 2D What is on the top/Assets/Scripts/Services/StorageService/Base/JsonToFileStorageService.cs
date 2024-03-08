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

/// Как я хочу чтобы было:
/// Я хочу чтобы прокинуть нужный мне класс и там уже готовые методы которые сохранеет разного рода данные например сохранения рессурсов, сохранения внутреигровую валюту, 
///
/// Что у меня есть :
/// - у меня есть сохранения в файл под IStorageService
/// - также у меня сейчаст методы сохранения рессурсов работает в классе Score - который по сути не должен этим заниматся
///
/// Что я могу сделать :
/// я могу сделать медиатор сохранений разного рода такие как сохранения рессурсов и тп
/// что мне это даст - это избавит нас от доп логики сохранения данных как минимум в классе S
/// 
