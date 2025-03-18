using System.Reflection.Metadata;
using SDL2;

namespace ag
{
    partial class Program
    {
        static double scalew = 1;
        static double scaleh = 1;

        /// <summary> identify the location of the mouse event if the window was scaled </summary>
        /// <param name="mouseEvent">the mouse event</param>
        /// <returns>Nothing</returns>
        public static void SDLScale_MouseEvent(ref SDL.SDL_Event mouseEvent)
        {
            mouseEvent.button.x = mouseEvent.button.x / (int)scalew;
            mouseEvent.button.y = mouseEvent.button.y / (int)scaleh;
        }

    
        /// <summary> scales a texture according to the requirements </summary>
        /// <param name="renderer">The renderer on which the scaling happens</param>
        /// <param name="texture">The texture to scale</param>
        /// <param name="srcRect">The original size, if any</param>
        /// <param name="dstRect">The scaled rectangle</param>
        /// <returns>Nothing</returns>
        public static void SDLScale_RenderCopy(IntPtr renderer, IntPtr texture, SDL.SDL_Rect? srcRect, ref SDL.SDL_Rect dstRect)
        {
            SDL.SDL_Rect dstReal;
            // if (dstRect != null)
            // {
            dstReal.x = (int)(dstRect.x * scalew);
            dstReal.y = (int)(dstRect.y * scaleh);
            dstReal.h = (int)(dstRect.h * scaleh);
            dstReal.w = (int)(dstRect.w * scalew);
            // TODO: Handle SDL error check

            SDL.SDL_Rect srcRectSent = new SDL.SDL_Rect();
            if (srcRect == null)
            {
                srcRectSent.x = 0;
                srcRectSent.y = 0;
                srcRectSent.w = 0;
                srcRectSent.h = 0;
            }
            else
            {
                srcRectSent = (SDL.SDL_Rect)srcRect;
            }
            
            SDL.SDL_RenderCopy(renderer, texture, ref srcRectSent, ref dstReal);

        }

        /// <summary> applies the scaling factor in run-time changes </summary>
        /// <param name="w">width factor</param>
        /// <param name="h">height factor</param>
        /// <returns>Nothing</returns>
        public static void SDLScaleSet(double w, double h)
        {
            scalew = w;
            scaleh = h;
        }
    }
}
