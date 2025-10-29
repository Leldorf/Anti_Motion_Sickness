using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace AntiMotionSickness
{
    internal class ConfigFileManager
    {
        string filePath;
        Dictionary<string, string> configData;

        static public T DecodeData<T>(string str)
        {
            T config = JsonConvert.DeserializeObject<T>(str);
            return config;
        }

        static public string EncodeData<T>(T config)
        {
            string encodedStr = JsonConvert.SerializeObject(config);

            return encodedStr;
        }

        public ConfigFileManager()
        {
            filePath = Environment.CurrentDirectory.ToString() + "/config.json";
            if (File.Exists(filePath))
            {
                using (StreamReader reader = File.OpenText(filePath))
                {
                    configData = DecodeData<Dictionary<string, string>>(reader.ReadToEnd());
                }
            }
            else
            {
                configData = new Dictionary<string, string>();
            }
        }

        public List<string> GetConfigIndex()
        {
            return configData.Keys.ToList();
        }

        public bool GetConfigData(string key, out string value)
        {
            return configData.TryGetValue(key, out value);
        }

        public bool GetConfigData<T>(string key, out T value)
        {
            string str;
            var flag = GetConfigData(key, out str);
            value = flag ? DecodeData<T>(str) : default(T);
            return flag;
        }

        public void SetConfigData(string key, string value)
        {
            configData[key] = value;
        }

        public void SetConfigData<T>(string key, T value)
        {
            SetConfigData(key, EncodeData(value));
        }

        public void RemoveConfig(string key)
        {
            configData.Remove(key);
        }

        public void SaveData()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.Write(EncodeData(configData));
                }
            }
            catch
            {
                Log.Write("config save error");
            }
        }

        ~ConfigFileManager()
        {
            SaveData();
        }
    }
}
