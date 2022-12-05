using UnityEngine;
using System.Collections;

namespace Crosstales.RTVoice.Provider
{

    /// <summary>Android voice provider.</summary>
    public class VoiceProviderAndroid : BaseVoiceProvider
    {

        #region Variables

        private static System.Collections.Generic.List<Model.Voice> cachedVoices;

        private const string extension = ".wav";

#if (UNITY_ANDROID || UNITY_EDITOR) && !UNITY_WEBPLAYER
        private static bool isInitialized = false;
        private static AndroidJavaObject TtsHandler;

        private const float waitForSeconds = 0.07f;
#endif

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

#if (UNITY_ANDROID || UNITY_EDITOR) && !UNITY_WEBPLAYER

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
                    if (!isInitialized)
                    {
                        initializeTTS();

                        do
                        {
                            // waiting...
                            yield return new WaitForSeconds(waitForSeconds);

                        } while (!(isInitialized = TtsHandler.CallStatic<bool>("isInitalized")));
                    }

                    string voiceName = string.Empty;
                    if (wrapper.Voice == null || string.IsNullOrEmpty(wrapper.Voice.Name))
                    {
                        Debug.LogWarning("'Voice' or 'Voice.Name' is null! Using the OS 'default' voice.");
                    }
                    else
                    {
                        voiceName = wrapper.Voice.Name;
                    }
                    silence = false;
                    onSpeakStart(wrapper);

                    TtsHandler.CallStatic("SpeakNative", new object[] {
                        wrapper.Text,
                        wrapper.Rate,
                        wrapper.Pitch,
                        wrapper.Volume,
                        voiceName
                    });

                    do
                    {
                        yield return new WaitForSeconds(waitForSeconds);
                    } while (!silence && TtsHandler.CallStatic<bool>("isWorking"));

                    if (Util.Constants.DEBUG)
                        Debug.Log("Text spoken: " + wrapper.Text);

                    onSpeakComplete(wrapper);
                }
            }

#else

            yield return null;

#endif

        }

        public override IEnumerator Speak(Model.Wrapper wrapper)
        {

#if (UNITY_ANDROID || UNITY_EDITOR) && !UNITY_WEBPLAYER

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
                    else
                    {
                        if (!isInitialized)
                        {
                            initializeTTS();

                            do
                            {
                                // waiting...
                                yield return new WaitForSeconds(waitForSeconds);

                            } while (!(isInitialized = TtsHandler.CallStatic<bool>("isInitalized")));
                        }

                        string voiceName = string.Empty;

                        if (wrapper.Voice == null || string.IsNullOrEmpty(wrapper.Voice.Name))
                        {
                            Debug.LogWarning("'Voice' or 'Voice.Name' is null! Using the OS's 'default' voice.");
                        }
                        else
                        {
                            voiceName = wrapper.Voice.Name;
                        }

                        string outputFile = Application.persistentDataPath + "/" + wrapper.Uid + extension;

                        TtsHandler.CallStatic<string>("Speak", new object[] {
                            wrapper.Text,
                            wrapper.Rate,
                            wrapper.Pitch,
                            voiceName,
                            outputFile
                        });

                        silence = false;
                        onSpeakAudioGenerationStart(wrapper);

                        do
                        {
                            yield return new WaitForSeconds(waitForSeconds);
                        } while (!silence && TtsHandler.CallStatic<bool>("isWorking"));

                        using (WWW www = new WWW("file://" + outputFile))
                        {

                            if (string.IsNullOrEmpty(www.error))
                            {
                                AudioClip ac = www.GetAudioClip(false, false, AudioType.WAV);
                                //AudioClip ac = www.GetAudioClipCompressed(false, AudioType.WAV);

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
                                        fileCopy(outputFile, wrapper.OutputFile, Util.Constants.AUDIOFILE_AUTOMATIC_DELETE);
                                    }

                                    if (Util.Constants.AUDIOFILE_AUTOMATIC_DELETE)
                                    {
                                        if (System.IO.File.Exists(outputFile))
                                        {
                                            try
                                            {
                                                System.IO.File.Delete(outputFile);
                                            }
                                            catch (System.Exception ex)
                                            {
                                                string errorMessage = "Could not delete file '" + outputFile + "'!" + System.Environment.NewLine + ex;
                                                Debug.LogError(errorMessage);
                                                onErrorInfo(errorMessage);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(wrapper.OutputFile))
                                        {
                                            wrapper.OutputFile = outputFile;
                                        }
                                    }

                                    if (wrapper.SpeakImmediately && wrapper.Source != null)
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
                            }
                            else
                            {
                                string errorMessage = "Could not read the file: " + www.error;
                                Debug.LogError(errorMessage);
                                onErrorInfo(errorMessage);
                            }
                        }

                        onSpeakAudioGenerationComplete(wrapper);
                    }
                }
            }

#else

            yield return null;

#endif

        }

        public override void Silence()
        {
            silence = true;

#if (UNITY_ANDROID || UNITY_EDITOR) && !UNITY_WEBPLAYER
            TtsHandler.CallStatic("StopNative");
#endif
        }


        #endregion


        #region Public methods


        public void ShutdownTTS()
        {
#if (UNITY_ANDROID || UNITY_EDITOR) && !UNITY_WEBPLAYER
            TtsHandler.CallStatic("Shutdown");
#endif
        }

        #endregion


        #region Private methods

        private void getVoices()
        {
            cachedVoices = new System.Collections.Generic.List<Model.Voice>();

#if (UNITY_ANDROID || UNITY_EDITOR) && !UNITY_WEBPLAYER

            if (!isInitialized)
            {
                initializeTTS();

                float time = Time.realtimeSinceStartup;

                do
                {
                    // waiting...
                    //TODO find a better solution for this...
                    System.Threading.Thread.Sleep(50);
                } while (Time.realtimeSinceStartup - time < Util.Constants.TTS_KILL_TIME / 1000 && !(isInitialized = TtsHandler.CallStatic<bool>("isInitalized")));

            }

            try
            {
                string[] myStringVoices = TtsHandler.CallStatic<string[]>("GetVoices");

                foreach (string voice in myStringVoices)
                {
                    string[] currentVoiceData = voice.Split(';');
                    Model.Voice newVoice = new Model.Voice(currentVoiceData[0], "Android voice: " + voice, currentVoiceData[1]);
                    cachedVoices.Add(newVoice);
                }

                if (Util.Constants.DEBUG)
                    Debug.Log("Voices read: " + cachedVoices.CTDump());
            }
            catch (System.Exception ex)
            {
                string errorMessage = "Could not get any voices!" + System.Environment.NewLine + ex;
                Debug.LogError(errorMessage);
                onErrorInfo(errorMessage);
            }

#endif
        }

#if (UNITY_ANDROID || UNITY_EDITOR) && !UNITY_WEBPLAYER

        private void initializeTTS()
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            TtsHandler = new AndroidJavaObject("com.crosstales.RTVoice.RTVoiceAndroidBridge", new object[] { jo });
        }

#endif

        #endregion


        #region Editor-only methods

#if UNITY_EDITOR

        public override void GenerateInEditor(Model.Wrapper wrapper)
        {
            Debug.LogError("GenerateInEditor is not supported for Unity Android!");
        }

        public override void SpeakNativeInEditor(Model.Wrapper wrapper)
        {
            Debug.LogError("SpeakNativeInEditor is not supported for Unity Android!");
        }

#endif

        #endregion

    }
}
// © 2016-2017 crosstales LLC (https://www.crosstales.com)