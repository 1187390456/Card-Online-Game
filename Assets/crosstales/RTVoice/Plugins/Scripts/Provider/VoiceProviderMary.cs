using UnityEngine;
using System.Collections;

namespace Crosstales.RTVoice.Provider
{

    /// <summary>MaryTTS voice provider.</summary>
    public class VoiceProviderMary : BaseVoiceProvider
    {

        #region Variables

        private static System.Collections.Generic.List<Model.Voice> cachedVoices;

        private const string extension = ".wav";

        private string uri;

        //private static bool policyFetched = false;

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor for VoiceProviderMary. Needed to pass IP and Port of the MaryTTS server to the Provider.
        /// </summary>
        /// <param name="url">IP-Address of the MaryTTS-server</param>
        /// <param name="port">Port to connect to on the MaryTTS-server</param>
        public VoiceProviderMary(string url, int port)
        {
            uri = url + ":" + port;
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
                    getVoices();
                }

                return cachedVoices;
            }
        }

        public override IEnumerator SpeakNative(Model.Wrapper wrapper)
        {
            yield return speak(wrapper, true);
        }

        public override IEnumerator Speak(Model.Wrapper wrapper)
        {
            yield return speak(wrapper, false);
        }

        public override void Silence()
        {
            silence = true;
        }

        #endregion


        #region Private methods

        private IEnumerator speak(Model.Wrapper wrapper, bool isNative)
        {

            if (wrapper == null)
            {
                Debug.LogWarning("'wrapper' is null!");
            }
            else
            {
                if (string.IsNullOrEmpty(wrapper.Text))
                {
                    Debug.LogWarning("'Text' is null or empty: " + wrapper);
                    yield return null;
                }
                else
                {
                    if (wrapper.Source == null)
                    {
                        Debug.LogWarning("'Source' is null: " + wrapper);
                        yield return null;
                    }

                    string voiceCulture = string.Empty;
                    string voiceName = string.Empty;

                    if (wrapper.Voice == null || string.IsNullOrEmpty(wrapper.Voice.Name) || string.IsNullOrEmpty(wrapper.Voice.Culture))
                    {
                        Debug.LogWarning("'Voice', 'Voice.Name' or 'Voice.Culture' is null! Using the 'default' voice.");
                    }
                    else
                    {
                        voiceCulture = wrapper.Voice.Culture;
                        voiceName = wrapper.Voice.Name;
                    }

                    silence = false;

                    if (!isNative)
                    {
                        onSpeakAudioGenerationStart(wrapper);
                    }

                    string plusText = Util.Helper.CleanText(wrapper.Text, false).Replace(' ', '+');

                    string request = null;

                    if (wrapper.Volume != 1f)
                    {
                        request = uri + "/process?INPUT_TEXT=" + plusText + "&INPUT_TYPE=TEXT&OUTPUT_TYPE=AUDIO&AUDIO=WAVE_FILE&LOCALE=" + voiceCulture + "&VOICE=" + voiceName + "&effect_Volume_selected=on&effect_Volume_parameters=amount:" + wrapper.Volume;
                    }
                    else
                    {
                        request = uri + "/process?INPUT_TEXT=" + plusText + "&INPUT_TYPE=TEXT&OUTPUT_TYPE=AUDIO&AUDIO=WAVE_FILE&LOCALE=" + voiceCulture + "&VOICE=" + voiceName;
                    }

                    using (WWW www = new WWW(request))
                    {

                        if (string.IsNullOrEmpty(www.error))
                        {
                            AudioClip ac = www.GetAudioClip(false, false, AudioType.WAV); //TODO streaming to true?

                            do
                            {
                                yield return www;
                            } while (ac.loadState == AudioDataLoadState.Loading);

                            if (wrapper.Source != null && ac.loadState == AudioDataLoadState.Loaded)
                            {
                                wrapper.Source.clip = ac;

                                if (Util.Constants.DEBUG)
                                    Debug.Log("Text generated: " + wrapper.Text);

                                if (!string.IsNullOrEmpty(wrapper.OutputFile))
                                {
                                    wrapper.OutputFile += AudioFileExtension;
                                    Util.AudioExporter.SaveAsWav(wrapper.OutputFile, ac);
                                }

                                if ((isNative || wrapper.SpeakImmediately) && wrapper.Source != null)
                                {
                                    wrapper.Source.Play();
                                    onSpeakStart(wrapper);

                                    do
                                    {
                                        yield return null;
                                    } while (!silence &&
                                             (wrapper.Source != null &&
                                             ((!wrapper.Source.loop && wrapper.Source.timeSamples > 0 && (wrapper.Source.clip != null && wrapper.Source.timeSamples < wrapper.Source.clip.samples - 4096)) ||
                                             wrapper.Source.isPlaying)));

                                    if (Util.Constants.DEBUG)
                                        Debug.Log("Text spoken: " + wrapper.Text);

                                    onSpeakComplete(wrapper);
                                }
                            }

                            if (!isNative)
                            {
                                onSpeakAudioGenerationComplete(wrapper);
                            }
                        }
                        else
                        {
                            string errorMessage = "Could generate the speech: " + www.error;
                            Debug.LogError(errorMessage);
                            onErrorInfo(errorMessage);
                        }
                    }
                }
            }
        }

        private void getVoices()
        {
            cachedVoices = new System.Collections.Generic.List<Model.Voice>();

            System.Collections.Generic.List<string[]> serverVoicesResponse = new System.Collections.Generic.List<string[]>();

            using (WWW www = new WWW(uri + "/voices"))
            {

                float time = Time.realtimeSinceStartup;

                do
                {
                    // waiting...
                    //TODO find a better solution for this - e.g. use co-routine instead...

#if !UNITY_WSA && !UNITY_WSA_10_0 && !UNITY_WEBGL
                    System.Threading.Thread.Sleep(50);
#endif
                } while (Time.realtimeSinceStartup - time < Util.Constants.TTS_KILL_TIME / 1000 && !www.isDone);


                if (string.IsNullOrEmpty(www.error) && Time.realtimeSinceStartup - time < Util.Constants.TTS_KILL_TIME / 1000)
                {
                    string[] rawVoices = www.text.Split('\n');
                    foreach (string rawVoice in rawVoices)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(rawVoice))
                            {
                                string[] newVoice =
                                    {
                                    rawVoice.Split(' ')[0],
                                    rawVoice.Split(' ')[1],
                                    rawVoice.Split(' ')[2]
                                };
                                serverVoicesResponse.Add(newVoice);
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogWarning("Problem preparing voice: " + rawVoice + " - " + ex);
                        }
                    }

                    foreach (string[] voice in serverVoicesResponse)
                    {
                        Model.Voice newVoice = new Model.Voice(voice[0], "MaryTTS voice: " + voice[0], voice[2], "unknown", voice[1]);
                        cachedVoices.Add(newVoice);
                    }

                    if (Util.Constants.DEBUG)
                        Debug.Log("Voices read: " + cachedVoices.CTDump());
                }
                else
                {
                    string errorMessage = null;

                    if (!string.IsNullOrEmpty(www.error))
                    {
                        errorMessage = "Could not get the voices: " + www.error;
                    }
                    else
                    {
                        errorMessage = "Could not get the voices - MaryTTS had a timeout!";
                    }

                    Debug.LogError(errorMessage);
                    onErrorInfo(errorMessage);
                }
            }
        }

        #endregion


        #region Editor-only methods

#if UNITY_EDITOR

        public override void GenerateInEditor(Model.Wrapper wrapper)
        {
            Debug.LogError("GenerateInEditor is not supported for MaryTTS!");
        }

        public override void SpeakNativeInEditor(Model.Wrapper wrapper)
        {
            Debug.LogError("SpeakNativeInEditor is not supported for MaryTTS!");
        }

#endif

        #endregion

    }
}
// © 2016-2017 crosstales LLC (https://www.crosstales.com)