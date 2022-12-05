namespace Crosstales.RTVoice.Model.Event
{
    /// <summary>EventArgs for the current viseme.</summary>
    public class CurrentVisemeEventArgs : SpeakEventArgs
    {
        /// <summary>Current viseme.</summary>
        public string Viseme;

        public CurrentVisemeEventArgs(Wrapper wrapper, string viseme) : base(wrapper)
        {
            Viseme = viseme;
        }
    }
}
// © 2016-2017 crosstales LLC (https://www.crosstales.com)