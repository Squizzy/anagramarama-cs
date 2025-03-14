using SDL2;

namespace ag
{
    partial class Program
    {
        /// <summary> Displays a sprite on the screen </summary>
        /// <param name="screen">The renderer to display on</param>
        /// <param name="movie">The sprite to use</param>
        /// <returns>Nothing</returns>
        public static void ShowSprite(ref IntPtr screen, ref Sprite movie)
        {
            SDL.SDL_Rect rect = new()
            {
                x = movie.x,
                y = movie.y,
                w = movie.w,
                h = movie.h
            };

            for (int i = 0; i < movie.numSpr; i++)
            {
                rect.x = movie.x + movie.spr[i].x_offset;
                rect.y = movie.y + movie.spr[i].y_offset;
                rect.w = movie.spr[i].sprite_dimensions.w;
                rect.h = movie.spr[i].sprite_dimensions.h;
                SDLScale_RenderCopy(screen, movie.spr[i].texture, ref movie.spr[i].sprite_dimensions, ref rect);
            }
        }

        /// <summary> checks if a sprite needs to move </summary>
        /// <param name="sprite">The sprite tested</param>
        /// <returns>true if this sprite needs to move</returns>

        public static bool IsSpriteMoving(Sprite sprite)
        {
            return (sprite.y != sprite.toY) || (sprite.x != sprite.toX);
        }

        /// <summary> checks if any sprite needs to move </summary>
        /// <param name="letters">The sprite tested</param>
        /// <returns>false if a sprite needs to move</returns>
        public static bool AnySpriteMoving(Sprite letters)
        {
            Sprite current;
            for (current = letters; current != null; current = current.next)
            {
                if (IsSpriteMoving(current))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary> Moves a sprite </summary>
        /// <param name="screen">The renderer</param>
        /// <param name="movie">The sprite to move</param>
        /// <param name="letterSpeed">The speed to move the sprite at</param>
        /// <returns>Nothing</returns>
        public static void MoveSprite(ref IntPtr screen, ref Sprite movie, int letterSpeed)
        {
            int Xsteps;

            // move a sprite from its curent location to the new location
            if ((movie.y != movie.toY) || (movie.x != movie.toX))
            {
                int x = movie.toX - movie.x;
                int y = movie.toY - movie.y;

                if (y != 0)
                {
                    if (x < 0) x *= -1;
                    if (y < 0) y *= -1;
                    Xsteps = (x / y) * letterSpeed;
                }
                else
                {
                    Xsteps = letterSpeed;
                }

                for (int i = 0; i < Xsteps; i++)
                {
                    if (movie.x < movie.toX) movie.x++;
                    if (movie.x > movie.toX) movie.x--;
                }

                for (int i = 0; i < letterSpeed; i++)
                {
                    if (movie.y < movie.toY) movie.y++;
                    if (movie.y > movie.toY) movie.y--;
                }
            }
        }

        /// <summary> Animate the moving of the sprites </summary>
        /// <param name="screen">the renderer the sprites move on</param>
        /// <param name="letters">the sprites to move</param>
        /// <param name="letterSpeed">the speed of the move</param>
        /// <returns>Nothing</returns>
        public static void MoveSprites(IntPtr screen, Sprite letters, int letterSpeed)
        {
            Sprite current;

            current = letters;
            while (current != null)
            {
                MoveSprite(ref screen, ref current, letterSpeed);
                current = current.next;
            }

            current = letters;
            while (current != null)
            {
                ShowSprite(ref screen, ref current);
                current = current.next;
            }
            SDL.SDL_RenderPresent(screen);

        }

        /// <summary>
        /// Frees the sprite letters memory
        /// was needed in C but no longer in c# as handled by garbage collector
        /// Kept for the sake of keepting
        /// </summary>
        /// <param name="letters">the set of letters to remove from memory</param>
        /// <returns>Nothing</returns>
        public static void DestroyLetters(ref Sprite? letters)
        {
            letters = null;
        }

    }
}
