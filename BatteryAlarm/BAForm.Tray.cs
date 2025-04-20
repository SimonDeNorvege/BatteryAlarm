namespace BatteryAlarm
{
    public partial class BatteryAlarm
    {
        /// <summary>
        ///  Initialize the labels for BatteryAlarm
        /// </summary>
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