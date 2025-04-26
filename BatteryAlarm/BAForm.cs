using System.Media;
using Microsoft.Win32;

namespace BatteryAlarm
{
    public partial class BatteryAlarm : Form
    {

        private const int BatteryFifty = 50;
        private const int BatteryForty = 40;
        private const int BatteryThirty = 30;
        private const int BatteryTwenty = 20;
        private const int BatteryTen = 10;
        private const int BatteryFive = 5;

        private NotifyIcon _notifyIcon = null!;
        private ContextMenuStrip _contextMenu = null!;
        private Label _percentLabel = null!;
        private Label _statusLabel = null!;

        private System.Windows.Forms.Timer _timer = null!;

       

        /// <summary>
        /// Constructeur de BatteryAlarm
        /// </summary>
        public BatteryAlarm()
        {
            InitializeComponent();
            ConfigureForm();
            InitializeLabels();
            InitializeSounds();
            InitializeTrayIcon();

            // Joue un son au démarrage
            PlaySound(new SoundPlayer(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../Sounds/loading.wav")));

            SetAutoStart(true, "BatteryAlarm", Application.ExecutablePath);

            // Mise à jour initiale
            UpdateBatteryInfo();
            TimerUpdate();
        }
        
        /// <summary>
        /// Gère l'événement autour du timer avec la remise à zéro pour la remise en marche du son
        /// </summary>
        private void TimerUpdate()
        {
            _timer = new System.Windows.Forms.Timer
            {
                Interval = 500
            };

            _timer.Tick += (_, _) =>
            {
                UpdateBatteryInfo();
                _timer.Stop();
                _timer.Start();
            };
            _timer.Start();
        }

        /// <summary>
        /// Permet de lancer l'application au démarrage de windows
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="appName"></param>
        /// <param name="exePath"></param>
        private static void SetAutoStart(bool enable, string appName, string exePath)
        {
           using (RegistryKey? rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                if (rk != null)
                {
                    if (enable)
                        rk.SetValue(appName, exePath);
                    else
                        rk.DeleteValue(appName, false);
                }
                else
                {
                    Console.Error.WriteLine("Impossible d’ouvrir la clé de registre.");
                }
            }

        }

        /// <summary>
        /// Gère les labels qui gèrent l'affichage
        /// </summary>
        private void UpdateBatteryInfo()
        {
            PowerStatus status = SystemInformation.PowerStatus;
            float batteryLevel = status.BatteryLifePercent * 100;
            string powerLine = status.PowerLineStatus.ToString();

            _statusLabel.Text = powerLine == "Online"
                ? "En charge ⚡"
                : "Sur batterie 🔋";

            if (powerLine != "Online")
            {
                _percentLabel.Text = GetBatteryStatusSound(batteryLevel);
            }
            else
            {
                _percentLabel.Text = $"Battery at {batteryLevel:0}%";
            }
        }
    }
}
