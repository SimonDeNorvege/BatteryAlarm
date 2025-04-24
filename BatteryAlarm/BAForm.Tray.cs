namespace BatteryAlarm
{
    public partial class BatteryAlarm
    {
        /// <summary>
        ///  Initialize the labels for BatteryAlarm
        /// </summary>
        private void InitializeLabels()
        {
            _percentLabel = new Label()
            {
                Location = new System.Drawing.Point(50, 50),
                AutoSize = true
            };
            this.Controls.Add(_percentLabel);

            _statusLabel = new Label()
            {
                Location = new System.Drawing.Point(50, 30),
                AutoSize = true
            };
            this.Controls.Add(_statusLabel);
        }

        /// <summary>
        /// Configure the form displayed
        /// </summary>
        private void ConfigureForm()
        {
            this.FormClosing += OnFormClosing;
            this.Resize += OnResize;
            this.Text = "BatteryAlarm";
            this.Width = 300;
            this.Height = 200;
        }

        /// <summary>
        /// Intialize the Tray
        /// </summary>
        private void InitializeTrayIcon()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Warning,
                Text = "Battery Alarm",
                Visible = true
            };

            _contextMenu = new ContextMenuStrip();
            _contextMenu.Items.Add("Afficher", null, (s, e) => ShowWindow());
            _contextMenu.Items.Add("Quitter", null, (s, e) => Application.Exit());

            _notifyIcon.ContextMenuStrip = _contextMenu;
            _notifyIcon.DoubleClick += (s, e) => ShowWindow();
        }

        /// <summary>
        /// Resize the window 
        /// </summary>
        /// <param name="sender"> "this" </param>
        /// <param name="e"> </param>
        private void OnResize(object? sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        /// <summary>
        /// Ignore la fin de l'application et la cache dans les icones cachés
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        /// <summary>
        /// Quand la fenêtre est cachée, permet de la reafficher
        /// </summary>
        private void ShowWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }
    }

    
}