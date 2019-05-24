using System.Drawing;
using System.Windows.Forms;
using MiniWebServer.Core;

namespace MiniWebServer.Components
{
    public partial class WebListItem : UserControl
    {
        private Color _stoppingColor = Color.LightCoral;

        private Color _runningColor = Color.LightGreen;

        private readonly MyHttpServer _server;

        public WebListItem(string name, string path, int port)
        {
            InitializeComponent();
            lblName.Text = name;
            lblPath.Text = port + "  " + path;
            _server = new MyHttpServer(path, port);
        }

        public void Stop()
        {
            _server.Stop();
        }

        private void btnStart_Click(object sender, System.EventArgs e)
        {
            if (_server.IsRunning)
            {
                _server.Stop();
                btnStart.Text = "Start";
                this.BackColor = _stoppingColor;
            }
            else
            {
                _server.Start();
                btnStart.Text = "Stop";
                this.BackColor = _runningColor;
            }
        }
    }
}