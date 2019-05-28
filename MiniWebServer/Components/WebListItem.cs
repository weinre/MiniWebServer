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

        private MyHttpServer _server;

        public delegate void RemoveEventHandler(WebListItem sender);

        public event RemoveEventHandler OnRemove;

        public delegate void UpdateEventHandler(WebListItem sender);

        public event UpdateEventHandler OnUpdate;

        public WebListItem(Setting setting)
        {
            InitializeComponent();
            UpdateSetting(setting);
        }

        public Setting Setting { get; private set; }

        public void UpdateSetting(Setting setting)
        {
            if(_server != null && _server.IsRunning)
            {
                Stop();
            }

            Setting = setting;
            lblName.Text = Setting.Name;
            lblPath.Text = Setting.Port + "  " + Setting.Path;
            _server = new MyHttpServer(Setting.Path, Setting.Port);
        }

        public void Stop()
        {
            _server.Stop();
            btnStart.Text = "Start";
            this.BackColor = _stoppingColor;
        }

        private void btnStart_Click(object sender, System.EventArgs e)
        {
            if (_server.IsRunning)
            {
                Stop();
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

        private void btnUpdate_Click(object sender, System.EventArgs e)
        {
            OnUpdate(this);
        }
    }
}