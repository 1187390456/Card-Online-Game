using UnityEngine;
using System.Collections;

namespace Crosstales.RTVoice.Provider
{
    /// <summary>Base class for voice providers.</summary>
    public abstract class BaseVoiceProvider
    {

        #region Variables

        //protected static System.Collections.Generic.List<Model.Voice> cachedVoices;

#if (!UNITY_WSA && !UNITY_WSA_10_0) || UNITY_EDITOR
        protected System.Collections.Generic.Dictionary<System.Guid, System.Diagnostics.Process> processes = new System.Collections.Generic.Dictionary<System.Guid, System.Diagnostics.Process>();
#endif

        protected bool silence = false;

        protected static char[] splitCharWords = new char[] { ' ' };

        #endregion


        #region Properties

        /// <summary>Returns the extension of the generated audio files.</summary>
        /// <returns>Extension of the generated audio files.</returns>
        public abstract string AudioFileExtension
        {
            get;
        }

        /// <summary>Get all available voices from the current TTS-provider and fills it into a given list.</summary>
        /// <returns>All available voices from the current TTS-provider as list.</returns>
        public abstract System.Collections.Generic.List<Model.Voice> Voices
        {
            get;
        }

        #endregion


        #region Events

        public delegate void SpeakCurrentWord(Model.Event.CurrentWordEventArgs e);
        public delegate void SpeakCurrentPhoneme(Model.Event.CurrentPhonemeEventArgs e);
        public delegate void SpeakCurrentViseme(Model.Event.CurrentVisemeEventArgs e);
        public delegate void SpeakStart(Model.Event.SpeakEventArgs e);
        public delegate void SpeakComplete(Model.Event.SpeakEventArgs e);
        public delegate void SpeakAudioGenerationStart(Model.Event.SpeakEventArgs e);
        public delegate void SpeakAudioGenerationComplete(Model.Event.SpeakEventArgs e);
        public delegate void ErrorInfo(string info);

        /// <summary>An event triggered whenever a new word is spoken (native mode, Windows only).</summary>
        public static event SpeakCurrentWord OnSpeakCurrentWord;

        /// <summary>An event triggered whenever a new phoneme is spoken (native mode, Windows only).</summary>
        public static event SpeakCurrentPhoneme OnSpeakCurrentPhoneme;

        /// <summary>An event triggered whenever a new viseme is spoken  (native mode, Windows only).</summary>
        public static event SpeakCurrentViseme OnSpeakCurrentViseme;

        /// <summary>An event triggered whenever a speak is started.</summary>
        public static event SpeakStart OnSpeakStart;

        /// <summary>An event triggered whenever a native speak is completed.</summary>
        public static event SpeakComplete OnSpeakComplete;

        /// <summary>An event triggered whenever a speak audio generation is started.</summary>
        public static event SpeakAudioGenerationStart OnSpeakAudioGenerationStart;

        /// <summary>An event triggered whenever a speak audio generation is completed.</summary>
        public static event SpeakAudioGenerationComplete OnSpeakAudioGenerationComplete;

        /// <summary>An event triggered whenever an error occurs.</summary>
        public static event ErrorInfo OnErrorInfo;

        #endregion


        #region Public methods

        /// <summary>Silence all active TTS-providers.</summary>
        public virtual void Silence()
        {
            silence = true;

#if (UNITY_STANDALONE || UNITY_EDITOR) && !UNITY_WEBPLAYER

            foreach (System.Collections.Generic.KeyValuePair<System.Guid, System.Diagnostics.Process> kvp in processes)
            {
                if (!kvp.Value.HasExited)
                {
                    kvp.Value.Kill();
                }
            }

            processes.Clear();
#endif

        }

        /// <summary>Silence the current TTS-provider (native mode).</summary>
        /// <param name="uid">UID of the speaker</param>
        public virtual void Silence(System.Guid uid)
        {
#if (UNITY_STANDALONE || UNITY_EDITOR) && !UNITY_WEBPLAYER

            if (processes.ContainsKey(uid) && !processes[uid].HasExited)
            {
                processes[uid].Kill();
            }

            processes.Remove(uid);
#endif

        }

        /// <summary>The current provider speaks a text with a given voice (native mode).</summary>
        /// <param name="wrapper">Wrapper containing the data.</param>
        public abstract IEnumerator SpeakNative(Model.Wrapper wrapper);

        /// <summary>The current provider speaks a text with a given voice.</summary>
        /// <param name="wrapper">Wrapper containing the data.</param>
        public abstract IEnumerator Speak(Model.Wrapper wrapper);

        #endregion


        #region Protected methods

        protected void fileCopy(string inputFile, string outputFile, bool move = false)
        {
            if (!string.IsNullOrEmpty(outputFile))
            {
                try
                {
                    if (!System.IO.File.Exists(inputFile))
                    {
                        Debug.LogError("Input file does not exists: " + inputFile);
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputFile));

                        if (System.IO.File.Exists(outputFile))
                        {
                            //Debug.LogWarning("Overwrite output file: " + outputFile);
                            System.IO.File.Delete(outputFile);
                        }

                        if (move)
                        {
#if (UNITY_STANDALONE || UNITY_EDITOR) && !UNITY_WEBPLAYER

                            System.IO.File.Move(inputFile, outputFile);
#else
                            System.IO.File.Copy(inputFile, outputFile);
                            System.IO.File.Delete(inputFile);
#endif
                        }
                        else
                        {
                            System.IO.File.Copy(inputFile, outputFile);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Could not copy file!" + System.Environment.NewLine + ex);
                }
            }
        }

        protected static void onSpeakCurrentWord(Model.Wrapper wrapper, string[] speechTextArray, int wordIndex)
        {
            if (Util.Constants.DEBUG)
                Debug.Log("Speak current word: " + speechTextArray[wordIndex] + System.Environment.NewLine + wrapper);

            if (OnSpeakCurrentWord != null)
            {
                OnSpeakCurrentWord(new Model.Event.CurrentWordEventArgs(wrapper, speechTextArray, wordIndex));
            }
        }

        protected static void onSpeakCurrentPhoneme(Model.Wrapper wrapper, string phoneme)
        {
            if (Util.Constants.DEBUG)
                Debug.Log("Speak current phoneme: " + phoneme + System.Environment.NewLine + wrapper);

            if (OnSpeakCurrentPhoneme != null)
            {
                OnSpeakCurrentPhoneme(new Model.Event.CurrentPhonemeEventArgs(wrapper, phoneme));
            }
        }

        protected static void onSpeakCurrentViseme(Model.Wrapper wrapper, string viseme)
        {
            if (Util.Constants.DEBUG)
                Debug.Log("Speak current viseme: " + viseme + System.Environment.NewLine + wrapper);

            if (OnSpeakCurrentViseme != null)
            {
                OnSpeakCurrentViseme(new Model.Event.CurrentVisemeEventArgs(wrapper, viseme));
            }
        }

        protected static void onSpeakStart(Model.Wrapper wrapper)
        {
            if (Util.Constants.DEBUG)
                Debug.Log("Speak start: " + wrapper);

            if (OnSpeakStart != null)
            {
                OnSpeakStart(new Model.Event.SpeakEventArgs(wrapper));
            }
        }

        protected static void onSpeakComplete(Model.Wrapper wrapper)
        {
            if (Util.Constants.DEBUG)
                Debug.Log("Speak complete: " + wrapper);

            if (OnSpeakComplete != null)
            {
                OnSpeakComplete(new Model.Event.SpeakEventArgs(wrapper));
            }
        }

        protected static void onSpeakAudioGenerationStart(Model.Wrapper wrapper)
        {
            if (Util.Constants.DEBUG)
                Debug.Log("Speak audio generation start: " + wrapper);

            if (OnSpeakAudioGenerationStart != null)
            {
                OnSpeakAudioGenerationStart(new Model.Event.SpeakEventArgs(wrapper));
            }
        }

        protected static void onSpeakAudioGenerationComplete(Model.Wrapper wrapper)
        {
            if (Util.Constants.DEBUG)
                Debug.Log("Speak audio generation complete: " + wrapper);

            if (OnSpeakAudioGenerationComplete != null)
            {
                OnSpeakAudioGenerationComplete(new Model.Event.SpeakEventArgs(wrapper));
            }
        }

        protected static void onErrorInfo(string info)
        {
            if (Util.Constants.DEBUG)
                Debug.Log("Error info: " + info);

            if (OnErrorInfo != null)
            {
                OnErrorInfo(info);
            }
        }

        #endregion


        #region Editor-only methods

#if UNITY_EDITOR

        /// <summary>The current provider speaks a text with a given voice (native mode & Editor only).</summary>
        /// <param name="wrapper">Wrapper containing the data.</param>
        public abstract void SpeakNativeInEditor(Model.Wrapper wrapper);

        /// <summary>Generates audio with the current provider.</summary>
        /// <param name="wrapper">Wrapper containing the data.</param>
        public abstract void GenerateInEditor(Model.Wrapper wrapper);

#endif

        #endregion
    }
}
// © 2015-2017 crosstales LLC (https://www.crosstales.com)