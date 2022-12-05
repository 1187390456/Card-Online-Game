using UnityEngine;

namespace Crosstales.RTVoice.Model
{
    /// <summary>Wrapper for "Speak"-function calls.</summary>
    public class Wrapper
    {
        #region Variables

        /// <summary>UID of the speech.</summary>
        public System.Guid Uid;

        /// <summary>Text for the speech.</summary>
        public string Text;

        /// <summary>AudioSource for the speech.</summary>
        public AudioSource Source;

        /// <summary>Voice for the speech.</summary>
        public Voice Voice;

        /// <summary>Speak immediatlely after the audio generation. Only works if 'Source' is not null.</summary>
        public bool SpeakImmediately;

        /// <summary>Output file (without extension) for the generated audio.</summary>
        public string OutputFile;

        private float rate;
        private float pitch;
        private float volume;

        #endregion


        #region Properties

        /// <summary>Rate of the speech (values: 0-3).</summary>
        public float Rate
        {
            get
            {
                return rate;
            }

            set
            {
                rate = Mathf.Clamp(value, 0f, 3f);
            }
        }

        /// <summary>Pitch of the speech (values: 0-2).</summary>
        public float Pitch
        {
            get
            {
                return pitch;
            }

            set
            {
                pitch = Mathf.Clamp(value, 0f, 2f);
            }
        }

        /// <summary>Volume of the speech (values: 0-1).</summary>
        public float Volume
        {
            get
            {
                return volume;
            }

            set
            {
                volume = Mathf.Clamp(value, 0f, 1f);
            }
        }

        #endregion


        #region Constructors

        /// <summary>Instantiate the class.</summary>
        /// <param name="text">Text for the speech.</param>
        /// <param name="voice">Voice for the speech.</param>
        /// <param name="rate">Rate of the speech (values: 0-3).</param>
        /// <param name="pitch">Pitch of the speech (values: 0-2).</param>
        /// <param name="volume">Volume of the speech (values: 0-1, Windows only).</param>
        /// <param name="source">AudioSource for the speech.</param>
        /// <param name="speakImmediately">>Speak immediatlely after the audio generation. Only works if 'Source' is not null.</param>
        /// <param name="outputFile">Output file (without extension) for the generated audio.</param>
        public Wrapper(string text, Voice voice = null, float rate = 1f, float pitch = 1f, float volume = 1f, AudioSource source = null, bool speakImmediately = true, string outputFile = "")
        {
            Uid = System.Guid.NewGuid();
            Text = text;
            Source = source;
            Voice = voice;
            SpeakImmediately = speakImmediately;
            Rate = rate;
            Pitch = pitch;
            Volume = volume;
            OutputFile = outputFile;
        }

        /// <summary>Instantiate the class.</summary>
        /// <param name="uid">UID of the speech.</param>
        /// <param name="text">Text for the speech.</param>
        /// <param name="voice">Voice for the speech.</param>
        /// <param name="rate">Rate of the speech (values: 0-3).</param>
        /// <param name="pitch">Pitch of the speech (values: 0-2).</param>
        /// <param name="volume">Volume of the speech (values: 0-1, Windows only).</param>
        /// <param name="source">AudioSource for the speech.</param>
        /// <param name="speakImmediately">>Speak immediatlely after the audio generation. Only works if 'Source' is not null.</param>
        /// <param name="outputFile">Output file (without extension) for the generated audio.</param>
        public Wrapper(System.Guid uid, string text, Voice voice = null, float rate = 1f, float pitch = 1f, float volume = 1f, AudioSource source = null, bool speakImmediately = true, string outputFile = "") : this(text, voice, rate, pitch, volume, source, speakImmediately, outputFile)
        {
            Uid = uid;
        }

        #endregion


        #region Overridden methods

        public override string ToString()
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();

            result.Append(GetType().Name);
            result.Append(Util.Constants.TEXT_TOSTRING_START);

            result.Append("Uid='");
            result.Append(Uid);
            result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Text='");
            result.Append(Text);
            result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Source='");
            result.Append(Source);
            result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Voice='");
            result.Append(Voice);
            result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("SpeakImmediately='");
            result.Append(SpeakImmediately);
            result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Rate='");
            result.Append(Rate);
            result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Pitch='");
            result.Append(Pitch);
            result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Volume='");
            result.Append(Volume);
            result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("OutputFile='");
            result.Append(OutputFile);
            result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER_END);

            result.Append(Util.Constants.TEXT_TOSTRING_END);

            return result.ToString();
        }

        #endregion
    }
}
// © 2015-2017 crosstales LLC (https://www.crosstales.com)