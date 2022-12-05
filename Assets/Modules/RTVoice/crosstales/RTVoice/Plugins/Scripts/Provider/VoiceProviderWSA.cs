using UnityEngine;
using System.Collections;

namespace Crosstales.RTVoice.Provider
{
    public class VoiceProviderWSA : BaseVoiceProvider
    {

        #region Variables

        private static System.Collections.Generic.List<Model.Voice> cachedVoices;

        private const string extension = ".wav";

#if (UNITY_WSA_10_0 || UNITY_WSA || UNITY_EDITOR) && !UNITY_WEBPLAYER

        //private string myPath;
        private bool isInitialized = false;

        private RTVoiceUWPBridge ttsHandler;
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

        private void getVoices()
        {
            cachedVoices = new System.Collections.Generic.List<Model.Voice>();

#if (UNITY_WSA_10_0 || UNITY_WSA || UNITY_EDITOR) && !UNITY_WEBPLAYER
            if (!isInitialized)
            {
                initializeTTS();
            }

            try
            {
                System.Collections.Generic.List<string> myStringVoices = ttsHandler.GetVoices();

                foreach (string voice in myStringVoices)
                {
                    string[] currentVoiceData = voice.Split(';');
                    Model.Voice newVoice = new Model.Voice(currentVoiceData[0], "UWP voice: " + voice, currentVoiceData[1]);
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

        private IEnumerator speak(Model.Wrapper wrapper, bool isNative)
        {

#if (UNITY_WSA_10_0 || UNITY_WSA || UNITY_EDITOR) && !UNITY_WEBPLAYER
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
                            yield return null;
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

                        string outputFile = Application.persistentDataPath;
                        ttsHandler.SynthesizeToFile(wrapper.Text, outputFile.Replace('/', '\\'), wrapper.Uid + extension, voiceName);

                        yield return null;

                        silence = false;

                        if (!isNative)
                        {
                            onSpeakAudioGenerationStart(wrapper);
                        }

                        do
                        {
                            yield return new WaitForSeconds(waitForSeconds);
                        } while (!silence && ttsHandler.IsBusy());

                        using (WWW www = new WWW("file://" + outputFile + "/" + wrapper.Uid + extension))
                        {

                            if (string.IsNullOrEmpty(www.error))
                            {
                                AudioClip ac = www.GetAudioClip(false, false, AudioType.WAV);

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
                            }
                            else
                            {
                                string errorMessage = "Could not read the file: " + www.error;
                                Debug.LogError(errorMessage);
                                onErrorInfo(errorMessage);
                            }
                        }

                        if (!isNative)
                        {
                            onSpeakAudioGenerationComplete(wrapper);
                        }
                    }
                }
            }


#else
            yield return null;
#endif
        }

#if (UNITY_WSA_10_0 || UNITY_WSA || UNITY_EDITOR) && !UNITY_WEBPLAYER

        private void initializeTTS()
        {
            if (Util.Constants.DEBUG)
                Debug.Log("Initializing TTS...");

            ttsHandler = new RTVoiceUWPBridge();

            ttsHandler.DEBUG(Util.Constants.DEBUG);

            //myPath = TTSHandler.GetTargetFolder();

            isInitialized = true;
        }

#endif

        #endregion


        #region Editor-only methods


#if UNITY_EDITOR

        public override void GenerateInEditor(Model.Wrapper wrapper)
        {
            Debug.LogError("GenerateInEditor is not supported for Unity WSA!");
        }

        public override void SpeakNativeInEditor(Model.Wrapper wrapper)
        {
            Debug.LogError("SpeakNativeInEditor is not supported for Unity WSA!");
        }

#endif

        #endregion

    }
}
// © 2016-2017 crosstales LLC (https://www.crosstales.com)