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
            Element spr;
            int numSpr;
            char letter;
            int x, y, w, h;
            Sprite next;
            int index;
            int box;

        }

    }
}
