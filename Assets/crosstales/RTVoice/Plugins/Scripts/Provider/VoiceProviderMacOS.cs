using UnityEngine;
using System.Collections;

namespace Crosstales.RTVoice.Provider
{
    /// <summary>MacOS voice provider.</summary>
    public class VoiceProviderMacOS : BaseVoiceProvider
    {

        #region Variables

        private static System.Collections.Generic.List<Model.Voice> cachedVoices;

#if (UNITY_STANDALONE_OSX || UNITY_EDITOR) && !UNITY_WEBPLAYER
        private static readonly System.Text.RegularExpressions.Regex sayRegex = new System.Text.RegularExpressions.Regex(@"^([^#]+?)\s*([^ ]+)\s*# (.*?)$");
#endif

        private const int defaultRate = 175;
        private const string extension = ".aiff";

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
#if (UNITY_STANDALONE_OSX || UNITY_EDITOR) && !UNITY_WEBPLAYER
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
                    yield return null;

                    string speaker = string.Empty;
                    int calculatedRate = calculateRate(wrapper.Rate);

                    if (wrapper.Voice == null || string.IsNullOrEmpty(wrapper.Voice.Name))
                    {
                        Debug.LogWarning("'Voice' or 'Voice.Name' is null! Using the OS 'default' voice.");
                    }
                    else
                    {
                        speaker = wrapper.Voice.Name;
                    }

                    using (System.Diagnostics.Process speakProcess = new System.Diagnostics.Process())
                    {

                        string args = (string.IsNullOrEmpty(speaker) ? string.Empty : (" -v \"" + speaker.Replace('"', '\'') + '"')) +
                                                        (calculatedRate != defaultRate ? (" -r " + calculatedRate) : string.Empty) + " \"" +
                                                        wrapper.Text.Replace('"', '\'') + '"';

                        if (Util.Constants.DEBUG)
                            Debug.Log("Process argruments: " + args);

                        speakProcess.StartInfo.FileName = Util.Constants.TTS_MACOS;
                        speakProcess.StartInfo.Arguments = args;
                        speakProcess.StartInfo.CreateNoWindow = true;
                        speakProcess.StartInfo.RedirectStandardOutput = true;
                        speakProcess.StartInfo.RedirectStandardError = true;
                        speakProcess.StartInfo.UseShellExecute = false;
                        speakProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
                        //* Set your output and error (asynchronous) handlers
                        //speakProcess.OutputDataReceived += new DataReceivedEventHandler(speakNativeHandler);
                        //scanProcess.ErrorDataReceived += new DataReceivedEventHandler(ErrorHandler);

                        speakProcess.Start();
                        //speakProcess.BeginOutputReadLine();
                        //speakProcess.BeginErrorReadLine();

                        silence = false;
                        processes.Add(wrapper.Uid, speakProcess);
                        onSpeakStart(wrapper);

                        do
                        {
                            yield return null;

                        } while (!speakProcess.HasExited);

                        if (speakProcess.ExitCode == 0 || speakProcess.ExitCode == -1)
                        { //0 = normal ended, -1 = killed
                            if (Util.Constants.DEBUG)
                                Debug.Log("Text spoken: " + wrapper.Text);
                        }
                        else
                        {
                            using (System.IO.StreamReader sr = speakProcess.StandardError)
                            {
                                string errorMessage = "Could not speak the text: " + speakProcess.ExitCode + System.Environment.NewLine + sr.ReadToEnd();
                                Debug.LogError(errorMessage);
                                onErrorInfo(errorMessage);
                            }
                        }

                        processes.Remove(wrapper.Uid);
                        onSpeakComplete(wrapper);
                    }
                }
            }
#else
            yield return null;
#endif
        }

        public override IEnumerator Speak(Model.Wrapper wrapper)
        {
#if (UNITY_STANDALONE_OSX || UNITY_EDITOR) && !UNITY_WEBPLAYER
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
                        yield return null;

                        string speaker = string.Empty;
                        int calculatedRate = calculateRate(wrapper.Rate);

                        if (wrapper.Voice == null || string.IsNullOrEmpty(wrapper.Voice.Name))
                        {
                            Debug.LogWarning("'Voice' or 'Voice.Name' is null! Using the OS 'default' voice.");
                        }
                        else
                        {
                            speaker = wrapper.Voice.Name;
                        }

                        string outputFile = Util.Constants.AUDIOFILE_PATH + wrapper.Uid + extension;

                        using (System.Diagnostics.Process speakToFileProcess = new System.Diagnostics.Process())
                        {

                            string args = (string.IsNullOrEmpty(speaker) ? string.Empty : (" -v \"" + speaker.Replace('"', '\'') + '"')) +
                                          (calculatedRate != defaultRate ? (" -r " + calculatedRate) : string.Empty) + " -o \"" +
                                          outputFile.Replace('"', '\'') + '"' +
                                          " --file-format=AIFFLE" + " \"" +
                                          wrapper.Text.Replace('"', '\'') + '"';

                            if (Util.Constants.DEBUG)
                                Debug.Log("Process argruments: " + args);

                            speakToFileProcess.StartInfo.FileName = Util.Constants.TTS_MACOS;
                            speakToFileProcess.StartInfo.Arguments = args;
                            speakToFileProcess.StartInfo.CreateNoWindow = true;
                            speakToFileProcess.StartInfo.RedirectStandardOutput = true;
                            speakToFileProcess.StartInfo.RedirectStandardError = true;
                            speakToFileProcess.StartInfo.UseShellExecute = false;
                            speakToFileProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
                            //* Set your output and error (asynchronous) handlers
                            //speakToFileProcess.OutputDataReceived += new DataReceivedEventHandler(speakNativeHandler);
                            //speakToFileProcess.ErrorDataReceived += new DataReceivedEventHandler(ErrorHandler);

                            speakToFileProcess.Start();
                            //speakToFileProcess.BeginOutputReadLine();
                            //speakToFileProcess.BeginErrorReadLine();

                            silence = false;
                            onSpeakAudioGenerationStart(wrapper);

                            do
                            {
                                yield return null;
                            } while (!speakToFileProcess.HasExited);

                            if (speakToFileProcess.ExitCode == 0)
                            {

                                using (WWW www = new WWW("file:///" + outputFile))
                                {

                                    if (string.IsNullOrEmpty(www.error))
                                    {
                                        AudioClip ac = www.GetAudioClip(false, false, AudioType.AIFF);

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
                            }
                            else
                            {
                                using (System.IO.StreamReader sr = speakToFileProcess.StandardError)
                                {
                                    string errorMessage = "Could not speak the text: " + speakToFileProcess.ExitCode + System.Environment.NewLine + sr.ReadToEnd();
                                    Debug.LogError(errorMessage);
                                    onErrorInfo(errorMessage);
                                }
                            }

                            onSpeakAudioGenerationComplete(wrapper);
                        }
                    }
                }
            }
#else
            yield return null;
#endif
        }

        #endregion


        #region Private methods

        private void getVoices()
        {
            cachedVoices = new System.Collections.Generic.List<Model.Voice>();

#if (UNITY_STANDALONE_OSX || UNITY_EDITOR) && !UNITY_WEBPLAYER
            using (System.Diagnostics.Process voicesProcess = new System.Diagnostics.Process())
            {
                voicesProcess.StartInfo.FileName = Util.Constants.TTS_MACOS;
                voicesProcess.StartInfo.Arguments = "-v '?'";
                voicesProcess.StartInfo.CreateNoWindow = true;
                voicesProcess.StartInfo.RedirectStandardOutput = true;
                voicesProcess.StartInfo.RedirectStandardError = true;
                voicesProcess.StartInfo.UseShellExecute = false;
                voicesProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
                //* Set your output and error (asynchronous) handlers
                //voicesProcess.OutputDataReceived += new DataReceivedEventHandler(speakNativeHandler);
                //voicesProcess.ErrorDataReceived += new DataReceivedEventHandler(ErrorHandler);

                try
                {
                    voicesProcess.Start();

                    voicesProcess.WaitForExit(Util.Constants.TTS_KILL_TIME);

                    if (voicesProcess.ExitCode == 0)
                    {
                        using (System.IO.StreamReader streamReader = voicesProcess.StandardOutput)
                        {
                            string reply;
                            while (!streamReader.EndOfStream)
                            {
                                reply = streamReader.ReadLine();

                                if (!string.IsNullOrEmpty(reply))
                                {
                                    System.Text.RegularExpressions.Match match = sayRegex.Match(reply);

                                    if (match.Success)
                                    {
                                        cachedVoices.Add(new Model.Voice(match.Groups[1].ToString(), match.Groups[3].ToString(), match.Groups[2].ToString().Replace('_', '-')));
                                    }
                                }
                            }
                        }

                        if (Util.Constants.DEBUG)
                            Debug.Log("Voices read: " + cachedVoices.CTDump());
                    }
                    else
                    {
                        using (System.IO.StreamReader sr = voicesProcess.StandardError)
                        {
                            string errorMessage = "Could not get any voices: " + voicesProcess.ExitCode + System.Environment.NewLine + sr.ReadToEnd();
                            Debug.LogError(errorMessage);
                            onErrorInfo(errorMessage);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    string errorMessage = "Could not get any voices!" + System.Environment.NewLine + ex;
                    Debug.LogError(errorMessage);
                    onErrorInfo(errorMessage);
                }
            }
#endif
        }

        private int calculateRate(float rate)
        {
            return Mathf.Clamp(rate != 1f ? (int)(defaultRate * rate) : defaultRate, 1, 3 * defaultRate);
        }

        #endregion


        #region Editor-only methods

#if UNITY_EDITOR

        public override void GenerateInEditor(Model.Wrapper wrapper)
        {
#if !UNITY_WEBPLAYER
            if (wrapper == null)
            {
                Debug.LogWarning("'wrapper' is null!");
            }
            else
            {
                if (string.IsNullOrEmpty(wrapper.Text))
                {
                    Debug.LogWarning("'Text' is null or empty: " + wrapper);
                }
                else
                {
                    string speaker = string.Empty;
                    int calculatedRate = calculateRate(wrapper.Rate);

                    if (wrapper.Voice == null || string.IsNullOrEmpty(wrapper.Voice.Name))
                    {
                        Debug.LogWarning("'Voice' or 'Voice.Name' is null! Using the OS 'default' voice.");
                    }
                    else
                    {
                        speaker = wrapper.Voice.Name;
                    }

                    string outputFile = Util.Constants.AUDIOFILE_PATH + wrapper.Uid + extension;

                    using (System.Diagnostics.Process speakToFileProcess = new System.Diagnostics.Process())
                    {

                        string args = (string.IsNullOrEmpty(speaker) ? string.Empty : (" -v \"" + speaker.Replace('"', '\'') + '"')) +
                                                        (calculatedRate != defaultRate ? (" -r " + calculatedRate) : string.Empty) + " -o \"" +
                                                        outputFile.Replace('"', '\'') + '"' +
                                                        " --file-format=AIFFLE" + " \"" +
                                                        wrapper.Text.Replace('"', '\'') + '"';

                        if (Util.Constants.DEBUG)
                            Debug.Log("Process argruments: " + args);

                        speakToFileProcess.StartInfo.FileName = Util.Constants.TTS_MACOS;
                        speakToFileProcess.StartInfo.Arguments = args;
                        speakToFileProcess.StartInfo.CreateNoWindow = true;
                        speakToFileProcess.StartInfo.RedirectStandardOutput = true;
                        speakToFileProcess.StartInfo.RedirectStandardError = true;
                        speakToFileProcess.StartInfo.UseShellExecute = false;
                        speakToFileProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;

                        speakToFileProcess.Start();

                        silence = false;

                        do
                        {
                            System.Threading.Thread.Sleep(50);
                        } while (!speakToFileProcess.HasExited);

                        if (speakToFileProcess.ExitCode == 0)
                        {

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
                                        Debug.LogError("Could not delete file '" + outputFile + "'!" + System.Environment.NewLine + ex);
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

                            if (Util.Constants.DEBUG)
                                Debug.Log("Text generated: " + wrapper.Text);
                        }
                        else
                        {
                            using (System.IO.StreamReader sr = speakToFileProcess.StandardError)
                            {
                                Debug.LogError("Could not speak the text: " + speakToFileProcess.ExitCode + System.Environment.NewLine + sr.ReadToEnd());
                            }
                        }
                    }
                }
            }
#endif
        }

        public override void SpeakNativeInEditor(Model.Wrapper wrapper)
        {
#if !UNITY_WEBPLAYER
            if (wrapper == null)
            {
                Debug.LogWarning("'wrapper' is null!");
            }
            else
            {
                if (string.IsNullOrEmpty(wrapper.Text))
                {
                    Debug.LogWarning("'Text' is null or empty: " + wrapper);
                }
                else
                {
                    string speaker = string.Empty;
                    int calculatedRate = calculateRate(wrapper.Rate);

                    if (wrapper.Voice == null || string.IsNullOrEmpty(wrapper.Voice.Name))
                    {
                        Debug.LogWarning("'Voice' or 'Voice.Name' is null! Using the OS 'default' voice.");
                    }
                    else
                    {
                        speaker = wrapper.Voice.Name;
                    }

                    using (System.Diagnostics.Process speakProcess = new System.Diagnostics.Process())
                    {

                        string args = (string.IsNullOrEmpty(speaker) ? string.Empty : (" -v \"" + speaker.Replace('"', '\'') + '"')) +
                                                        (calculatedRate != defaultRate ? (" -r " + calculatedRate) : string.Empty) + " \"" +
                                                        wrapper.Text.Replace('"', '\'') + '"';

                        if (Util.Constants.DEBUG)
                            Debug.Log("Process argruments: " + args);

                        speakProcess.StartInfo.FileName = Util.Constants.TTS_MACOS;
                        speakProcess.StartInfo.Arguments = args;
                        speakProcess.StartInfo.CreateNoWindow = true;
                        speakProcess.StartInfo.RedirectStandardOutput = true;
                        speakProcess.StartInfo.RedirectStandardError = true;
                        speakProcess.StartInfo.UseShellExecute = false;
                        speakProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;

                        speakProcess.Start();
                        silence = false;

                        do
                        {
                            System.Threading.Thread.Sleep(50);

                            if (silence)
                            {
                                speakProcess.Kill();
                            }
                        } while (!speakProcess.HasExited);

                        if (speakProcess.ExitCode == 0 || speakProcess.ExitCode == -1)
                        { //0 = normal ended, -1 = killed
                            if (Util.Constants.DEBUG)
                                Debug.Log("Text spoken: " + wrapper.Text);
                        }
                        else
                        {
                            using (System.IO.StreamReader sr = speakProcess.StandardError)
                            {
                                Debug.LogError("Could not speak the text: " + speakProcess.ExitCode + System.Environment.NewLine + sr.ReadToEnd());
                            }
                        }
                    }
                }
            }
#endif
        }

#endif

        #endregion
    }
}
// © 2015-2017 crosstales LLC (https://www.crosstales.com)