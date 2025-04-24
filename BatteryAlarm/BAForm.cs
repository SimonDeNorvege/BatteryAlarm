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
            
            debug_fct();

            // Joue un son au démarrage
            PlaySound(new SoundPlayer(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../Sounds/loading.wav")));

            SetAutoStart(true, "BatteryAlarm", Application.ExecutablePath);

            // Mise à jour initiale
            UpdateBatteryInfo();
            TimerUpdate();
        }

        void debug_fct()
        {
            string currentDirectory = Environment.CurrentDirectory;
            
            Console.WriteLine("DEBUG FCT");
            Console.WriteLine(currentDirectory);
            Console.WriteLine(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        /// <summary>
        /// Gère l'événement autour du timer
        /// </summary>
        private void TimerUpdate()
        {
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer
            {
                Interval = 500
            };

            timer.Tick += (_, _) =>
            {
                UpdateBatteryInfo();
                timer.Stop();
                timer.Start();
            };
            timer.Start();
        }

        /// <summary>
        /// Permet de lancer l'application au démarrage de windows
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="appName"></param>
        /// <param name="exePath"></param>
        public static void SetAutoStart(bool enable, string appName, string exePath)
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
