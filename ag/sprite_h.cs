using SDL2;

namespace ag
{
    partial class Program
    {

        /// <value> Property <c>LETTER_FAST</c> letter sprite fast speed </value>
        public const int LETTER_FAST = 30;
        /// <value> Property <c>LETTER_SLOW</c> letter sprite slow speed </value>
        public const int LETTER_SLOW = 10;
        /// <value> Property <c>GAME_LETTER_HEIGHT</c> is the width of the graphic of a letter in the "band" image file</value>
        public const int GAME_LETTER_WIDTH = 80;
        /// <value> Property <c>GAME_LETTER_HEIGHT</c> height of the graphic of a letter in the "band" image file</value>
        public const int GAME_LETTER_HEIGHT = 90;
        /// <value> Property <c>GAME_LETTER_SPACE</c> separation between the graphics of two letters in the SHUFFLE box</value>
        public const int GAME_LETTER_SPACE = 2;
        /// <value> Property <c>BOX_START_X</c> X position of the SHUFFLE and ANSWERS box</value>
        public const int BOX_START_X = 80;
        /// <value> Property <c>SHUFFLE_BOX_Y</c> Y Position from the top of the window of the SHUFFLE box</value>
        public const int SHUFFLE_BOX_Y = 107;
        /// <value> Property <c>ANSWER_BOX_Y</c> Y Position from the top of the window of the ANSWER box</value>
        public const int ANSWER_BOX_Y = 247;

        /// <summary>
        /// Structure to identify a letter graphic in the "band" image containing 
        /// all the letters
        /// </summary>
        public struct Element
        {
            /// <value> Property <c>t</c>texture of the sprite</value>
            /// <remarks>(SDL_Texture *t) For some reason SDL2.SDL.SDL_Texture has not been defined in for c#, so using IntPtr</remarks>
            public IntPtr sprite_band_texture;
            /// <value>SDL_rect dimentions of the sprite</value>
            public SDL.SDL_Rect sprite_band_dimensions;
            /// <value>Sprite graphic offset from the top left position of the band image</value>
            public int sprite_x_offset, sprite_y_offset;
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
            /// <value> Property <c>spr</c>The graphical representation</value>
            public Element[] sprite { get; set; }
            /// <value>The number of spr elements in this Sprite</value>
            public int numSpr;
            /// <value> Property <c>letter</c> the actual letter value </value>
            public char letter;
            /// <value> Property <c>x, y, w, h</c> the offset position from top left to use for in the band image and width and height </value>
            public int x, y, w, h;
            /// <value> Property <c>toX, toY</c> the position in the SHUFFLE or ANSWER box where the letter should be placed </value>
            public int toX, toY;
            /// <value> Property <c>next</c> **??** </value>
            public Sprite? next;
            /// <value> Property <c>index</c> the index of this letter (?) </value>
            public int index;
            /// <value> Property <c>box</c> the box in which this letter should be placed </value>
            public int box;
            
            /// <summary> constructor for Sprite
            /// </summary>
            /// <param name="numOfElements">The number of sprite elements</param>
            public Sprite (int numOfElements)
            {
                sprite = new Element[numOfElements];
                // spr[0] = null;
                numSpr = 0;
                letter = '\0';
                x = y = w = h = 0;
                toX = toY = 0;
                next = null;
                index = 0;
                box = 0;
            }
        }

    }
}