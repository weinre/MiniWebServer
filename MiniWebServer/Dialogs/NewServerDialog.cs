using System;
using System.Windows.Forms;
using MiniWebServer.Core;
using MiniWebServer.Core.Model;

namespace MiniWebServer.Dialogs
{
    public partial class NewServerDialog : Form
    {
        private readonly SettingStorage _settingStorage;

        public NewServerDialog()
        {
            InitializeComponent();
            _settingStorage = new SettingStorage();
        }

        public string ServerName
        {
            get
            {
                return txtName.Text;
            }
        }

        public string Path
        {
            get
            {
                return txtPath.Text;
            }
        }

        public int Port
        {
            get
            {
                return (int)numericUpDown1.Value;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (_settingStorage.Contains((int)numericUpDown1.Value))
            {
                MessageBox.Show("Duplicate port", "Error");
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}