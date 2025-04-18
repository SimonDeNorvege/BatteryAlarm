using System;
using System.Windows.Forms;
using System.Media;


namespace BatteryAlarm
{
    public partial class BatteryAlarm : Form
    {
        
        private const int BATTERY_FIFTY = 50;
        private const int BATTERY_FOURTY = 40;
        private const int BATTERY_THIRTY = 30;
        private const int BATTERY_TWENTY = 20;
        private const int BATTERY_TEN = 10;
        private const int BATTERY_FIVE = 5;
        
        
        private const int BATTERY_LOW = 30;
        private const int BATTERY_MID = 50;
        private const int BATTERY_FULL = 90;

        private NotifyIcon notifyIcon = null!;
        private ContextMenuStrip contextMenu = null!;
        private Label percentLabel = null!;
        private Label statusLabel = null!;
        private List<SoundPlayer> sounds = null!;
        public BatteryAlarm()
        {
            InitializeComponent();
            ConfigureForm();
            InitializeLabels();
            InitializeSounds();

            //Première Instanciation de UpdateBattery 
            UpdateBatteryInfo();
            TimerUpdate();
            InitializeTrayIcon();
            
        }

        private void InitializeTrayIcon()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = SystemIcons.Information; // Remplace par une vraie icône si tu veux
            notifyIcon.Text = "Battery Alarm";
            notifyIcon.Visible = true;

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
            // Si l'utilisateur clique sur la croix (Alt+F4 ou bouton X)
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide(); // On cache la fenêtre
            }
        }



        private void ShowWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }



        void InitializeSounds()
        {
            this.sounds = new List<SoundPlayer>();
            sounds.Add(new SoundPlayer("sounds/fifty.wav"));
            sounds.Add(new SoundPlayer("sounds/fourty.wav"));
            sounds.Add(new SoundPlayer("sounds/thirty.wav"));
            sounds.Add(new SoundPlayer("sounds/twenty.wav"));            
            sounds.Add(new SoundPlayer("sounds/ten.wav"));
            sounds.Add(new SoundPlayer("sounds/five.wav"));
        }

        /*
        * Lance un timer qui regarde l'état de la batterie toutes les demi-secondes
        */
        private void TimerUpdate()
        {
            //deux timer existent, précision du timer spécifique
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 500;
            timer.Tick += (s, e) =>
            {
                UpdateBatteryInfo();

                //remise à zéro du timer 
                timer.Stop();
                timer.Start();
            };
            timer.Start();

        }

        private void ConfigureForm()
        {
            this.FormClosing += OnFormClosing;
            this.Resize += OnResize;
            this.Text = "BatteryAlarm";
            this.Width = 300;
            this.Height = 200;
        }

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

        private void UpdateBatteryInfo()
        {
            PowerStatus status = SystemInformation.PowerStatus;

            
            float batteryLevel = status.BatteryLifePercent * 100;

            percentLabel.Text = GetBatteryStatusText(batteryLevel);
            
            string powerLine = SystemInformation.PowerStatus.PowerLineStatus.ToString();
            statusLabel.Text = powerLine == "Online"
            ? "En charge ⚡"
            : "Sur batterie 🔋";
        }

        private string GetBatteryStatusText(float batteryLevel)
        {
            if (batteryLevel <= BATTERY_FIFTY)
                PlaySound(sounds[(int)SoundLevel.Fifty]);
            else if (batteryLevel <= BATTERY_FOURTY)
                PlaySound(sounds[(int)SoundLevel.Fourty]);
            else if (batteryLevel <= BATTERY_THIRTY)
                PlaySound(sounds[(int)SoundLevel.Thirty]);
            else if (batteryLevel <= BATTERY_TWENTY)
                PlaySound(sounds[(int)SoundLevel.Twenty]);
            else if (batteryLevel <= BATTERY_TEN)
                PlaySound(sounds[(int)SoundLevel.Ten]);
            return $"Battery over {BATTERY_FULL}% ({batteryLevel:0}%)";
        }

        public void PlaySound(SoundPlayer sound)
        {
            try
                {
                    sound.Load(); // Peut lever une exception si le fichier est corrompu
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Erreur lors du chargement du son : {ex.Message}");
                }
                sound.Play();
        }
    }
}
