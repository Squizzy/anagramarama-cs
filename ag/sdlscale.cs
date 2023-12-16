using SDL2;

namespace ag
{
    partial class Program
    {
        static double scalew = 1;
        static double scaleh = 1;
        public void SDLScale_MouseEvent(SDL.SDL_Event mouseEvent)
        {
            mouseEvent.button.x = mouseEvent.button.x / (int)scalew;
            mouseEvent.button.y = mouseEvent.button.y / (int)scaleh;
        }

        public void SDLScale_RenderCopy (IntPtr renderer, ref IntPtr texture, ref SDL.SDL_Rect? srcRect, ref SDL.SDL_Rect? dstRect)
        {
            SDL.SDL_Rect dstReal;
            if (dstRect.HasValue)
            {
                dstReal.x = dstRect.Value.x * (int)scalew;
                dstReal.y = dstRect.Value.y * (int)scaleh;
                dstReal.h = dstRect.Value.h * (int)scaleh;
                dstReal.w = dstRect.Value.w * (int)scalew;
                SDL.SDL_RenderCopy(renderer,  texture, ref srcRect,  ref dstReal);
            }
            else
            {
                dstReal.x = dstRect.Value.x;
                dstReal.y = dstRect.Value.y;
                dstReal.h = dstRect.Value.h;
                dstReal.w = dstRect.Value.w;
                SDL.SDL_RenderCopy(renderer,  texture, ref srcRect, ref dstReal);
            }
        }

        public void SDLScale_set(double w, double h)
        {
            scalew = w;
            scaleh = h;
        }

    }
}
