namespace Crosstales.RTVoice.Model.Event
{
    /// <summary>EventArgs and base class for all speaker events.</summary>
    public class SpeakEventArgs : System.EventArgs
    {
        #region Variables

        /// <summary>Wrapper with "Speak"-function call.</summary>
        public Wrapper Wrapper;

        #endregion

        #region Constructor

        public SpeakEventArgs(Wrapper wrapper)
        {
            Wrapper = wrapper;
        }

        #endregion


        #region Overridden methods

        public override string ToString()
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();

            result.Append(GetType().Name);
            result.Append(Util.Constants.TEXT_TOSTRING_START);

            result.Append("Wrapper='");
            result.Append(Wrapper);
            result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER_END);

            result.Append(Util.Constants.TEXT_TOSTRING_END);

            return result.ToString();
        }

        #endregion
    }
}
// © 2016-2017 crosstales LLC (https://www.crosstales.com)