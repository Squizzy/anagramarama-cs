using SDL2;

namespace ag
{
    partial class Program
    {
        public struct Element
        {
            IntPtr t;  // (SDL_Texture *t) For some reason SDL2.SDL.SDL_Texture has not been defined in for c#!
            SDL.SDL_Rect w;
            int x, y;
        }
        public class Sprite
        {
            public Element? spr;
            public int numSpr;
            char letter;
            int x, y, w, h;
            public Sprite next;
            int index;
            int box;

        }

        public static void destroyLetters(Sprite? letters)
        {
             Sprite? current = letters;
            while (current != null) {
                Sprite? tmp = current;
                if (current.numSpr > 0)
                    current.spr = null;
                current = current.next;
                tmp = null;
            }
	        letters = null;
        }

    }
}
