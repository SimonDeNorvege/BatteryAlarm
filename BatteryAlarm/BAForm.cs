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
