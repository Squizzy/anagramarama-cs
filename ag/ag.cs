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

                
                // Not needed in C# as garbage collection is handled already
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
            shuffle = remain;
            answer = SPACE_FILLED_STRING;

// HERE




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


        public static void BuildLetters(ref Sprite letters, IntPtr screen)
        {
            Sprite thisLetter = new Sprite();
            Sprite previousLetter = new Sprite();
            SDL.SDL_Rect rect = new SDL.SDL_Rect();
            int index = 0;

            rect.y = 0;
            rect.w = GAME_LETTER_WIDTH;
            rect.h = GAME_LETTER_HEIGHT;

            int len = shuffle.Length;

            for (int i=0; i<len; i++)

            {
                thisLetter.numSpr = 0;

                // determine which letter we're wanting and load it from 
                // the letterbank*/

                if ((int)shuffle[i] != ASCII_SPACE && shuffle[i] != SPACE_CHAR )
                {
                    int chr = (int)(shuffle[i] - 'a');
                    rect.x = chr * GAME_LETTER_WIDTH;
                    thisLetter.numSpr = 1;


                    thisLetter.spr[0].t = letterBank;
                    thisLetter.spr[0].w = rect;
                    thisLetter.spr[0].x = 0;
                    thisLetter.spr[0].y = 0;


                    thisLetter.x = rnd.Next(0, 799); // Dulsi comment did not seem to align with his code: i * (GAME_LETTER_WIDTH + GAME_LETTER_SPACE) + BOX_START_X;
                    thisLetter.y = rnd.Next(0, 599); // Dulsi comment did not seem to align with his code:  SHUFFLE_BOX_Y;
                    thisLetter.letter = shuffle[i];
                    thisLetter.h = GAME_LETTER_HEIGHT;
                    thisLetter.w = GAME_LETTER_WIDTH;
                    thisLetter.toX = i * (GAME_LETTER_WIDTH + GAME_LETTER_SPACE) + BOX_START_X;
                    thisLetter.toY = SHUFFLE_BOX_Y;
                }
            }



        }
// HERE TOO
    }
}