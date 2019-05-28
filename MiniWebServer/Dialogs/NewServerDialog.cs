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
            numericUpDown1.Enabled = true;
        }

        public NewServerDialog(Setting setting): this()
        {
            txtName.Text = setting.Name;
            txtPath.Text = setting.Path;
            numericUpDown1.Value = setting.Port;
            numericUpDown1.Enabled = false;
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
            if (numericUpDown1.Enabled && _settingStorage.Contains((int)numericUpDown1.Value))
            {
                MessageBox.Show("Duplicate port", "Error");
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}