// Replicating the ag.h file
// Curiously, pre-processor defines were used to define constants - Here is not going to be done like that as I can't work out why it was done so there.

using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace ag
{


    /// <summary> Represents the main program class containing constants and structures.</summary>
    public partial class Program
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




        // shuffle is an array that can be modified so it needs to have a field that is set and get
        /// <summary>Represents a shuffled array of characters.</summary>
        private static readonly char[] _shuffle = new char[8];

        /// <summary>Gets or sets the shuffled array of characters.</summary>
        public static char[] Shuffle
        {
            get { return _shuffle; }
            set
            {
                if (value != null && value.Length == _shuffle.Length)
                {
                    value.CopyTo(_shuffle, 0);
                }
                else
                {
                    throw new ArgumentException($"Array must be of length {_shuffle.Length}.");
                }
            }
        }

        // answer is an array that can be modified so it needs to have a field that is set and get
        /// <summary>Represents an answer array of characters.</summary>
        public static readonly char[] _answer = new char[8];

        /// <summary>Gets or sets the answer array of characters.</summary>
        public static char[] Answer
        {
            get { return _answer; }
            set
            {
                if (value != null && value.Length == _answer.Length)
                {
                    value.CopyTo(_answer, 0);
                }
                else
                {
                    throw new ArgumentException($"Array must be of length {_answer.Length}.");
                }
            }
        }
    }
}