using System.Collections.Generic;
using System.Media;

namespace BatteryAlarm
{
    public partial class BatteryAlarm
    {
        private List<SoundPlayer> sounds = null!;

        /// <summary>
        /// Initialise la liste sounds qui contient les sons
        /// </summary>
        public void InitializeSounds()
        {
            this.sounds = new List<SoundPlayer>
            {
                new SoundPlayer("sounds/fifty.wav"),
                new SoundPlayer("sounds/fourty.wav"),
                new SoundPlayer("sounds/thirty.wav"),
                new SoundPlayer("sounds/twenty.wav"),
                new SoundPlayer("sounds/ten.wav"),
                new SoundPlayer("sounds/five.wav")
            };
        }

        /// <summary>
        /// Permet de récupérer le son pour le niveau de batterie correspondant
        /// </summary>
        /// <param name="batteryLevel"></param>
        /// <returns>Une string à afficher</returns>
        private string GetBatteryStatusSound(float batteryLevel)
        {
            if (batteryLevel == BATTERY_FIFTY)
                PlaySound(sounds[(int)SoundLevel.Fifty]);
            else if (batteryLevel == BATTERY_FOURTY)
                PlaySound(sounds[(int)SoundLevel.Fourty]);
            else if (batteryLevel == BATTERY_THIRTY)
                PlaySound(sounds[(int)SoundLevel.Thirty]);
            else if (batteryLevel == BATTERY_TWENTY)
                PlaySound(sounds[(int)SoundLevel.Twenty]);
            else if (batteryLevel == BATTERY_TEN)
                PlaySound(sounds[(int)SoundLevel.Ten]);
            else if (batteryLevel == BATTERY_FIVE)
                PlaySound(sounds[(int)SoundLevel.Five]);
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
                Console.Error.WriteLine($"Erreur lors du chargement du son : {ex.Message}");
            }
        }
        
    }
}