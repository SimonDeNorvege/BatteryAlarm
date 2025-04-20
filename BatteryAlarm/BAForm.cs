using System;
using System.Windows.Forms;
using System.Media;
using Microsoft.Win32;
using System.Collections.Generic;

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
        private const int BATTERY_FULL = 90;

        private NotifyIcon notifyIcon = null!;
        private ContextMenuStrip contextMenu = null!;
        private Label percentLabel = null!;
        private Label statusLabel = null!;

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
            try
            {
                PlaySound(new SoundPlayer("sounds/loading.wav"));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erreur son démarrage : {ex.Message}");
            }

            SetAutoStart(true, "BatteryAlarm", Application.ExecutablePath);

            // Mise à jour initiale
            UpdateBatteryInfo();
            TimerUpdate();
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

            timer.Tick += (s, e) =>
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

            statusLabel.Text = powerLine == "Online"
                ? "En charge ⚡"
                : "Sur batterie 🔋";

            if (powerLine != "Online")
            {
                percentLabel.Text = GetBatteryStatusSound(batteryLevel);
            }
            else
            {
                percentLabel.Text = $"Battery at {batteryLevel:0}%";
            }
        }
    }
}
