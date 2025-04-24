using System.Media;

namespace BatteryAlarm
{
    public partial class BatteryAlarm
    {
        private const string _soundsPath = "../../../"; 
        private List<SoundPlayer> _sounds = null!;

        /// <summary>
        /// Initialise la liste sounds qui contient les sons
        /// </summary>
        public void InitializeSounds()
        {

            this._sounds = new List<SoundPlayer>
            {
                new SoundPlayer(_soundsPath + "Sounds/fifty.wav"),
                new SoundPlayer(_soundsPath + "Sounds/fourty.wav"),
                new SoundPlayer(_soundsPath + "Sounds/thirty.wav"),
                new SoundPlayer(_soundsPath + "Sounds/twenty.wav"),
                new SoundPlayer(_soundsPath + "Sounds/ten.wav"),
                new SoundPlayer(_soundsPath + "Sounds/five.wav")
            };

            Console.WriteLine(_sounds.Count);
        }

        /// <summary>
        /// Permet de récupérer le son pour le niveau de batterie correspondant
        /// </summary>
        /// <param name="batteryLevel"></param>
        /// <returns>Une string à afficher</returns>
        private string GetBatteryStatusSound(float batteryLevel)
        {
            if ((batteryLevel - BatteryFifty < 10) && (BatteryFifty - batteryLevel < 10))
                PlaySound(_sounds[(int)SoundLevel.Fifty]);
            else if ((batteryLevel - BatteryForty < 10) && (BatteryForty - batteryLevel < 10))
                PlaySound(_sounds[(int)SoundLevel.Fourty]);
            else if ((batteryLevel - BatteryThirty < 10) && (BatteryThirty - batteryLevel < 10))
                PlaySound(_sounds[(int)SoundLevel.Thirty]);
            else if ((batteryLevel - BatteryTwenty < 10) && (BatteryTwenty - batteryLevel < 10))
                PlaySound(_sounds[(int)SoundLevel.Twenty]);
            else if ((batteryLevel - BatteryTen < 10) && (BatteryTen - batteryLevel < 10))
                PlaySound(_sounds[(int)SoundLevel.Ten]);
            else if ((batteryLevel - BatteryFive < 10) && (BatteryFive - batteryLevel < 10))
                PlaySound(_sounds[(int)SoundLevel.Five]);
            return $"Battery : {batteryLevel}%";
        }

        /// <summary>
        /// Lit un son
        /// </summary>
        /// <param name="sound">Son à lire</param>
        public void PlaySound(SoundPlayer sound)
        {
            try
            {
                sound.Load(); // Peut lever une exception si le fichier est manquant
                sound.Play();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erreur lors du chargement de {sound.SoundLocation} : {ex.Message} ");
            }
        }
        
    }
}