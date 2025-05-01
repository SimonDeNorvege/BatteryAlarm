using System.Media;

namespace BatteryAlarm
{
    public partial class BatteryAlarm
    {
        private const string SoundsPath = "../../../Resources/Sounds/"; //Path mauvais
        
        private static readonly List<string> SoundsPaths = new List<string>()
        {
            new string(SoundsPath + "fifty.wav"),
            new string(SoundsPath + "fourty.wav"),
            new string(SoundsPath + "thirty.wav"),
            new string(SoundsPath + "twenty.wav"),
            new string(SoundsPath + "ten.wav"),
            new string(SoundsPath + "five.wav"),
        };
        
        private readonly SoundPlayer _soundPlayer = new SoundPlayer();

        /// <summary>
        /// Permet de récupérer le son pour le niveau de batterie correspondant
        /// </summary>
        /// <param name="batteryLevel"></param>
        /// <returns>Une string à afficher</returns>
        private string GetBatteryStatusSound(float batteryLevel) // mauvais d'abord faire le int index et 
        {
            int remainingBattery = (int)Math.Round(Math.Clamp(0, batteryLevel, 100));
            string toPlay;

            if ((remainingBattery - BatteryFifty < 10) && (BatteryFifty - remainingBattery < 10))
                toPlay = SoundsPaths[(int)SoundLevel.Fifty];
            else if ((remainingBattery - BatteryForty < 10) && (BatteryForty - remainingBattery < 10))
                toPlay = SoundsPaths[(int)SoundLevel.Fourty];
            else if ((remainingBattery - BatteryThirty < 10) && (BatteryThirty - remainingBattery < 10))
                toPlay = SoundsPaths[(int)SoundLevel.Thirty];
            else if ((remainingBattery - BatteryTwenty < 10) && (BatteryTwenty - remainingBattery < 10))
                toPlay = SoundsPaths[(int)SoundLevel.Twenty];
            else if ((remainingBattery - BatteryTen < 10) && (BatteryTen - remainingBattery < 10))
                toPlay = SoundsPaths[(int)SoundLevel.Ten];
            else if ((remainingBattery - BatteryFive < 10) && (BatteryFive - remainingBattery < 10))
                toPlay = SoundsPaths[(int)SoundLevel.Five];
            else
                toPlay = "No Sound";
            
            if (toPlay != "No Sound")
                PlaySound(toPlay);
            
            
            return $"Battery : {batteryLevel}%";
            
            
            
        }

        /// <summary>
        /// Lit un son
        /// </summary>
        /// <param name="sound">Son à lire</param>
        public void PlaySound(string sound)
        {
                _soundPlayer.SoundLocation = sound;
                _soundPlayer.Load(); // Peut lever une exception si le fichier est manquant
                _soundPlayer.Play();
        }
        
    }
}