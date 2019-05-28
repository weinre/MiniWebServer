using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MiniWebServer.Core.Model;
using Newtonsoft.Json;

namespace MiniWebServer.Core
{
    public class SettingStorage
    {
        private const string FileName = "data.json";

        private static readonly Lazy<HashSet<Setting>> _dataList = new Lazy<HashSet<Setting>>(() => Load());

        private void Save()
        {
            var json = JsonConvert.SerializeObject(DataList);
            File.WriteAllText(FileName, json);
        }

        private static HashSet<Setting> DataList
        {
            get { return _dataList.Value; }
        }

        public IEnumerable<Setting> GetServers()
        {
            return DataList.AsEnumerable();
        }

        public bool Contains(int port)
        {
            return DataList.Contains(new Setting { Port = port });
        }

        public bool Add(Setting item)
        {
            if(!DataList.Add(item))
            {
                return false;
            }

            Save();
            return true;
        }

        public bool Update(Setting item)
        {
            var setting = DataList.First(x => x.Port == item.Port);
            if(setting == null)
            {
                return false;
            }

            setting.Name = item.Name;
            setting.Path = item.Path;
            Save();
            return true;
        }

        public void Remove(Setting item)
        {
            DataList.Remove(item);
            Save();
        }


        private static HashSet<Setting> Load()
        {
            if (File.Exists(FileName))
            {
                var json = File.ReadAllText(FileName);
                return JsonConvert.DeserializeObject<HashSet<Setting>>(json);
            }
            
            return new HashSet<Setting>();
        }
    }
}