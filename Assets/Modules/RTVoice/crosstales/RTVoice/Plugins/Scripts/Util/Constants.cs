namespace Crosstales.RTVoice.Util
{
    /// <summary>Collected constants of very general utility for the asset.</summary>
    public static class Constants
    {

        #region Constant variables

        /// <summary>Is PRO-version?</summary>
        public static readonly bool isPro = true;

        /// <summary>Name of the asset.</summary>
        public const string ASSET_NAME = "RTVoice PRO"; //PRO
        //public const string ASSET_NAME = "RTVoice"; //DLL

        /// <summary>Version of the asset.</summary>
        public const string ASSET_VERSION = "2.7.1";

        /// <summary>Build number of the asset.</summary>
        public const int ASSET_BUILD = 271;

        /// <summary>Create date of the asset (YYYY, MM, DD).</summary>
        public static readonly System.DateTime ASSET_CREATED = new System.DateTime(2015, 4, 29);

        /// <summary>Change date of the asset (YYYY, MM, DD).</summary>
        public static readonly System.DateTime ASSET_CHANGED = new System.DateTime(2017, 4, 9);

        /// <summary>Author of the asset.</summary>
        public const string ASSET_AUTHOR = "crosstales LLC";

        /// <summary>URL of the asset author.</summary>
        public const string ASSET_AUTHOR_URL = "https://www.crosstales.com";

        /// <summary>URL of the crosstales assets in UAS.</summary>
        public const string ASSET_CT_URL = "https://www.assetstore.unity3d.com/#!/list/42213-crosstales?aid=1011lNGT"; // crosstales list

        /// <summary>URL of the PRO asset in UAS.</summary>
        public const string ASSET_PRO_URL = "https://www.assetstore.unity3d.com/#!/content/41068?aid=1011lNGT";

        /// <summary>URL of the 3rd party assets in UAS.</summary>
        public const string ASSET_3P_URL = "https://www.assetstore.unity3d.com/en/#!/list/42209-rt-voice-friends?aid=1011lNGT"; // RTV&Friends list

        /// <summary>URL for update-checks of the asset</summary>
        public const string ASSET_UPDATE_CHECK_URL = "https://www.crosstales.com/media/assets/rtvoice_versions.txt";

        /// <summary>Contact to the owner of the asset.</summary>
        public const string ASSET_CONTACT = "rtvoice@crosstales.com";

        /// <summary>URL of the asset manual.</summary>
        public const string ASSET_MANUAL_URL = "https://www.crosstales.com/media/data/assets/rtvoice/RTVoice-doc.pdf";

        /// <summary>URL of the asset API.</summary>
		public const string ASSET_API_URL = "http://goo.gl/6w4Fy0"; // checked: 08.03.2017
        //public const string ASSET_API_URL = "http://www.crosstales.com/en/assets/rtvoice/api/";

        /// <summary>URL of the asset forum.</summary>
		public const string ASSET_FORUM_URL = "http://goo.gl/Z6MZMl"; // checked: 08.03.2017
        //public const string ASSET_FORUM_URL = "http://forum.unity3d.com/threads/rt-voice-run-time-text-to-speech-solution.340046/";

        /// <summary>URL of the asset in crosstales.</summary>
        public const string ASSET_WEB_URL = "https://www.crosstales.com/en/portfolio/rtvoice/";

        /// <summary>URL of the 3rd party asset "PlayMaker".</summary>
        public const string ASSET_3P_PLAYMAKER = "https://www.assetstore.unity3d.com/en/#!/content/368?aid=1011lNGT";

        /// <summary>URL of the 3rd party asset "Adventure Creator".</summary>
        public const string ASSET_3P_ADVENTURE_CREATOR = "https://www.assetstore.unity3d.com/en/#!/content/11896?aid=1011lNGT";

        /// <summary>URL of the 3rd party asset "Cinema Director".</summary>
        public const string ASSET_3P_CINEMA_DIRECTOR = "https://www.assetstore.unity3d.com/en/#!/content/19779?aid=1011lNGT";

        /// <summary>URL of the 3rd party asset "Dialogue System".</summary>
        public const string ASSET_3P_DIALOG_SYSTEM = "https://www.assetstore.unity3d.com/en/#!/content/11672?aid=1011lNGT";

        /// <summary>URL of the 3rd party asset "Localized Dialogs".</summary>
        public const string ASSET_3P_LOCALIZED_DIALOGS = "https://www.assetstore.unity3d.com/en/#!/content/5020?aid=1011lNGT";

        /// <summary>URL of the 3rd party asset "LipSync Pro".</summary>
        public const string ASSET_3P_LIPSYNC = "https://www.assetstore.unity3d.com/en/#!/content/32117?aid=1011lNGT";

        /// <summary>URL of the 3rd party asset "NPC Chat".</summary>
        public const string ASSET_3P_NPC_CHAT = "https://www.assetstore.unity3d.com/en/#!/content/9723?aid=1011lNGT";

        /// <summary>URL of the 3rd party asset "Quest System Pro".</summary>
        public const string ASSET_3P_QUEST_SYSTEM = "https://www.assetstore.unity3d.com/en/#!/content/63460?aid=1011lNGT";

        /// <summary>URL of the 3rd party asset "SALSA".</summary>
        public const string ASSET_3P_SALSA = "https://www.assetstore.unity3d.com/en/#!/content/16944?aid=1011lNGT";

        /// <summary>URL of the 3rd party asset "SLATE".</summary>
        public const string ASSET_3P_SLATE = "https://www.assetstore.unity3d.com/en/#!/content/56558?aid=1011lNGT";

        /// <summary>URL of the 3rd party asset "THE Dialogue Engine".</summary>
        public const string ASSET_3P_DIALOGUE_ENGINE = "https://www.assetstore.unity3d.com/en/#!/content/42467?aid=1011lNGT";

        /// <summary>URL of the 3rd party asset "uSequencer".</summary>
        public const string ASSET_3P_USEQUENCER = "https://www.assetstore.unity3d.com/en/#!/content/3666?aid=1011lNGT";

        /// <summary>Name of the RT-Voice scene object.</summary>
        public const string RTVOICE_SCENE_OBJECT_NAME = "RTVoice";

        // Keys for the configuration of the asset
        private const string KEY_PREFIX = "RTVOICE_CFG_";
        public const string KEY_ASSET_PATH = KEY_PREFIX + "ASSET_PATH";
        public const string KEY_DEBUG = KEY_PREFIX + "DEBUG";
        public const string KEY_UPDATE_CHECK = KEY_PREFIX + "UPDATE_CHECK";
        public const string KEY_UPDATE_OPEN_UAS = KEY_PREFIX + "UPDATE_OPEN_UAS";
        //public const string KEY_DONT_DESTROY_ON_LOAD = KEY_PREFIX + "DONT_DESTROY_ON_LOAD";
        public const string KEY_PREFAB_AUTOLOAD = KEY_PREFIX + "PREFAB_AUTOLOAD";
        public const string KEY_AUDIOFILE_PATH = KEY_PREFIX + "AUDIOFILE_PATH";
        public const string KEY_AUDIOFILE_AUTOMATIC_DELETE = KEY_PREFIX + "AUDIOFILE_AUTOMATIC_DELETE";
        public const string KEY_HIERARCHY_ICON = KEY_PREFIX + "HIERARCHY_ICON";
        public const string KEY_ENFORCE_32BIT_WINDOWS = KEY_PREFIX + "ENFORCE_32BIT_WINDOWS";
        //public const string KEY_TTS_MACOS = KEY_PREFIX + "TTS_MACOS";
        public const string KEY_UPDATE_DATE = KEY_PREFIX + "UPDATE_DATE";

        // Default values
        public const string DEFAULT_ASSET_PATH = "/crosstales/RTVoice/";
        public const bool DEFAULT_DEBUG = false;
        public const bool DEFAULT_UPDATE_CHECK = true;
        public const bool DEFAULT_UPDATE_OPEN_UAS = false;
        public const bool DEFAULT_DONT_DESTROY_ON_LOAD = true;
        public const bool DEFAULT_PREFAB_AUTOLOAD = false;

#if UNITY_WSA || UNITY_WSA_10_0 || UNITY_WEBPLAYER || UNITY_WEBGL
        public static readonly string DEFAULT_AUDIOFILE_PATH = string.Empty;
#else
        public static readonly string DEFAULT_AUDIOFILE_PATH = System.IO.Path.GetTempPath();
#endif
        public const bool DEFAULT_AUDIOFILE_AUTOMATIC_DELETE = true;

        public const bool DEFAULT_HIERARCHY_ICON = true;

        public const bool DEFAULT_ENFORCE_32BIT_WINDOWS = false;
        public const string DEFAULT_TTS_WINDOWS_BUILD = @"/RTVoiceTTSWrapper.exe"; //TODO PlayerPrefs?
        public const string DEFAULT_TTS_MACOS = "say";
        public const int DEFAULT_TTS_KILL_TIME = 5000; //TODO PlayerPrefs?

        #endregion


        #region Changable variables

        /// <summary>Path to the asset inside the Unity project.</summary>
        public static string ASSET_PATH = DEFAULT_ASSET_PATH;

        /// <summary>Enable or disable debug logging for the asset.</summary>
        public static bool DEBUG = DEFAULT_DEBUG;

        /// <summaryEnable or disable update-checks for the asset.</summary>
        public static bool UPDATE_CHECK = DEFAULT_UPDATE_CHECK;

        /// <summaryOpen the UAS-site when an update is found.</summary>
        public static bool UPDATE_OPEN_UAS = DEFAULT_UPDATE_OPEN_UAS;

        /// <summary>Don't destroy RTVoice during scene switches.</summary>
        public static bool DONT_DESTROY_ON_LOAD = DEFAULT_DONT_DESTROY_ON_LOAD;

        /// <summary>Automatically load and add the prefabs to the scene.</summary>
        public static bool PREFAB_AUTOLOAD = DEFAULT_PREFAB_AUTOLOAD;

        /// <summary>Path to the generated audio files.</summary>
        public static string AUDIOFILE_PATH = DEFAULT_AUDIOFILE_PATH;

        /// <summary>Automatically delete the generated audio files.</summary>
        public static bool AUDIOFILE_AUTOMATIC_DELETE = DEFAULT_AUDIOFILE_AUTOMATIC_DELETE;

        /// <summary>Enable or disable the icon in the hierarchy.</summary>
        public static bool HIERARCHY_ICON = DEFAULT_HIERARCHY_ICON;

        /// <summary>Enforce 32bit versions of voices under Windows.</summary>
        public static bool ENFORCE_32BIT_WINDOWS = DEFAULT_ENFORCE_32BIT_WINDOWS;


        // Technical settings
        /// <summary>Location of the TTS-wrapper under Windows (stand-alone).</summary>
        public static string TTS_WINDOWS_BUILD = DEFAULT_TTS_WINDOWS_BUILD;

        /// <summary>Location of the TTS-system under MacOS.</summary>
        public static string TTS_MACOS = DEFAULT_TTS_MACOS;

        /// <summary>Kill processes after 5000 milliseconds.</summary>
        public static int TTS_KILL_TIME = DEFAULT_TTS_KILL_TIME;

        /// <summary>Sub-path to the prefabs.</summary>
        public static string PREFAB_SUBPATH = "Prefabs/";

        /// <summary>Sub-path to the TTS-wrapper under Windows (Editor).</summary>
        public static string TTS_WINDOWS_SUBPATH = "Plugins/Windows/RTVoiceTTSWrapper.exe";

        /// <summary>Sub-path to the TTS-wrapper (32bit) under Windows (Editor).</summary>
        public static string TTS_WINDOWS_x86_SUBPATH = "Plugins/Windows/RTVoiceTTSWrapper_x86.exe";

        // Text fragments for the asset
        public static string TEXT_TOSTRING_START = " {";
        public static string TEXT_TOSTRING_END = "}";
        public static string TEXT_TOSTRING_DELIMITER = "', ";
        public static string TEXT_TOSTRING_DELIMITER_END = "'";

        #endregion


        #region Properties

        /// <summary>URL of the asset in UAS.</summary>
        public static string ASSET_URL
        {
            get
            {

                if (isPro)
                {
                    return ASSET_PRO_URL;
                }
                else
                {
                    return "https://www.assetstore.unity3d.com/#!/content/48394?aid=1011lNGT";
                }
            }
        }

        /// <summary>UID of the asset.</summary>
        public static System.Guid ASSET_UID
        {
            get
            {
                if (isPro)
                {
                    return new System.Guid("181f4dab-261f-4746-85f8-849c2866d353");
                }
                else
                {
                    return new System.Guid("1ffe8fd4-4e17-497b-9df7-7cd9200d0941");
                }
            }
        }

        /// <summary>Path of the prefabs.</summary>
        public static string PREFAB_PATH
        {
            get
            {
                return ASSET_PATH + PREFAB_SUBPATH;
            }
        }

        /// <summary>Location of the TTS-wrapper under Windows (Editor).</summary>
        public static string TTS_WINDOWS_EDITOR
        {
            get
            {
                return ASSET_PATH + TTS_WINDOWS_SUBPATH;
            }
        }

        /// <summary>Location of the TTS-wrapper (32bit) under Windows (Editor).</summary>
        public static string TTS_WINDOWS_EDITOR_x86
        {
            get
            {
                return ASSET_PATH + TTS_WINDOWS_x86_SUBPATH;
            }
        }

        #endregion


        #region Public static methods

        /// <summary>Resets all changable variables to their default value.</summary>
        public static void Reset()
        {
            ASSET_PATH = DEFAULT_ASSET_PATH;
            DEBUG = DEFAULT_DEBUG;
            UPDATE_CHECK = DEFAULT_UPDATE_CHECK;
            UPDATE_OPEN_UAS = DEFAULT_UPDATE_OPEN_UAS;
            DONT_DESTROY_ON_LOAD = DEFAULT_DONT_DESTROY_ON_LOAD;
            PREFAB_AUTOLOAD = DEFAULT_PREFAB_AUTOLOAD;
            AUDIOFILE_PATH = DEFAULT_AUDIOFILE_PATH;
            AUDIOFILE_AUTOMATIC_DELETE = DEFAULT_AUDIOFILE_AUTOMATIC_DELETE;
            HIERARCHY_ICON = DEFAULT_HIERARCHY_ICON;
            ENFORCE_32BIT_WINDOWS = DEFAULT_ENFORCE_32BIT_WINDOWS;
            TTS_WINDOWS_BUILD = DEFAULT_TTS_WINDOWS_BUILD;
            TTS_MACOS = DEFAULT_TTS_MACOS;
            TTS_KILL_TIME = DEFAULT_TTS_KILL_TIME;
        }

        /// <summary>Loads all changable variables.</summary>
        public static void Load()
        {
            if (CTPlayerPrefs.HasKey(KEY_DEBUG))
            {
                DEBUG = CTPlayerPrefs.GetBool(KEY_DEBUG);
            }

            if (CTPlayerPrefs.HasKey(KEY_UPDATE_CHECK))
            {
                UPDATE_CHECK = CTPlayerPrefs.GetBool(KEY_UPDATE_CHECK);
            }

            if (CTPlayerPrefs.HasKey(KEY_UPDATE_OPEN_UAS))
            {
                UPDATE_OPEN_UAS = CTPlayerPrefs.GetBool(KEY_UPDATE_OPEN_UAS);
            }

            //if (CTPlayerPrefs.HasKey(KEY_DONT_DESTROY_ON_LOAD))
            //{
            //    DONT_DESTROY_ON_LOAD = CTPlayerPrefs.GetBool(KEY_DONT_DESTROY_ON_LOAD);
            //}

            if (CTPlayerPrefs.HasKey(KEY_ASSET_PATH))
            {
                ASSET_PATH = CTPlayerPrefs.GetString(KEY_ASSET_PATH);
            }

            if (CTPlayerPrefs.HasKey(KEY_PREFAB_AUTOLOAD))
            {
                PREFAB_AUTOLOAD = CTPlayerPrefs.GetBool(KEY_PREFAB_AUTOLOAD);
            }

            if (CTPlayerPrefs.HasKey(KEY_AUDIOFILE_PATH))
            {
                AUDIOFILE_PATH = CTPlayerPrefs.GetString(KEY_AUDIOFILE_PATH);
            }

            if (CTPlayerPrefs.HasKey(KEY_AUDIOFILE_AUTOMATIC_DELETE))
            {
                AUDIOFILE_AUTOMATIC_DELETE = CTPlayerPrefs.GetBool(KEY_AUDIOFILE_AUTOMATIC_DELETE);
            }

            if (CTPlayerPrefs.HasKey(KEY_HIERARCHY_ICON))
            {
                HIERARCHY_ICON = CTPlayerPrefs.GetBool(KEY_HIERARCHY_ICON);
            }

            if (CTPlayerPrefs.HasKey(KEY_ENFORCE_32BIT_WINDOWS))
            {
                ENFORCE_32BIT_WINDOWS = CTPlayerPrefs.GetBool(KEY_ENFORCE_32BIT_WINDOWS);
            }

            //if (CTPlayerPrefs.HasKey(KEY_TTS_MACOS))
            //{
            //    TTS_MACOS = CTPlayerPrefs.GetString(KEY_TTS_MACOS);
            //}
        }

        /// <summary>Saves all changable variables.</summary>
        public static void Save()
        {
            CTPlayerPrefs.SetString(KEY_ASSET_PATH, ASSET_PATH);
            CTPlayerPrefs.SetBool(KEY_DEBUG, DEBUG);
            CTPlayerPrefs.SetBool(KEY_UPDATE_CHECK, UPDATE_CHECK);
            CTPlayerPrefs.SetBool(KEY_UPDATE_OPEN_UAS, UPDATE_OPEN_UAS);
            //CTPlayerPrefs.SetBool(KEY_DONT_DESTROY_ON_LOAD, DONT_DESTROY_ON_LOAD);
            CTPlayerPrefs.SetBool(KEY_PREFAB_AUTOLOAD, PREFAB_AUTOLOAD);
            CTPlayerPrefs.SetString(KEY_AUDIOFILE_PATH, AUDIOFILE_PATH);
            CTPlayerPrefs.SetBool(KEY_AUDIOFILE_AUTOMATIC_DELETE, AUDIOFILE_AUTOMATIC_DELETE);
            CTPlayerPrefs.SetBool(KEY_HIERARCHY_ICON, HIERARCHY_ICON);
            CTPlayerPrefs.SetBool(KEY_ENFORCE_32BIT_WINDOWS, ENFORCE_32BIT_WINDOWS);
            //CTPlayerPrefs.SetString(KEY_TTS_MACOS, TTS_MACOS);

            CTPlayerPrefs.Save();
        }

        #endregion
    }
}
// © 2015-2017 crosstales LLC (https://www.crosstales.com)