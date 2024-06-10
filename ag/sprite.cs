using SDL2;

namespace ag
{
    partial class Program
    {
        /// <value> Property <c>GAME_LETTER_HEIGHT</c> is the width of the graphic of a letter in the "band" image file</value>
        public static int GAME_LETTER_WIDTH = 80;
        /// <value> Property <c>GAME_LETTER_HEIGHT</c> height of the graphic of a letter in the "band" image file</value>
        public static int GAME_LETTER_HEIGHT = 90;
        /// <value> Property <c>GAME_LETTER_SPACE</c> separation between the graphics of two letters in the SHUFFLE box</value>
        public static int GAME_LETTER_SPACE = 2;
        /// <value> Property <c>BOX_START_X</c> X position of the SHUFFLE and ANSWERS box</value>
        public static int BOX_START_X = 80;
        /// <value> Property <c>SHUFFLE_BOX_Y</c> Y Position from the top of the window of the SHUFFLE box</value>
        public static int SHUFFLE_BOX_Y = 107;
        /// <value> Property <c>ANSWER_BOX_Y</c> Y Position from the top of the window of the ANSWER box</value>
        public static int ANSWER_BOX_Y = 247;

        /// <summary>
        /// Structure to identify a letter graphic in the "band" image containing 
        /// all the letters
        /// </summary>
        public struct Element
        {
            /// <value> Property <c>t</c> basically, refers to the image containing all the letters</value>
            /// <remarks>(SDL_Texture *t) For some reason SDL2.SDL.SDL_Texture has not been defined in for c#, so using IntPtr</remarks>
            public IntPtr t;
            /// <value>Property <c>w</c> represent the displacement horizontally in t from the start position to find the letter</value>
            public SDL.SDL_Rect w;
            
            /// <value>Property <c>x, y</c> represent the start position in t to find the letter</value>
            public int x, y;
        }
        
        /// <summary>
        /// class Sprite is used to position the letters in the SHUFFLE and ANSWER boxes.
        ///     spr:        the graphical representation
        ///     numSpr:     ** ?? **
        ///     letter:     the actual letter value
        ///     x, y:       the offset position to use for in the band image
        ///     h, w:       height and width of the graphical representation of the character in the band image
        ///     toX, toY:   the position in the SHUFFLE or ANSWER box where the letter should be placed
        ///     next:       ** ?? **
        ///     box:        the box in which this letter should be placed
        ///     index:      the index of this letter (?)
        /// </summary>
        public class Sprite
        {
            //public Element[] spr;
            public Element[] spr;
            public int numSpr;
            public char letter;
            public int x, y;
            public int w, h;
            public int toX, toY;
            public Sprite? next;
            public int index;
            public int box;
            public Sprite(int numOfElements)
            {
                Element[] spr = new Element(numOfElements);
                spr[0] = null;
                numSpr = 0;
                x = y = w = h = 0;
                toX = toY = 0;
                next = null;
                index = 0;
                box = 0;
            }
        }


        /// <summary>
        /// This is an attempt to convert the c function from the original anagramarama 0.7
        /// but this is not needed in c#
        /// It is currently not working
        /// </summary>
        /// <param name="letters"></param>
        public static void destroyLetters(ref Sprite? letters)
        {
            Sprite? current = letters;
            while (current != null) {
                Sprite tmp = current;
                if (current.numSpr > 0)
                    current.spr = null;
                current = current.next;
                tmp = null;
            }
	        letters = null;
        }

    }
}
