using System;
using System.Windows.Forms;
using System.Media;


namespace BatteryAlarm
{
    public partial class BatteryAlarm : Form
    {
        private const int BATTERY_LOW = 30;
        private const int BATTERY_MID = 50;
        private const int BATTERY_FULL = 90;

        private Label percentLabel = null!;
        private Label statusLabel = null!;

        public BatteryAlarm()
        {
            InitializeComponent();
            ConfigureForm();
            InitializeLabels();

            //Première Instanciation de UpdateBattery 
            UpdateBatteryInfo();
            TimerUpdate();
        }

        /*
        * Lance un timer qui regarde l'état de la batterie toutes les secondes
        */
        private void TimerUpdate()
        {
            //Précision du timer car il y en a deux
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 500;
            timer.Tick += (s, e) =>
            {
                UpdateBatteryInfo();
                timer.Stop();
                timer.Start();
            };
            timer.Start();

        }

        private void ConfigureForm()
        {
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
            if (batteryLevel <= BATTERY_LOW)
                return $"Low battery ({batteryLevel:0}%)";
            else if (batteryLevel <= BATTERY_MID)
                return $"Half-full battery ({batteryLevel:0}%)";
            else if (batteryLevel <= BATTERY_FULL)
                return $"Full battery ({batteryLevel:0}%)";
            else
                return $"Battery over {BATTERY_FULL}% ({batteryLevel:0}%)";
        }
    }
}
