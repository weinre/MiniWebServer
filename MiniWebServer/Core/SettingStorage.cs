using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace MiniWebServer.Core
{
    public class SettingStorage<T>
    {
        private const string FileName = "data.json";

        public IEnumerable<T> Load()
        {
            if (File.Exists(FileName))
            {
                var json = File.ReadAllText(FileName);
                return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
            }

            return new List<T>();
        }

        public void Save(IEnumerable<T> dataList)
        {
            var json = JsonConvert.SerializeObject(dataList);
            File.WriteAllText(FileName, json);
        }

        public void Add(T data)
        {
            var dataList = Load().ToList();
            dataList.Add(data);
            Save(dataList);
        }
    }
}