// Replicating the ag.h file
// Curiously, pre-processor defines were used to define constants - Here is not going to be done like that as I can't work out why it was done so there.

namespace ag
{



    public partial class Program
    {
        // public static IntPtr backgroundTex;
        public const char SPACE_CHAR = '#';
        public const string SPACE_FILLED_CHARS = "#######";
        public static char[] SPACE_FILLED_STRING = { SPACE_CHAR, SPACE_CHAR, SPACE_CHAR, SPACE_CHAR, SPACE_CHAR, SPACE_CHAR, SPACE_CHAR, '\0' };

        public const int ASCII_SPACE = 32;

        public static char[] shuffle = new char[8];
        public static char[] answer = new char[8];    
        public struct Box
        {
            public int x;
            public int y;
            public int width;
            public int height;
        }
    }
}