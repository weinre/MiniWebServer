using System;
using System.Windows.Forms;

namespace MiniWebServer.Dialogs
{
    public partial class NewServerDialog : Form
    {
        public NewServerDialog()
        {
            InitializeComponent();
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
    }
}