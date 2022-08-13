using UnityEngine;

namespace SLINTIC.BepInEx.Utils
{
    public class AudioClipUtility
    {
        ///<summary>
        /// Generates a Unity AudioClip from an external file or byte array
        ///</summary>
        public static AudioClip GenerateAudioClip(string filePath, SampleRate sampleRate, int channels)
        {
            float[] data = new float[File.ReadAllBytes(filePath).Length / 4]; // get byte array from file and interpret array as float
            AudioClip clip = AudioClip.Create("ClipName", data.Length, channels, (int)sampleRate, false); // having to cast reminds me of how much I miss type coercion
            clip.SetData(data, 0);
            return clip;
        }
        //Overload 1 : Generate using pre-existing byte[]
        public static AudioClip GenerateAudioClip(byte[] fileData, SampleRate sampleRate, int channels)
        {
            float[] data = new float[fileData.Length / 4]; // interpret byte[] as float
            AudioClip clip = AudioClip.Create("ClipName", data.Length, channels, (int)sampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }

        ///<summary>
        /// Enum list of standard audio sample rates
        ///</summary>
        public enum SampleRate
        {  // kinda weirdly laid out like this cause enum limtations
           kHz8000 = 8000,
           kHz11025 = 11025,
           kHz16000 = 16000,
           kHz22050 = 22050,
           kHz44100 = 44100,
           kHz48000 = 48000,
           kHz88200 = 88200,
           kHz96000 = 96000,
           kHz176400 = 176400,
           kHz192000 = 192000,
           kHz352800 = 352800,
           kHz384000 = 384000
        }
    }
}