// Replicating the ag.h file
// Preprocessors defines are replaced with const
// Variable strings are set with get/set for more security controls

using System.Reflection.Metadata;

namespace ag
{

    /// <summary> Represents the main program class containing constants and structures.</summary>
    partial class Program
    {
        // public static IntPtr backgroundTex;

        /// <value> ASCII OFFSET to convert a number to its character equivalent </value>
        public const int NUM_TO_CHAR = 48;

        /// <summary>  Represents a rectangular box with position and size. </summary>
        public struct Box
        {
            /// <value>x-coordinate of the top left of the box.</value>
            public int x;

            /// <value>y-coordinate of the top left of the box.</value>
            public int y;

            /// <value>width of the box.</value>
            public int width;

            /// <value>height of the box.</value>
            public int height;
        }

        /// <summary> Pixel details of boxes </summary>
        /// <value> Shuffle Box Y position </value>
        public const int SHUFFLEBOX = 110;
        /// <value> Answer Box Y position </value>
        public const int ANSWERBOX = 245;
        /// <value> Shuffle and Answer Boxes X position </value>
        public const int BOX_START = 30;
        /// <value> Shuffle and Answer Boxes length </value>
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
        /// <summary> Controls box identifier (Score + Time...)</summary>
        public const int CONTROLS = 3;

        /// <summary> define the clock position and character width </summary>
        /// <value> ClockBox X pixel position </value>
        public const int CLOCK_X = 690;
        /// <value> ClockBox Y pixel position </value>
        public const int CLOCK_Y = 35;
        /// <value> ClockCharacter pixel width </value>
        public const int CLOCK_WIDTH = 18;
        /// <value> ClockCharacter pixel height </value>
        public const int CLOCK_HEIGHT = 32;

        /// <summary> define the clock position and character width </summary>
        /// <value> ScoreBox X pixel position </value>
        public const int SCORE_X = 690;
        /// <value> ScoreBox Y pixel position </value>
        public const int SCORE_Y = 67;
        /// <value> ScoreCharacter pixel width </value>
        public const int SCORE_WIDTH = 18;
        /// <value> ScoreCharacter pixel height </value>
        public const int SCORE_HEIGHT = 32;


        /// <value>Represents the space character used in the program.</value>
        public const char SPACE_CHAR = '#';

        /// <value>Represents the ASCII value for the space character.</value>
        public const int ASCII_SPACE = 32;

        /// <value>Represents a string filled with space characters.</value> 
        public const string SPACE_FILLED_CHARS = "#######";

        /// <value> Represents an array of space characters. </value>
        public static readonly string SPACE_FILLED_STRING = new(SPACE_CHAR, 7);

        /// <value> Sets the time of the game (5 mins default = 300s). </value>
        public const int AVAILABLE_TIME = 300;

        /// <value> Path to the locale dictionary </value>
        static string DEFAULT_LOCALE_PATH = "i18n" + DIR_SEP + "en_GB" + DIR_SEP;
        // public const string DEFAULT_LOCALE_PATH = "i18n/en_GB";

        /// <value> directory separator for the OS used </value>
        static char DIR_SEP = Path.DirectorySeparatorChar;

        /// <value>subdirectory where the audio is stored</value>
        static string audioSubPath = "audio" + DIR_SEP;

        /// <value>subdirectory for internationalised content</value>
        static string i18nPath = "i18n" + DIR_SEP;

        /// <value>subdirectory where the audio is stored</value>
        static string imagesSubPath = "images" + DIR_SEP;

        /// <value>subdirectory where the internationalised content is stored (dict, images)</value>
        static string localePath = "";

        /// <value>the size of the biggest word to be used in the game</value>
        public const int  MAX_ANAGRAM_LENGTH = 7;
    }
}