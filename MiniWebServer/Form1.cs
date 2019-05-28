using System;
using System.Windows.Forms;
using MiniWebServer.Components;
using MiniWebServer.Core;
using MiniWebServer.Core.Model;
using MiniWebServer.Dialogs;

namespace MiniWebServer
{
    public partial class Form1 : Form
    {
        private readonly SettingStorage<Setting> _settingStorage;

        public Form1()
        {
            InitializeComponent();
            _settingStorage = new SettingStorage<Setting>();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadServers();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            StopAllServers();
        }

        private void LoadServers()
        {
            var servers = _settingStorage.Load();
            foreach (var server in servers)
            {
                AddServer(server);
            }
        }

        private void StopAllServers()
        {
            foreach (var control in flowLayoutPanel1.Controls)
            {
                var webListItem = control as WebListItem;
                if (webListItem != null)
                {
                    webListItem.Stop();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var dialog = new NewServerDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var setting = new Setting
                    {
                        Name = dialog.ServerName,
                        Path = dialog.Path,
                        Port = dialog.Port
                    };
                    AddServer(setting);
                    _settingStorage.Add(setting);
                }
            }
        }

        private void AddServer(Setting setting)
        {
            var item = new WebListItem(setting);            
            item.Width = flowLayoutPanel1.Width - 25;
            item.OnRemove += Item_OnRemove;
            flowLayoutPanel1.Controls.Add(item);
        }

        private void Item_OnRemove(object sender)
        {
            var item = sender as WebListItem;
            item.Stop();
            flowLayoutPanel1.Controls.Remove(item);
            _settingStorage.Remove(item.Setting);
            item.Dispose();
        }
    }
}