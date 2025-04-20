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

        public BatteryAlarm()
        {
            InitializeComponent();
            ConfigureForm();
            InitializeLabels();
            InitializeSounds();
            InitializeTrayIcon();

            // Joue un son au démarrage (optionnel)
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
