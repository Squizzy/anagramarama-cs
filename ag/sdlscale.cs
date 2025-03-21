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
        public static void SDLScale_RenderCopy(IntPtr renderer, IntPtr texture, SDL.SDL_Rect? srcRect, SDL.SDL_Rect? dstRect)
        {

            if (dstRect.HasValue)
            {
                SDL.SDL_Rect dstReal;
                SDL.SDL_Rect dstRectToSend = (SDL.SDL_Rect)dstRect;

                dstReal.x = (int)(dstRectToSend.x * scalew);
                dstReal.y = (int)(dstRectToSend.y * scaleh);
                dstReal.h = (int)(dstRectToSend.h * scaleh);
                dstReal.w = (int)(dstRectToSend.w * scalew);

                if (srcRect.HasValue)
                {
                    SDL.SDL_Rect srcRectToSend;
                    srcRectToSend = (SDL.SDL_Rect)srcRect;
                    int sdlRtn  = SDL.SDL_RenderCopy(renderer, texture, ref srcRectToSend, ref dstReal);
                    if (sdlRtn != 0)
                    {
                        Console.WriteLine("Problem with RenderCopy in SDLScale_RenderCopy");
                        Console.ReadLine();
                    }
                }
                else
                {
                    int sdlRtn  = SDL.SDL_RenderCopy(renderer, texture, (nint)null, ref dstReal);
                    if (sdlRtn != 0)
                    {
                        Console.WriteLine("Problem with RenderCopy in SDLScale_RenderCopy");
                        Console.ReadLine();
                    }
                }
            }

            else
            {

                if (srcRect.HasValue)
                {
                    SDL.SDL_Rect srcRectToSend;
                    srcRectToSend = (SDL.SDL_Rect)srcRect;
                    int sdlRtn  = SDL.SDL_RenderCopy(renderer, texture, ref srcRectToSend, (nint)null);
                    if (sdlRtn != 0)
                    {
                        Console.WriteLine("Problem with RenderCopy in SDLScale_RenderCopy");
                        Console.ReadLine();
                    }
                }
                else
                {
                    int sdlRtn  = SDL.SDL_RenderCopy(renderer, texture, (nint)null, (nint)null);
                    if (sdlRtn != 0)
                    {
                        Console.WriteLine("Problem with RenderCopy in SDLScale_RenderCopy");
                        Console.ReadLine();
                    }
                }
            }



            // SDL.SDL_Rect srcRectToSend;
            // SDL.SDL_Rect dstReal;

            // if (dstRect != null)
            // {
            //     dstReal.x = (int)(dstRect.x * scalew);
            //     dstReal.y = (int)(dstRect.y * scaleh);
            //     dstReal.h = (int)(dstRect.h * scaleh);
            //     dstReal.w = (int)(dstRect.w * scalew);
            // }
            // else
            // {
            //     // nint dstReal = (nint)null;
            //     // dstReal = (nint)null;
            // }



            // if (srcRect != null)
            // {
            //     // new to handle srcRect that can be null
            //     srcRectToSend = (SDL.SDL_Rect)srcRect;
            //     // SDL.SDL_Rect srcRectToSend = (SDL.SDL_Rect)srcRect;
            //     // SDL.SDL_RenderCopy(renderer, texture, ref srcRectToSend, ref dstReal);
            // }
            // else
            // {
            //     // SDL.SDL_RenderCopy(renderer, texture, (nint)null, ref dstReal);
            // }


            // var src = srcRect.HasValue ? srcRectToSend : (nint)null;


            // SDL.SDL_RenderCopy(renderer, texture, srcRect.HasValue ? ref srcRectToSend : (nint)null, dstRect.HasValue ? ref dstReal: IntPtr.Zero);


            // TODO: Handle SDL error check

            // SDL.SDL_Rect srcRectSent = new SDL.SDL_Rect();
            // if (srcRect == null)
            // {
            //     // New - to handle null textures but in c# can't pass a null as ref
            //     int textureWidth, textureHeight;
            //     SDL.SDL_QueryTexture(texture, out _, out _, out textureWidth, out textureHeight);
            //     // srcRectSent.x = 0;
            //     // srcRectSent.y = 0;
            //     // srcRectSent.w = 0;
            //     // srcRectSent.h = 0;
            //     srcRectSent.x = 0;
            //     srcRectSent.y = 0;
            //     srcRectSent.w = textureWidth;
            //     srcRectSent.h = textureHeight;
            // }
            // else
            // {
            //     srcRectSent = (SDL.SDL_Rect)srcRect;
            // }

            // int sdlRtn = SDL.SDL_RenderCopy(renderer, texture, (nint)srcRectSent, ref dstReal);

            // if (sdlRtn != 0)
            // {
            //     Console.WriteLine("Problem with RenderCopy in SDLScale_RenderCopy");
            //     Console.ReadLine();
            // }
            // Console.WriteLine(sdlRtn);


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
