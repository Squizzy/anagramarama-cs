// Replicating the ag.h file
// Preprocessors defines are replaced with const
// Variable strings are set with get/set for more security controls

namespace ag
{

    /// <summary> Represents the main program class containing constants and structures.</summary>
    partial class Program
    {
        // public static IntPtr backgroundTex;

        /// <summary> ASCII OFFSET to convert a number to it's character equivalent </summary>
        public const int NUM_TO_CHAR = 48;

        /// <summary>  Represents a rectangular box with position and size. </summary>
        public struct Box
        {
            /// <summary>The x-coordinate of the box.</summary>
            public int x;

            /// <summary>The y-coordinate of the box.</summary>
            public int y;

            /// <summary>The width of the box.</summary>
            public int width;

            /// <summary>The height of the box.</summary>
            public int height;
        }

        /// <summary> Pixel details of boxes </summary>
        /// <summary> Shuffle Box Y location </summary>
        public const int SHUFFLEBOX = 110;
        /// <summary> Answer Box Y location </summary>
        public const int ANSWERBOX = 245;
        /// <summary> Box X location </summary>
        public const int BOX_START = 30;
        /// <summary> Box length </summary>
        public const int BOX_LENGTH = 644;

        /// <summary> Pixel dimensions of letters </summary>
        public const int LETTERSTARTPOS = 644;
        /// <summary> Pixel letters width</summary>
        public const int LETTERWIDTH = 644;
        /// <summary> Pixel letters height </summary>
        public const int LETTERHEIGHT = 644;
        /// <summary> Pixel letters spacing </summary>
        public const int LETTERSPACING = 644;

        /// <summary> Answer box identifier </summary>
        public const int ANSWER = 1;
        /// <summary> Shuffle box identifier </summary>
        public const int SHUFFLE = 2;
        /// <summary> Controls box identifier </summary>
        public const int CONTROLS = 3;

        /// <summary> define the clock position and character width </summary>
        /// <summary> clock X pixel position </summary>
        public const int CLOCK_X = 690;
        /// <summary> clock Y pixel position </summary>
        public const int CLOCK_Y = 35;
        /// <summary> clock pixel width </summary>
        public const int CLOCK_WIDTH = 18;
        /// <summary> clock pixel height </summary>
        public const int CLOCK_HEIGHT = 32;

        /// <summary> define the clock position and character width </summary>
        /// <summary> Score X pixel position </summary>
        public const int SCORE_X = 690;
        /// <summary> Score Y pixel position </summary>
        public const int SCORE_Y = 67;
        /// <summary> Score pixel width </summary>
        public const int SCORE_WIDTH = 18;
        /// <summary> Score pixel height </summary>
        public const int SCORE_HEIGHT = 32;


        /// <summary>Represents the space character used in the program.</summary>
        public const char SPACE_CHAR = '#';

        /// <summary>Represents the ASCII value for the space character.</summary>
        public const int ASCII_SPACE = 32;

        /// <summary>Represents a string filled with space characters.</summary> 
        public const string SPACE_FILLED_CHARS = "#######";

        /// <summary> Represents an array of space characters. </summary>
        public static readonly string SPACE_FILLED_STRING = new(SPACE_CHAR, 7);

        /// <summary> Sets the time of the game (5 mins default = 300s). </summary>
        public const int AVAILABLE_TIME = 300;

        /// <summary> Path to the locale dictionary </summary>
        public const string DEFAULT_LOCALE_PATH = "i18n/en_GB";

    }
}