namespace Crosstales.RTVoice.Model.Event
{
    /// <summary>EventArgs for the current phoneme.</summary>
    public class CurrentPhonemeEventArgs : SpeakEventArgs
    {
        /// <summary>Current phoneme.</summary>
        public string Phoneme;

        public CurrentPhonemeEventArgs(Wrapper wrapper, string phoneme) : base(wrapper)
        {
            Phoneme = phoneme;
        }
    }
}
// © 2016-2017 crosstales LLC (https://www.crosstales.com)