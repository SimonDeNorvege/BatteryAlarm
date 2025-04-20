namespace BatteryAlarm
{
    public partial class BatteryAlarm
    {
        private void InitializeLabels()
        {
            percentLabel = new Label()
            {
                Location = new System.Drawing.Point(50, 50),
                AutoSize = true
            };
            this.Controls.Add(percentLabel);

            statusLabel = new Label()
            {
                Location = new System.Drawing.Point(50, 30),
                AutoSize = true
            };
            this.Controls.Add(statusLabel);
        }

        private void ConfigureForm()
        {
            this.FormClosing += OnFormClosing;
            this.Resize += OnResize;
            this.Text = "BatteryAlarm";
            this.Width = 300;
            this.Height = 200;
        }

        private void InitializeTrayIcon()
        {
            notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Warning,
                Text = "Battery Alarm",
                Visible = true
            };

            contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Afficher", null, (s, e) => ShowWindow());
            contextMenu.Items.Add("Quitter", null, (s, e) => Application.Exit());

            notifyIcon.ContextMenuStrip = contextMenu;
            notifyIcon.DoubleClick += (s, e) => ShowWindow();
        }

        private void OnResize(object? sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void OnFormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void ShowWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }
    }

    
}