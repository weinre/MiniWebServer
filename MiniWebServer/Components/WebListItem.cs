using System.Drawing;
using System.Windows.Forms;
using MiniWebServer.Core;
using MiniWebServer.Core.Model;

namespace MiniWebServer.Components
{
    public partial class WebListItem : UserControl
    {
        private readonly Color _stoppingColor = Color.LightCoral;

        private readonly Color _runningColor = Color.LightGreen;

        private readonly MyHttpServer _server;

        public delegate void RemoveEventHandler(object sender);

        public event RemoveEventHandler OnRemove;

        public WebListItem(Setting setting)
        {
            InitializeComponent();
            Setting = setting;
            lblName.Text = Setting.Name;
            lblPath.Text = Setting.Port + "  " + Setting.Path;
            _server = new MyHttpServer(Setting.Path, Setting.Port);
        }

        public Setting Setting { get; private set; }

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

        private void btnRemove_Click(object sender, System.EventArgs e)
        {
            OnRemove(this);
        }
    }
}