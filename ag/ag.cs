#define DEBUG

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using SDL2;


namespace ag
{
    public partial class Program
    {
        // public static IntPtr backgroundTex;
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


        public static void newGame(ref Node answers, ref dlb_node dict, IntPtr backgroundTex, IntPtr screen, Sprite letters, char[] rootWord, string wordsListPath = "")
        {
            // letters in the guess box
            char[] guess = new char[9];
            char[] remain = new char[9];
            // happy is true if we have <=77 anagrams and => 6
            bool happy = false;
            int answerSought=0;
            int bigWordLen = 0;

            SDL.SDL_Rect dest;
            dest.x = 0;
            dest.y = 0;
	        dest.w = 800;
	        dest.h = 600;
            //IntPtr temp = BackgroundText

            SDL.SDL_Rect firstrect;
            firstrect.x = 0;
            firstrect.y = 0;
            firstrect.w = 800;
            firstrect.h = 600;
            
	        SDLScale_RenderCopy(screen, backgroundTex, ref firstrect, ref dest);
            
            //destroyLetters(letters);
Console.WriteLine("About to look for Happy");
            while (!happy)
            {
                char[] buffer = new char[9];
                buffer = GetRandomWord(wordsListPath).ToCharArray();
                //guess = "".ToCharArray();
                rootWord = buffer;
                bigWordLen = rootWord.Length-1;

                for (int i =0; i<rootWord.Length; i++) remain[i] = rootWord[i];
                //remain =rootWord;

                //destroyAnswers(answers);

                answerSought = Length(answers);
                string newGuessString = new string(guess).Trim('\0');
                string newRemainString = new string(remain).Trim('\0');
                Ag(ref answers, dict, newGuessString, newRemainString);
                guess = newGuessString.ToCharArray();
                char[] rem = newRemainString.ToCharArray();
                for (int i =0; i<newRemainString.Length; i++) remain[i] = rem[i];
                //remain = newRemainString.ToCharArray();

                answerSought = Length(answers);
                
                // happy if the number of anagrams are 6 or more, and less than 77
                happy = ((answerSought < 77) && (answerSought >= 6));
             
#if DEBUG
    if (!happy) Console.WriteLine($"Too Many Answers!  word: {new string(rootWord)}, answers: {answerSought}");
#endif
            }
Console.WriteLine("Happy found");
#if DEBUG
    if (happy) Console.WriteLine($"Selected word: {new string(rootWord)}, answers: {answerSought}");
#endif

            Sort(ref answers);

            for (int i = bigWordLen; i<7; i++)
            {
                remain[i] = SPACE_CHAR;
            }
            remain[7] = '\0';
            remain[bigWordLen] = '\0';

            ShuffleWord(remain);
            // HERE


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


        /// <summary>
        /// Shuffles the characters in a given word array.
        /// </summary>
        /// <param name="word">The word array to be shuffled.</param>
        /// <remarks>
        /// This method generates a random number between 20 and 26, and then swaps two characters in the word array
        /// for the generated number of times. The characters to be swapped are randomly selected using the Random class.
        /// </remarks>

        public static void ShuffleWord(char[] word)
        {
            char tmp;
            
            // generate a random number between 20 and 26. The rand() function in C no longer exists in C#
            Random randCount = new Random();
            int count = randCount.Next(20, 27);

            // generate two random values, using the same random generator to prevent possible repeated values.
            Random randPos = new Random();
            int a = randPos.Next(0, 7);
            int b = randPos.Next(0, 7);

            for (int n=0; n < count; ++n )
            {
                a = randPos.Next(0, 7);
                b = randPos.Next(0, 7);
                tmp = word[a];
                word[a] = word[b];
                word[b] = tmp;
            }

        }
    }
}