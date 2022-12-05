using UnityEngine;
using System.Linq;

namespace Crosstales.RTVoice
{
    /// <summary>Main component of RTVoice.</summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [HelpURL("https://www.crosstales.com/media/data/assets/rtvoice/api/class_crosstales_1_1_r_t_voice_1_1_speaker.html")]
    public class Speaker : MonoBehaviour
    {

        #region Variables

        [Header("Mary TTS")]
        [Tooltip("Enable or disable MaryTTS (default: off).")]
        public bool MaryTTSMode = false;

        [Tooltip("Server URL for MaryTTS.")]
        public string MaryTTSURL = "http://mary.dfki.de";

        [Tooltip("Server port for MaryTTS.")]
        [Range(0, 65535)]
        public int MaryTTSPort = 59125;

        private System.Collections.Generic.Dictionary<System.Guid, AudioSource> removeSources = new System.Collections.Generic.Dictionary<System.Guid, AudioSource>();

        private float cleanUpTimer = 0f;

        private const float cleanUpTime = 5f;

        private static Provider.BaseVoiceProvider voiceProvider;
        private static Speaker speaker;
        private static bool initalized = false;

        private static System.Collections.Generic.Dictionary<System.Guid, AudioSource> genericSources = new System.Collections.Generic.Dictionary<System.Guid, AudioSource>();
        private static System.Collections.Generic.Dictionary<System.Guid, AudioSource> providedSources = new System.Collections.Generic.Dictionary<System.Guid, AudioSource>();

        private static GameObject go;

        private static bool loggedUnsupportedPlatform = false;
        private static bool loggedVPIsNull = false;

        private static bool loggedOnlyOneInstance = false;

        private static char[] splitCharWords = new char[] { ' ' };

        private static bool maryTTS;

        #endregion


        #region Events

        public delegate void SpeakNativeCurrentWord(Model.Event.CurrentWordEventArgs e);

        public delegate void SpeakNativeCurrentPhoneme(Model.Event.CurrentPhonemeEventArgs e);

        public delegate void SpeakNativeCurrentViseme(Model.Event.CurrentVisemeEventArgs e);

        public delegate void SpeakStart(Model.Event.SpeakEventArgs e);

        public delegate void SpeakComplete(Model.Event.SpeakEventArgs e);

        public delegate void SpeakAudioGenerationStart(Model.Event.SpeakEventArgs e);

        public delegate void SpeakAudioGenerationComplete(Model.Event.SpeakEventArgs e);

        public delegate void ErrorInfo(string info);

        public delegate void ProviderChange(string provider);

        /// <summary>An event triggered whenever a new word is spoken (native mode).</summary>
        public static event SpeakNativeCurrentWord OnSpeakNativeCurrentWord;

        /// <summary>An event triggered whenever a new phoneme is spoken (native mode).</summary>
        public static event SpeakNativeCurrentPhoneme OnSpeakNativeCurrentPhoneme;

        /// <summary>An event triggered whenever a new viseme is spoken  (native mode).</summary>
        public static event SpeakNativeCurrentViseme OnSpeakNativeCurrentViseme;

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

        /// <summary>An event triggered whenever an provider has changed.</summary>
        public static event ProviderChange OnProviderChange;

        #endregion


        #region MonoBehaviour methods

        public void OnEnable()
        {
            if (Util.Helper.isEditorMode || !initalized)
            {

                //Debug.Log("Creating new 'speaker'instance");

                //                if (speaker == null)
                //                {
                speaker = this;

                go = gameObject;

                go.name = Util.Constants.RTVOICE_SCENE_OBJECT_NAME;

                //maryTTS = MaryTTS;

                initProvider();

                // Subscribe event listeners
                Provider.BaseVoiceProvider.OnSpeakCurrentWord += onSpeakNativeCurrentWord;
                Provider.BaseVoiceProvider.OnSpeakCurrentPhoneme += onSpeakNativeCurrentPhoneme;
                Provider.BaseVoiceProvider.OnSpeakCurrentViseme += onSpeakNativeCurrentViseme;
                Provider.BaseVoiceProvider.OnSpeakStart += onSpeakStart;
                Provider.BaseVoiceProvider.OnSpeakComplete += onSpeakComplete;
                Provider.BaseVoiceProvider.OnSpeakAudioGenerationStart += onSpeakAudioGenerationStart;
                Provider.BaseVoiceProvider.OnSpeakAudioGenerationComplete += onSpeakAudioGenerationComplete;
                Provider.BaseVoiceProvider.OnErrorInfo += onErrorInfo;
                //                }
                //                else
                //                {
                //                    Debug.LogWarning("'speaker' wasn't null!");
                //                }

                if (!Util.Helper.isEditorMode && Util.Constants.DONT_DESTROY_ON_LOAD)
                {
                    DontDestroyOnLoad(transform.root.gameObject);
                    initalized = true;
                }

            }
            else
            {
                //Debug.Log("Re-using 'speaker'instance");

                if (!Util.Helper.isEditorMode && Util.Constants.DONT_DESTROY_ON_LOAD)
                {
                    if (!loggedOnlyOneInstance)
                    {
                        Debug.LogWarning("Only one active instance of 'RTVoice' allowed in all scenes!" + System.Environment.NewLine + "This object will now be destroyed.");

                        Destroy(gameObject, 0.2f);

                        //loggedOnlyOneInstance = true;
                    }
                }
            }

            if (!Util.Helper.hasBuiltInTTS)
            {
                MaryMode = true;
            }
        }

        public void Update()
        {
            cleanUpTimer += Time.deltaTime;

            if (cleanUpTimer > cleanUpTime)
            {
                cleanUpTimer = 0f;

                if (genericSources.Count > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<System.Guid, AudioSource> source in genericSources)
                    {
                        if (source.Value != null && source.Value.clip != null && !source.Value.isPlaying)
                        {
                            removeSources.Add(source.Key, source.Value);
                        }
                    }

                    foreach (System.Collections.Generic.KeyValuePair<System.Guid, AudioSource> source in removeSources)
                    {
                        genericSources.Remove(source.Key);
                        Destroy(source.Value);
                    }

                    removeSources.Clear();
                }

                if (providedSources.Count > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<System.Guid, AudioSource> source in providedSources)
                    {
                        if (source.Value != null && source.Value.clip != null && !source.Value.isPlaying)
                        {
                            source.Value.clip = null; //remove clip

                            removeSources.Add(source.Key, source.Value);
                        }
                    }

                    foreach (System.Collections.Generic.KeyValuePair<System.Guid, AudioSource> source in removeSources)
                    {
                        //genericSources.Remove(source.Key);
                        providedSources.Remove(source.Key);
                    }

                    removeSources.Clear();
                }
            }

            if (MaryMode != maryTTS) //update providers
            {
                Silence();

                initProvider();
            }

            if (Util.Helper.isEditorMode)
            {
                if (go != null)
                {
                    go.name = Util.Constants.RTVOICE_SCENE_OBJECT_NAME; //ensure name
                }
            }
        }

        public void OnDestroy()
        {
            if (go == gameObject)
            {
                if (voiceProvider != null)
                {
                    voiceProvider.Silence();
                }

                // Unsubscribe event listeners
                Provider.BaseVoiceProvider.OnSpeakCurrentWord -= onSpeakNativeCurrentWord;
                Provider.BaseVoiceProvider.OnSpeakCurrentPhoneme -= onSpeakNativeCurrentPhoneme;
                Provider.BaseVoiceProvider.OnSpeakCurrentViseme -= onSpeakNativeCurrentViseme;
                Provider.BaseVoiceProvider.OnSpeakStart -= onSpeakStart;
                Provider.BaseVoiceProvider.OnSpeakComplete -= onSpeakComplete;
                Provider.BaseVoiceProvider.OnSpeakAudioGenerationStart -= onSpeakAudioGenerationStart;
                Provider.BaseVoiceProvider.OnSpeakAudioGenerationComplete -= onSpeakAudioGenerationComplete;
                Provider.BaseVoiceProvider.OnErrorInfo -= onErrorInfo;
            }
        }

        public void OnApplicationQuit()
        {
            if (voiceProvider != null)
            {
                if (speaker != null)
                {
                    speaker.StopAllCoroutines();
                }

                voiceProvider.Silence();

                if (Util.Helper.isAndroidPlatform)
                {
                    ((Provider.VoiceProviderAndroid)voiceProvider).ShutdownTTS();
                }
            }
        }

        #endregion


        #region Static properties

        /// <summary>Enables or disables MaryTTS.</summary>
        public static bool MaryMode
        {
            get
            {
                return speaker.MaryTTSMode;
            }

            set
            {
                speaker.MaryTTSMode = value;
            }
        }

        /// <summary>Server URL for MaryTTS.</summary>
        public static string MaryURL
        {
            get
            {
                return speaker.MaryTTSURL;
            }

            set
            {
                speaker.MaryTTSURL = value;
            }
        }

        /// <summary>Server port for MaryTTS.</summary>
        public static int MaryPort
        {
            get
            {
                return speaker.MaryTTSPort;
            }

            set
            {
                speaker.MaryTTSPort = value;
            }
        }

        /// <summary>Returns the extension of the generated audio files.</summary>
        /// <returns>Extension of the generated audio files.</returns>
        public static string AudioFileExtension
        {
            get
            {
                if (Util.Helper.isSupportedPlatform)
                {
                    if (voiceProvider != null)
                    {
                        return voiceProvider.AudioFileExtension;
                    }
                    else
                    {
                        logVPIsNull();
                    }
                }
                else
                {
                    logUnsupportedPlatform();
                }

                return string.Empty;
            }
        }

        /// <summary>Get all available voices from the current TTS-system.</summary>
        /// <returns>All available voices (alphabetically ordered by 'Name') as a list.</returns>
        public static System.Collections.Generic.List<Model.Voice> Voices
        {
            get
            {
                if (Util.Helper.isSupportedPlatform)
                {
                    if (voiceProvider != null)
                    {
                        return voiceProvider.Voices.OrderBy(s => s.Name).ToList();
                    }
                    else
                    {
                        logVPIsNull();
                    }
                }
                else
                {
                    logUnsupportedPlatform();
                }

                return new System.Collections.Generic.List<Model.Voice>();
            }
        }

        /// <summary>Get all available cultures from the current TTS-system..</summary>
        /// <returns>All available cultures (alphabetically ordered by 'Culture') as a list.</returns>
        public static System.Collections.Generic.List<string> Cultures
        {
            get
            {
                System.Collections.Generic.List<string> result = new System.Collections.Generic.List<string>();

                if (Util.Helper.isSupportedPlatform)
                {
                    if (voiceProvider != null)
                    {
                        System.Collections.Generic.IEnumerable<Model.Voice> cultures = voiceProvider.Voices.GroupBy(cul => cul.Culture).Select(grp => grp.First()).OrderBy(s => s.Culture).ToList();

                        foreach (Model.Voice voice in cultures)
                        {
                            result.Add(voice.Culture);
                        }
                    }
                    else
                    {
                        logVPIsNull();
                    }
                }
                else
                {
                    logUnsupportedPlatform();
                }

                return result;
            }
        }

        /// <summary>Checks if TTS is available on this system.</summary>
        /// <returns>True if TTS is available on this system.</returns>
        public static bool isTTSAvailable
        {
            get
            {
                if (Util.Helper.isSupportedPlatform)
                {
                    if (voiceProvider != null)
                    {
                        return voiceProvider.Voices.Count > 0;
                    }
                    else
                    {
                        logVPIsNull();
                    }
                }
                else
                {
                    logUnsupportedPlatform();
                }

                return false;
            }
        }

        #endregion


        #region Static methods

        /// <summary>
        /// Approximates the speech length in seconds of a given text and rate. 
        /// Note: This is an experimental method and doesn't provide an exact value; +/- 15% is "normal"!
        /// </summary>
        /// <param name="text">Text for the length approximation.</param>
        /// <param name="rate">Speech rate of the speaker in percent for the length approximation (1 = 100%, default: 1, optional).</param>
        /// <param name="wordsPerMinute">Words per minute (default: 175, optional).</param>
        /// <param name="timeFactor">Time factor for the calculated value (default: 0.9, optional).</param>
        /// <returns>Approximated speech length in seconds of the given text and rate.</returns>
        public static float ApproximateSpeechLength(string text, float rate = 1f, float wordsPerMinute = 175f, float timeFactor = 0.9f)
        {
            float words = (float)text.Split(splitCharWords, System.StringSplitOptions.RemoveEmptyEntries).Length;
            float characters = (float)text.Length - words + 1;
            float ratio = characters / words;

            //Debug.Log("words: " + words);
            //Debug.Log("characters: " + characters);
            //Debug.Log("ratio: " + ratio);

            if (Util.Helper.isWindowsPlatform)
            {
                if (rate != 1f)
                { //relevant?
                    if (rate > 1f)
                    { //larger than 1
                        if (rate >= 2.75f)
                        {
                            rate = 2.78f;
                        }
                        else if (rate >= 2.6f && rate < 2.75f)
                        {
                            rate = 2.6f;
                        }
                        else if (rate >= 2.35f && rate < 2.6f)
                        {
                            rate = 2.39f;
                        }
                        else if (rate >= 2.2f && rate < 2.35f)
                        {
                            rate = 2.2f;
                        }
                        else if (rate >= 2f && rate < 2.2f)
                        {
                            rate = 2f;
                        }
                        else if (rate >= 1.8f && rate < 2f)
                        {
                            rate = 1.8f;
                        }
                        else if (rate >= 1.6f && rate < 1.8f)
                        {
                            rate = 1.6f;
                        }
                        else if (rate >= 1.4f && rate < 1.6f)
                        {
                            rate = 1.45f;
                        }
                        else if (rate >= 1.2f && rate < 1.4f)
                        {
                            rate = 1.28f;
                        }
                        else if (rate > 1f && rate < 1.2f)
                        {
                            rate = 1.14f;
                        }
                    }
                    else
                    { //smaller than 1
                        if (rate <= 0.3f)
                        {
                            rate = 0.33f;
                        }
                        else if (rate > 0.3 && rate <= 0.4f)
                        {
                            rate = 0.375f;
                        }
                        else if (rate > 0.4 && rate <= 0.45f)
                        {
                            rate = 0.42f;
                        }
                        else if (rate > 0.45 && rate <= 0.5f)
                        {
                            rate = 0.47f;
                        }
                        else if (rate > 0.5 && rate <= 0.55f)
                        {
                            rate = 0.525f;
                        }
                        else if (rate > 0.55 && rate <= 0.6f)
                        {
                            rate = 0.585f;
                        }
                        else if (rate > 0.6 && rate <= 0.7f)
                        {
                            rate = 0.655f;
                        }
                        else if (rate > 0.7 && rate <= 0.8f)
                        {
                            rate = 0.732f;
                        }
                        else if (rate > 0.8 && rate <= 0.9f)
                        {
                            rate = 0.82f;
                        }
                        else if (rate > 0.9 && rate < 1f)
                        {
                            rate = 0.92f;
                        }
                    }
                }
            }

            float speechLength = words / ((wordsPerMinute / 60) * rate);

            //Debug.Log("speechLength before: " + speechLength);

            if (ratio < 2)
            {
                speechLength *= 1f;
            }
            else if (ratio >= 2f && ratio < 3f)
            {
                speechLength *= 1.05f;
            }
            else if (ratio >= 3f && ratio < 3.5f)
            {
                speechLength *= 1.15f;
            }
            else if (ratio >= 3.5f && ratio < 4f)
            {
                speechLength *= 1.2f;
            }
            else if (ratio >= 4f && ratio < 4.5f)
            {
                speechLength *= 1.25f;
            }
            else if (ratio >= 4.5f && ratio < 5f)
            {
                speechLength *= 1.3f;
            }
            else if (ratio >= 5f && ratio < 5.5f)
            {
                speechLength *= 1.4f;
            }
            else if (ratio >= 5.5f && ratio < 6f)
            {
                speechLength *= 1.45f;
            }
            else if (ratio >= 6f && ratio < 6.5f)
            {
                speechLength *= 1.5f;
            }
            else if (ratio >= 6.5f && ratio < 7f)
            {
                speechLength *= 1.6f;
            }
            else if (ratio >= 7f && ratio < 8f)
            {
                speechLength *= 1.7f;
            }
            else if (ratio >= 8f && ratio < 9f)
            {
                speechLength *= 1.8f;
            }
            else
            {
                speechLength *= ((ratio * ((ratio / 100f) + 0.02f)) + 1f);
            }

            if (speechLength < 0.8f)
            {
                speechLength += 0.6f;
            }

            //Debug.Log("speechLength after: " + speechLength);

            return speechLength * timeFactor;
        }

        /// <summary>Get all available voices for a given culture from the current TTS-system.</summary>
        /// <param name="culture">Culture of the voice (e.g. "en")</param>
        /// <returns>All available voices (alphabetically ordered by 'Name') for a given culture as a list.</returns>
        public static System.Collections.Generic.List<Model.Voice> VoicesForCulture(string culture)
        {
            if (Util.Helper.isSupportedPlatform)
            {
                if (voiceProvider != null)
                {
                    if (string.IsNullOrEmpty(culture))
                    {
                        Debug.LogWarning("The given 'culture' is null or empty! Returning all available voices.");

                        return Voices;
                    }
                    else
                    {
#if UNITY_WSA_10_0 || UNITY_WSA
                        return voiceProvider.Voices.Where(s => s.Culture.StartsWith(culture, System.StringComparison.OrdinalIgnoreCase)).OrderBy(s => s.Name).ToList();
#else
                        return voiceProvider.Voices.Where(s => s.Culture.StartsWith(culture, System.StringComparison.InvariantCultureIgnoreCase)).OrderBy(s => s.Name).ToList();
#endif
                    }
                }
                else
                {
                    logVPIsNull();
                }
            }
            else
            {
                logUnsupportedPlatform();
            }

            return new System.Collections.Generic.List<Model.Voice>();
        }

        /// <summary>Get a voice from for a given culture and otional index from the current TTS-system.</summary>
        /// <param name="culture">Culture of the voice (e.g. "en")</param>
        /// <param name="index">Index of the voice (default = 0, optional)</param>
        /// <returns>Voice for the given culture and index.</returns>
        public static Model.Voice VoiceForCulture(string culture, int index = 0)
        {
            Model.Voice result = null;

            if (!string.IsNullOrEmpty(culture))
            {
                System.Collections.Generic.List<Model.Voice> voices = VoicesForCulture(culture);

                if (voices.Count > 0)
                {
                    if (voices.Count - 1 >= index && index >= 0)
                    {
                        result = voices[index];
                    }
                    else
                    {
                        result = voices[0];
                        Debug.LogWarning("No voices for culture '" + culture + "' with index '" + index + "' found! Speaking with the default voice!");
                    }
                }
                else
                { //use the default voice
                    Debug.LogWarning("No voice for culture '" + culture + "' found! Speaking with the default voice!");
                    //result = null;
                }
            }
            return result;
        }

        /// <summary>Get a voice for a given name from the current TTS-system.</summary>
        /// <param name="name">Name of the voice (e.g. "Alex")</param>
        /// <returns>Voice for the given name or null if not found.</returns>
        public static Model.Voice VoiceForName(string name)
        {
            Model.Voice result = null;

            if (Util.Helper.isSupportedPlatform)
            {
                if (string.IsNullOrEmpty(name))
                {
                    Debug.LogWarning("The given 'name' is null or empty! Returning null.");
                }
                else
                {
                    if (voiceProvider != null)
                    {
                        foreach (Model.Voice voice in voiceProvider.Voices)
                        {
                            if (name.Equals(voice.Name))
                            {
                                result = voice;
                                break;
                            }
                        }
                    }
                    else
                    {
                        logVPIsNull();
                    }
                }
            }
            else
            {
                logUnsupportedPlatform();
            }

            return result;
        }

        /// <summary>Speaks a text with a given voice (native mode).</summary>
        /// <param name="text">Text to speak.</param>
        /// <param name="voice">Voice to speak (optional).</param>
        /// <param name="rate">Speech rate of the speaker in percent (1 = 100%, default: 1, optional).</param>
        /// <param name="volume">Volume of the speaker in percent (1 = 100%, default: 1, optional).</param>
        /// <param name="pitch">Pitch of the speech in percent (1 = 100%, default: 1, optional).</param>
        /// <returns>UID of the speaker.</returns>
        public static System.Guid SpeakNative(string text, Model.Voice voice = null, float rate = 1f, float volume = 1f, float pitch = 1f)
        {
            Model.Wrapper wrapper = new Model.Wrapper(text, voice, rate, pitch, volume);

            SpeakNativeWithUID(wrapper);

            return wrapper.Uid;
        }

        /// <summary>Speaks a text with a given voice (native mode).</summary>
        /// <param name="wrapper">Speak wrapper.</param>
        public static void SpeakNativeWithUID(Model.Wrapper wrapper)
        {
            if (Util.Helper.isSupportedPlatform)
            {
                if (wrapper != null)
                {
                    if (voiceProvider != null)
                    {
                        if (string.IsNullOrEmpty(wrapper.Text))
                        {
                            Debug.LogWarning("'Text' is null or empty!");
                        }
                        else
                        {
                            if (speaker != null)
                            {
                                if (Util.Helper.isWSAPlatform || MaryMode) //add an AudioSource for providers without native support
                                {
                                    if (wrapper.Source == null)
                                    {
                                        wrapper.Source = go.AddComponent<AudioSource>();
                                        genericSources.Add(wrapper.Uid, wrapper.Source);
                                    }
                                    else
                                    {
                                        if (!providedSources.ContainsKey(wrapper.Uid))
                                        {
                                            providedSources.Add(wrapper.Uid, wrapper.Source);
                                        }
                                    }

                                    wrapper.SpeakImmediately = true; //must always speak immediately
                                }

                                speaker.StartCoroutine(voiceProvider.SpeakNative(wrapper));
                            }
                        }
                    }
                    else
                    {
                        logVPIsNull();
                    }
                }
                else
                {
                    logWrapperIsNull();
                }
            }
            else
            {
                logUnsupportedPlatform();
            }
        }

        /// <summary>Speaks a text with a given wrapper (native mode).</summary>
        /// <param name="wrapper">Speak wrapper.</param>
        /// <returns>UID of the speaker.</returns>
        public static System.Guid SpeakNative(Model.Wrapper wrapper)
        {
            if (wrapper != null)
            {
                SpeakNativeWithUID(wrapper);

                return wrapper.Uid;
            }
            else
            {
                logWrapperIsNull();
            }

            return System.Guid.NewGuid(); //fake uid
        }

        /// <summary>Speaks a text with a given voice.</summary>
        /// <param name="text">Text to speak.</param>
        /// <param name="source">AudioSource for the output (optional).</param>
        /// <param name="voice">Voice to speak (optional).</param>
        /// <param name="speakImmediately">Speak the text immediately (default: true). Only works if 'Source' is not null.</param>
        /// <param name="rate">Speech rate of the speaker in percent (1 = 100%, default: 1, optional).</param>
        /// <param name="volume">Volume of the speaker in percent (1 = 100%, default: 1, optional).</param>///
        /// <param name="outputFile">Saves the generated audio to an output file (without extension, optional).</param>
        /// <param name="pitch">Pitch of the speech in percent (1 = 100%, default: 1, optional).</param>
        /// <returns>UID of the speaker.</returns>
        public static System.Guid Speak(string text, AudioSource source = null, Model.Voice voice = null, bool speakImmediately = true, float rate = 1f, float volume = 1f, string outputFile = "", float pitch = 1f)
        {

            Model.Wrapper wrapper = new Model.Wrapper(text, voice, rate, pitch, volume, source, speakImmediately, outputFile);

            //Debug.LogWarning ("Speak called: " + wrapper);

            SpeakWithUID(wrapper);

            return wrapper.Uid;
        }

        /// <summary>Speaks a text with a given voice.</summary>
        /// <param name="wrapper">Speak wrapper.</param>
        public static void SpeakWithUID(Model.Wrapper wrapper)
        {
            //Debug.LogWarning ("SpeakWithUID called: " + wrapper);

            if (Util.Helper.isSupportedPlatform)
            {
                if (wrapper != null)
                {
                    if (voiceProvider != null)
                    {
                        if (string.IsNullOrEmpty(wrapper.Text))
                        {
                            Debug.LogWarning("'Text' is null or empty!");
                        }
                        else
                        {
                            if (speaker != null)
                            {
                                if (!Util.Helper.isIOSPlatform) //special case iOS (no audio file generation possible)
                                {
                                    if (wrapper.Source == null)
                                    {
                                        wrapper.Source = go.AddComponent<AudioSource>();
                                        genericSources.Add(wrapper.Uid, wrapper.Source);

                                        if (string.IsNullOrEmpty(wrapper.OutputFile))
                                        {
                                            wrapper.SpeakImmediately = true; //must always speak immediately (since there is no AudioSource given and no output file wanted)
                                        }
                                    }
                                    else
                                    {
                                        if (!providedSources.ContainsKey(wrapper.Uid))
                                        {
                                            providedSources.Add(wrapper.Uid, wrapper.Source);
                                        }
                                    }
                                }

                                speaker.StartCoroutine(voiceProvider.Speak(wrapper));
                            }
                        }
                    }
                    else
                    {
                        logVPIsNull();
                    }
                }
                else
                {
                    logWrapperIsNull();
                }
            }
            else
            {
                logUnsupportedPlatform();
            }
        }

        /// <summary>Speaks a text with a given wrapper.</summary>
        /// <param name="wrapper">Speak wrapper.</param>
        /// <returns>UID of the speaker.</returns>
        public static System.Guid Speak(Model.Wrapper wrapper)
        {
            if (wrapper != null)
            {
                SpeakWithUID(wrapper);

                return wrapper.Uid;
            }
            else
            {
                logWrapperIsNull();
            }

            return System.Guid.NewGuid(); //fake uid
        }

        /// <summary>Speaks and marks a text with a given wrapper.</summary>
        /// <param name="wrapper">Speak wrapper.</param>
        public static void SpeakMarkedWordsWithUID(Model.Wrapper wrapper)
        {
            if (Util.Helper.isSupportedPlatform)
            {
                if (voiceProvider != null)
                {
                    if (string.IsNullOrEmpty(wrapper.Text))
                    {
                        Debug.LogWarning("The given 'text' is null or empty!");
                    }
                    else
                    {
                        //AudioSource src = source;

                        if (wrapper.Source == null || wrapper.Source.clip == null)
                        {
                            Debug.LogError("'source' must be a valid AudioSource with a clip! Use 'Speak()' before!");
                        }
                        else
                        {
                            wrapper.SpeakImmediately = true;

                            SpeakNativeWithUID(wrapper);

                            if (!Util.Helper.isMacOSPlatform && !Util.Helper.isWSAPlatform && !MaryMode) //prevent "double-speak"
                            {
                                wrapper.Source.Play();
                            }
                        }
                    }
                }
                else
                {
                    logVPIsNull();
                }
            }
            else
            {
                logUnsupportedPlatform();
            }
        }


        /// <summary>Speaks and marks a text with a given voice and tracks the word position.</summary>
        /// <param name="uid">UID of the speaker</param>
        /// <param name="text">Text to speak.</param>
        /// <param name="source">AudioSource for the output.</param>
        /// <param name="voice">Voice to speak (optional).</param>
        /// <param name="rate">Speech rate of the speaker in percent (1 = 100%, default: 1, optional).</param>
        /// <param name="pitch">Pitch of the speech in percent (1 = 100%, default: 1, optional).</param>
        public static void SpeakMarkedWordsWithUID(System.Guid uid, string text, AudioSource source, Model.Voice voice = null, float rate = 1f, float pitch = 1f)
        {
            SpeakMarkedWordsWithUID(new Model.Wrapper(uid, text, voice, rate, pitch, 0));
        }

        //      /// <summary>
        //      /// Speaks a text with a given voice and tracks the word position.
        //      /// </summary>
        //      public static Guid SpeakMarkedWords(string text, AudioSource source = null, Voice voice = null, int rate = 1, int volume = 100) {
        //         Guid result = Guid.NewGuid();
        //
        //         SpeakMarkedWordsWithUID(result, text, source, voice, rate, volume);
        //
        //         return result;
        //      }

        /// <summary>Silence all active TTS-voices.</summary>
        public static void Silence()
        {
            if (Util.Helper.isSupportedPlatform)
            {
                if (voiceProvider != null)
                {
                    if (speaker != null)
                    {
                        speaker.StopAllCoroutines();
                    }

                    voiceProvider.Silence();

                    foreach (System.Collections.Generic.KeyValuePair<System.Guid, AudioSource> source in genericSources)
                    {
                        if (source.Value != null)
                        {
                            source.Value.Stop();
                            //Destroy(source.Value, 0.1f);
                            DestroyImmediate(source.Value);
                        }
                    }
                    genericSources.Clear();

                    foreach (System.Collections.Generic.KeyValuePair<System.Guid, AudioSource> source in providedSources)
                    {
                        if (source.Value != null)
                        {
                            source.Value.Stop();
                        }
                    }
                }
                else
                {
                    providedSources.Clear();

                    logVPIsNull();
                }
            }
            else
            {
                logUnsupportedPlatform();
            }
        }

        /// <summary>Silence an active TTS-voice with a UID.</summary>
        /// <param name="uid">UID of the speaker</param>
        public static void Silence(System.Guid uid)
        {
            if (Util.Helper.isSupportedPlatform)
            {
                if (voiceProvider != null)
                {
                    if (genericSources.ContainsKey(uid))
                    {
                        AudioSource source;

                        if (genericSources.TryGetValue(uid, out source))
                        {
                            source.Stop();
                            genericSources.Remove(uid);
                        }
                    }
                    else if (providedSources.ContainsKey(uid))
                    {
                        AudioSource source;

                        if (providedSources.TryGetValue(uid, out source))
                        {
                            source.Stop();
                            providedSources.Remove(uid);
                        }
                    }
                    else
                    {
                        voiceProvider.Silence(uid);
                    }
                }
                else
                {
                    logVPIsNull();
                }
            }
            else
            {
                logUnsupportedPlatform();
            }
        }

        #endregion


        #region Private methods

        private static void initProvider()
        {
            maryTTS = MaryMode;

            if (Util.Helper.isSupportedPlatform)
            {

                //Debug.Log("initProvider: " + Util.Helper.isWebPlayerPlatform);

                if (MaryMode)
                {
                    if (Util.Helper.isInternetAvailable)
                    {
                        if (MaryURL.Contains("mary.dfki.de"))
                        {
                            if (Util.Helper.isEditor)
                            {
                                Debug.LogWarning("You are using the test server of MaryTTS. Please setup your own server from http://mary.dfki.de");

                                voiceProvider = new Provider.VoiceProviderMary(MaryURL, MaryPort);
                            }
                            else
                            {
#if rtv_demo
                                voiceProvider = new Provider.VoiceProviderMary(MaryURL, MaryPort);
#else
								Debug.LogError("You are using the test server of MaryTTS - this is not allowed in builds! Please setup your own server from http://mary.dfki.de");
								
								maryTTS = MaryMode = false;
								
								initOSProvider();
#endif
                            }

                        }
                        else
                        {
                            voiceProvider = new Provider.VoiceProviderMary(MaryURL, MaryPort);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Internet is not available - can't use MaryTTS and enable OS provider as fallback.");

                        maryTTS = MaryMode = false;

                        initOSProvider();
                    }
                }
                else
                {
                    initOSProvider();
                }

                if (OnProviderChange != null)
                {
                    OnProviderChange(voiceProvider.GetType().ToString());
                }
            }
            else
            {
                logUnsupportedPlatform();
            }
        }

        private static void initOSProvider()
        {
            if (Util.Helper.isWindowsPlatform)
            {
                voiceProvider = new Provider.VoiceProviderWindows();
            }
            else if (Util.Helper.isAndroidPlatform)
            {
                voiceProvider = new Provider.VoiceProviderAndroid();
            }
            else if (Util.Helper.isIOSPlatform)
            {
                voiceProvider = new Provider.VoiceProviderIOS();
            }
            else if (Util.Helper.isWSAPlatform)
            {
                voiceProvider = new Provider.VoiceProviderWSA();
            }
            else
            { // always add a default provider
                voiceProvider = new Provider.VoiceProviderMacOS();
            }
        }

        private static void logWrapperIsNull()
        {
            //if (!loggedWrapperIsNull) {
            string errorMessage = "'wrapper' is null!";

            if (OnErrorInfo != null)
            {
                OnErrorInfo(errorMessage);
            }

            Debug.LogError(errorMessage);

            //}
        }

        private static void logUnsupportedPlatform()
        {
            string errorMessage = "RTVoice is not supported on your platform!";

            if (OnErrorInfo != null)
            {
                OnErrorInfo(errorMessage);
            }

            if (!loggedUnsupportedPlatform)
            {
                Debug.LogError(errorMessage);
                loggedUnsupportedPlatform = true;
            }
        }

        private static void logVPIsNull()
        {
            string errorMessage = "'voiceProvider' is null!" + System.Environment.NewLine + "Did you add the 'RTVoice'-prefab to the current scene?";

            if (OnErrorInfo != null)
            {
                OnErrorInfo(errorMessage);
            }

            if (!loggedVPIsNull)
            {
                Debug.LogWarning(errorMessage);
                loggedVPIsNull = true;
            }
        }

        private void onSpeakNativeCurrentWord(Model.Event.CurrentWordEventArgs e)
        {
            if (OnSpeakNativeCurrentWord != null)
            {
                OnSpeakNativeCurrentWord(e);
            }
        }

        private void onSpeakNativeCurrentPhoneme(Model.Event.CurrentPhonemeEventArgs e)
        {
            if (OnSpeakNativeCurrentPhoneme != null)
            {
                OnSpeakNativeCurrentPhoneme(e);
            }
        }

        private void onSpeakNativeCurrentViseme(Model.Event.CurrentVisemeEventArgs e)
        {
            if (OnSpeakNativeCurrentViseme != null)
            {
                OnSpeakNativeCurrentViseme(e);
            }
        }

        private void onSpeakStart(Model.Event.SpeakEventArgs e)
        {
            if (OnSpeakStart != null)
            {
                OnSpeakStart(e);
            }
        }

        private void onSpeakComplete(Model.Event.SpeakEventArgs e)
        {
            if (OnSpeakComplete != null)
            {
                OnSpeakComplete(e);
            }
        }

        private void onSpeakAudioGenerationStart(Model.Event.SpeakEventArgs e)
        {
            if (OnSpeakAudioGenerationStart != null)
            {
                OnSpeakAudioGenerationStart(e);
            }
        }

        private void onSpeakAudioGenerationComplete(Model.Event.SpeakEventArgs e)
        {
            if (OnSpeakAudioGenerationComplete != null)
            {
                OnSpeakAudioGenerationComplete(e);
            }
        }

        private void onErrorInfo(string errorInfo)
        {
            if (OnErrorInfo != null)
            {
                OnErrorInfo(errorInfo);
            }
        }

        #endregion


        #region Editor-only methods

#if UNITY_EDITOR

        /// <summary>Speaks a text with a given voice (native mode & Editor only).</summary>
        /// <param name="text">Text to speak.</param>
        /// <param name="voice">Voice to speak (optional).</param>
        /// <param name="rate">Speech rate of the speaker in percent (1 = 100%, default: 1, optional).</param>
        /// <param name="volume">Volume of the speaker in percent (1 = 100%, default: 1, optional).</param>
        /// <param name="pitch">Pitch of the speech (1 = 100%, default: 1, optional).</param>
        public static void SpeakNativeInEditor(string text, Model.Voice voice = null, float rate = 1f, float volume = 1f, float pitch = 1f)
        {
            if (Util.Helper.isEditorMode)
            {
                if (voiceProvider != null)
                {
                    Model.Wrapper wrapper = new Model.Wrapper(text, voice, rate, pitch, volume);

                    if (string.IsNullOrEmpty(wrapper.Text))
                    {
                        Debug.LogWarning("'Text' is null or empty!");
                    }
                    else
                    {
                        System.Threading.Thread worker = new System.Threading.Thread(() => voiceProvider.SpeakNativeInEditor(wrapper));
                        worker.Start();
                    }
                }
                else
                {
                    logVPIsNull();
                }
            }
            else
            {
                Debug.LogWarning("'SpeakNativeInEditor()' works only inside the Unity Editor!");
            }
        }

        /// <summary>Generates audio for a text with a given voice (Editor only).</summary>
        /// <param name="text">Text to speak.</param>
        /// <param name="voice">Voice to speak (optional).</param>
        /// <param name="rate">Speech rate of the speaker in percent (1 = 100%, default: 1, optional).</param>
        /// <param name="volume">Volume of the speaker in percent (1 = 100%, default: 1, optional).</param>///
        /// <param name="outputFile">Saves the generated audio to an output file (without extension, optional).</param>
        /// <param name="pitch">Pitch of the speech (1 = 100%, default: 1, optional).</param>
        public static void GenerateInEditor(string text, Model.Voice voice = null, float rate = 1f, float volume = 1f, string outputFile = "", float pitch = 1f)
        {
            if (Util.Helper.isEditorMode)
            {
                if (voiceProvider != null)
                {
                    Model.Wrapper wrapper = new Model.Wrapper(text, voice, rate, pitch, volume, null, true, outputFile);

                    if (string.IsNullOrEmpty(wrapper.Text))
                    {
                        Debug.LogWarning("'Text' is null or empty!");
                    }
                    else
                    {
                        System.Threading.Thread worker = new System.Threading.Thread(() => voiceProvider.GenerateInEditor(wrapper));
                        worker.Start();
                    }
                }
                else
                {
                    logVPIsNull();
                }
            }
            else
            {
                Debug.LogWarning("'GenerateInEditor()' works only inside the Unity Editor!");
            }
        }

#endif

        #endregion
    }
}
// © 2015-2017 crosstales LLC (https://www.crosstales.com)