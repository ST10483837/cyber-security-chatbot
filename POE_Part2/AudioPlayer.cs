using System;
using System.Media;
using System.IO;
using System.Windows;

namespace POE_Part2
{
    public class AudioPlayer
    {
        public void PlayGreeting()
        {
            try
            {
                // Direct path to your Downloads folder
                string audioPath = @"C:\Users\Student\Downloads\greeting.wav";

                if (File.Exists(audioPath))
                {
                    using (SoundPlayer player = new SoundPlayer(audioPath))
                    {
                        player.PlaySync();
                    }
                }
            }
            catch (Exception)
            {
                // Silently fail if audio doesn't work
            }
        }
    }
}