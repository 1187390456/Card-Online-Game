using UnityEngine;
using System.Collections;

namespace Crosstales.RTVoice.Provider
{

    /// <summary>iOS voice provider.</summary>
    public class VoiceProviderIOS : BaseVoiceProvider
    {
        #region Variables

        private static System.Collections.Generic.List<Model.Voice> cachedVoices;

        private const string extension = "none";

        private static System.Collections.Generic.List<Model.Voice> defaultVoices;

#if (UNITY_IOS || UNITY_EDITOR) && !UNITY_WEBPLAYER

        private static string[] speechTextArray;

        private static string State;

        private static int wordIndex = 0;

        //private static bool getVoicesCalled = false;
        private static bool isWorking = false;

        private static Model.Wrapper wrapperNative;
#endif

        #endregion


        #region Constructor

        public VoiceProviderIOS()
        {
            defaultVoices = new System.Collections.Generic.List<Model.Voice>();

            defaultVoices.Add(new Model.Voice("Maged", "ar-SA", "ar-SA"));
            defaultVoices.Add(new Model.Voice("Zuzana", "cs-CZ", "cs-CZ"));
            defaultVoices.Add(new Model.Voice("Sara ", "da-DK", "da-DK"));
            defaultVoices.Add(new Model.Voice("Anna ", "de-DE", "de-DE"));
            defaultVoices.Add(new Model.Voice("Melina", "el-GR", "el-GR"));
            defaultVoices.Add(new Model.Voice("Karen", "en-AU", "en-AU"));
            defaultVoices.Add(new Model.Voice("Daniel", "en-GB", "en-GB"));
            defaultVoices.Add(new Model.Voice("Moira", "en-IE", "en-IE"));
            defaultVoices.Add(new Model.Voice("Samantha", "en-US", "en-US"));
            defaultVoices.Add(new Model.Voice("Tessa", "en-ZA", "en-ZA"));
            defaultVoices.Add(new Model.Voice("Monica", "es-ES", "es-ES"));
            defaultVoices.Add(new Model.Voice("Paulina", "es-MX", "es-MX"));
            defaultVoices.Add(new Model.Voice("Satu ", "fi-FI", "fi-FI"));
            defaultVoices.Add(new Model.Voice("Amelie", "fr-CA", "fr-CA"));
            defaultVoices.Add(new Model.Voice("Thomas", "fr-FR", "fr-FR"));
            defaultVoices.Add(new Model.Voice("Lekha", "hi-IN", "hi-IN"));
            defaultVoices.Add(new Model.Voice("Mariska", "hu-HU", "hu-HU"));
            defaultVoices.Add(new Model.Voice("Damayanti", "id-ID", "id-ID"));
            defaultVoices.Add(new Model.Voice("Alice", "it-IT", "it-IT"));
            defaultVoices.Add(new Model.Voice("Kyoko", "ja-JP", "ja-JP"));
            defaultVoices.Add(new Model.Voice("Yuna ", "ko-KR", "ko-KR"));
            defaultVoices.Add(new Model.Voice("Ellen", "nl-BE", "nl-BE"));
            defaultVoices.Add(new Model.Voice("Xander", "nl-NL", "nl-NL"));
            defaultVoices.Add(new Model.Voice("Nora", "no-NO", "no-NO"));
            defaultVoices.Add(new Model.Voice("Zosia", "pl-PL", "pl-PL"));
            defaultVoices.Add(new Model.Voice("Luciana", "pt-BR", "pt-BR"));
            defaultVoices.Add(new Model.Voice("Joana", "pt-PT", "pt-PT"));
            defaultVoices.Add(new Model.Voice("Ioana", "ro-RO", "ro-RO"));
            defaultVoices.Add(new Model.Voice("Milena", "ru-RU", "ru-RU"));
            defaultVoices.Add(new Model.Voice("Laura", "sk-SK", "sk-SK"));
            defaultVoices.Add(new Model.Voice("Alva", "sv-SE", "sv-SE"));
            defaultVoices.Add(new Model.Voice("Kanya", "th-TH", "th-TH"));
            defaultVoices.Add(new Model.Voice("Yelda", "tr-TR", "tr-TR"));
            defaultVoices.Add(new Model.Voice("Ting-Ting", "zh-CN", "zh-CN"));
            defaultVoices.Add(new Model.Voice("Sin-Ji", "zh-HK", "zh-HK"));
            defaultVoices.Add(new Model.Voice("Mei-Jia", "zh-TW", "zh-TW"));
        }

        #endregion


        #region Bridge declaration and methods

#if (UNITY_IOS || UNITY_EDITOR) && !UNITY_WEBPLAYER

        /// <summary>Silence the current TTS-provider (native mode).</summary>
        [System.Runtime.InteropServices.DllImport("__Internal")]
        extern static public void Stop();

        /// <summary>Silence the current TTS-provider (native mode).</summary>
        [System.Runtime.InteropServices.DllImport("__Internal")]
        extern static public void GetVoices();

        /// <summary>Bridge to the native tts system</summary>
        /// <param name="gameObject">Receiving gameobject for the messages from iOS</param>
        /// <param name="text">Text to speak.</param>
        /// <param name="rate">Speech rate of the speaker in percent (default: 0.5, optional).</param>
        /// <param name="pitch">Pitch of the speech in percent (default: 1, optional).</param>
        /// <param name="volume">Volume of the speaker in percent (default: 1, optional).</param>
        /// <param name="culture">Culture of the voice to speak (optional).</param>
        [System.Runtime.InteropServices.DllImport("__Internal")]
        extern static public void Speak(string gameObject, string text, float rate = 0.5f, float pitch = 1f, float volume = 1f, string culture = "");

#endif

        /// <summary>Receives all voices</summary>
        /// <param name="voicesText">All voices as text string.</param>
        public static void SetVoices(string voicesText)
        {
#if (UNITY_IOS || UNITY_EDITOR) && !UNITY_WEBPLAYER
            string[] v = voicesText.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

            if (v.Length % 2 == 0)
            {
                cachedVoices = new System.Collections.Generic.List<Model.Voice>();

                string name;
                string culture;
                Model.Voice newVoice;

                for (int ii = 0; ii < v.Length; ii += 2)
                {
                    name = v[ii];
                    culture = v[ii + 1];
                    newVoice = new Model.Voice(name, "iOS voice: " + name + " " + culture, culture);

                    cachedVoices.Add(newVoice);
                }

                if (Util.Constants.DEBUG)
                    Debug.Log("Voices read: " + cachedVoices.CTDump());
            }
            else
            {
                Debug.LogWarning("Voice-string contains an uneven number of elements!");
            }
#endif
        }

        /// <summary>Receives the state of the speaker.</summary>
        /// <param name="state">The state of the speaker.</param>
        public static void SetState(string state)
        {
#if (UNITY_IOS || UNITY_EDITOR) && !UNITY_WEBPLAYER
            if (state.Equals("Start"))
            {
                // do nothing
            }
            else if (state.Equals("Finsish"))
            {
                isWorking = false;
            }
            else
            { //cancel
                isWorking = false;
            }
#endif
        }

        /// <summary>Called everytime a new word is spoken.</summary>
        public static void WordSpoken()
        {
#if (UNITY_IOS || UNITY_EDITOR) && !UNITY_WEBPLAYER
            if (wrapperNative != null)
            {
                onSpeakCurrentWord(wrapperNative, speechTextArray, wordIndex);
                wordIndex++;
            }
#endif
        }

        #endregion


        #region Implemented methods

        public override string AudioFileExtension
        {
            get
            {
                return extension;
            }
        }


        public override System.Collections.Generic.List<Model.Voice> Voices
        {
            get
            {

                if (cachedVoices == null)
                {
                    cachedVoices = defaultVoices;

#if (UNITY_IOS || UNITY_EDITOR) && !UNITY_WEBPLAYER
                    GetVoices();
#endif
                }

                return cachedVoices;
            }
        }

        public override void Silence()
        {
            silence = true;

#if (UNITY_IOS || UNITY_EDITOR) && !UNITY_WEBPLAYER
            Stop();
#endif
        }

        public override IEnumerator SpeakNative(Model.Wrapper wrapper)
        {
            yield return speak(wrapper, true);
        }


        public override IEnumerator Speak(Model.Wrapper wrapper)
        {
            yield return speak(wrapper, false);
        }

        #endregion


        #region Private methods

        private IEnumerator speak(Model.Wrapper wrapper, bool isNative)
        {

#if (UNITY_IOS || UNITY_EDITOR) && !UNITY_WEBPLAYER
            if (wrapper == null)
            {
                Debug.LogWarning("'wrapper' is null!");
            }
            else
            {
                if (string.IsNullOrEmpty(wrapper.Text))
                {
                    Debug.LogWarning("'Text' is null or empty!");
                    yield return null;
                }
                else
                {
                    yield return null;
                    string voiceName = string.Empty;
                    if (wrapper.Voice == null || string.IsNullOrEmpty(wrapper.Voice.Culture))
                    {
                        Debug.LogWarning("'Voice' or 'Voice.Culture' is null! Using the OS 'default' voice.");
                    }
                    else
                    {
                        voiceName = wrapper.Voice.Culture;
                    }

                    silence = false;

                    if (!isNative)
                    {
                        onSpeakAudioGenerationStart(wrapper); //just a fake event if some code needs the feedback...
                    }

                    onSpeakStart(wrapper);
                    isWorking = true;

                    speechTextArray = Util.Helper.CleanText(wrapper.Text, false).Split(splitCharWords, System.StringSplitOptions.RemoveEmptyEntries);
                    wordIndex = 0;
                    wrapperNative = wrapper;

                    Speak(Util.Constants.RTVOICE_SCENE_OBJECT_NAME, wrapper.Text, calculateRate(wrapper.Rate), wrapper.Pitch, wrapper.Volume, voiceName);

                    do
                    {
                        yield return null;
                    } while (isWorking && !silence);

                    if (Util.Constants.DEBUG)
                        Debug.Log("Text spoken: " + wrapper.Text);

                    wrapperNative = null;
                    onSpeakComplete(wrapper);

                    if (!isNative)
                    {
                        onSpeakAudioGenerationComplete(wrapper); //just a fake event if some code needs the feedback...
                    }
                }
            }
#else
            yield return null;
#endif
        }

        private float calculateRate(float rate)
        {
            if (rate > 1)
            {
                rate = (rate + 1) * 0.25f;
            }

            return rate;
        }


        #endregion


        #region Editor-only methods

#if UNITY_EDITOR

        public override void GenerateInEditor(Model.Wrapper wrapper)
        {
            Debug.LogError("GenerateInEditor is not supported for Unity iOS!");
        }

        public override void SpeakNativeInEditor(Model.Wrapper wrapper)
        {
            Debug.LogError("SpeakNativeInEditor is not supported for Unity iOS!");
        }
#endif

        #endregion
    }
}
// © 2016-2017 crosstales LLC (https://www.crosstales.com)