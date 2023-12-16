using System.ComponentModel;
using System.Runtime.CompilerServices;
using SDL2;

namespace ag
{
    partial class Program
    {
        public static IntPtr backgroundTex;
        public struct Box
        {
            public int x;
            public int y;
            public int width;
            public int height;
        }

        Box[] hotbox = new Box []
        {
            new Box  { x = 612, y =   0, width = 66, height = 30 },  /* BoxSolve */  
            new Box  { x = 686, y =   0, width = 46, height = 30 },  /* BoxNew */    
            new Box  { x = 742, y =   0, width = 58, height = 30 },  /* BoxQuit */   
            new Box  { x = 618, y = 206, width = 66, height = 16 },  /* BoxShuffle */
            new Box  { x = 690, y = 254, width = 40, height = 35 },  /* BoxEnter */  
            new Box  { x = 690, y = 304, width = 40, height = 40 }   /* BoxClear */  
        };

        enum hotBoxes { boxSolve, boxNew, boxQuit, boxShuffle, boxEnter, boxClear };
        string[] boxnames = {"solve", "new", "quit", "shuffle", "enter", "clear"};


        private static void newGame(Node answers, dlb_node dict, IntPtr screen, Sprite letters)
        {
            // letters in the guess box
            char[] guess = new char[9];
            char[] remain = new char[9];
            // happy is true if we have <=77 anagrams and => 6
            bool happy = false;

            SDL.SDL_Rect dest;
            dest.x = 0;
            dest.y = 0;
	        dest.w = 800;
	        dest.h = 600;
            //IntPtr temp = BackgroundText
	        SDLScale_RenderCopy(screen, ref backgroundTex, ref null, ref dest);

            
            // setup the list of anagrams based on rootWord. Original game does not want more than 66 anagrams.
            // TODO: Adjust with a variable?
           /* string[] anagramsList = new string[66];

            string rootWord;

            int anagramsCount = 0;

            while (!happy)
            {
                // First off, go pick a random 7-letter word from the dictionary
                rootWord = GetRandomWord(dict);
                //guess = "".ToCharArray();
                Array.Clear(guess, 0, guess.Length);
                Array.Clear(anagramsList, 0, anagramsList.Length);
                anagramsCount = 0;
                

                // remaining letters in list of letter box is set to all the letters of rootWord.
                remain = rootWord.ToCharArray();

                Ag(anagramsList, dict, guess, remain);
            }*/






        }

    }
}