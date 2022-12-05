namespace Crosstales.RTVoice.Model.Event
{
    /// <summary>EventArgs for the current word.</summary>
    public class CurrentWordEventArgs : SpeakEventArgs
    {
        /// <summary>Array with the text splitted into words.</summary>
        public string[] SpeechTextArray;

        /// <summary>Current word index.</summary>
        public int WordIndex;

        public CurrentWordEventArgs(Wrapper wrapper, string[] speechTextArray, int wordIndex) : base(wrapper)
        {
            SpeechTextArray = speechTextArray;
            WordIndex = wordIndex;
        }
    }
}
// © 2016-2017 crosstales LLC (https://www.crosstales.com)