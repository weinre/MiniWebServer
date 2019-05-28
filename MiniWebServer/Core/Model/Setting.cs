using System;

namespace MiniWebServer.Core.Model
{
    public class Setting
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public int Port { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as Setting;

            if (item == null)
            {
                return false;
            }

            return this.Port == item.Port;
        }

        public override int GetHashCode()
        {
            return this.Port.GetHashCode();
        }
    }
}