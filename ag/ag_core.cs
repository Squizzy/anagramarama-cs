using System.Runtime.CompilerServices;

namespace ag
{
    
    partial class Program
    {
    //    public static Random rnd;

        /// <summary>
        /// determine the next blank space in a string - blanks are indicated by pound not space
        /// returns the first occurrence of SPACE_CHAR in a string
        /// </summary>
        /// <param name="thisString"></param>
        /// <returns>returns position of next blank (1 is first character) or 0 if no blanks found</returns>
        public static int NextBlank(string thisString)
        {
            // +1 is needed to align with the 1-position of first character in original C application
            return thisString.IndexOf(SPACE_CHAR) + 1;
        }

        /// <summary>
        /// shift a string one character to the left, truncating the leftmost character
        /// </summary>
        /// <param name="thisString"></param>
        /// <returns>thisString less its first character</returns>
        public static string ShitfLeftKill(string thisString)
        {
            return thisString[1..];
        }

        /// <summary>
        /// shift a string one character to the left and move the first character to the end so it wraps around
        /// </summary>
        /// <param name="thisString"></param>
        /// <returns></returns>
        public static string ShiftLeft(string thisString)
        {
            return string.Concat(thisString[1..] , thisString[..1]);
        }

        // Generate all possible combinations of the root word "remain" the initial letter is fixed (save under "head"), so to work out all anagrams in the dictionarydlbHead, prefix with space.
        //public void Ag(Node head, dlb_node dlbHead, string guess, string remain)

        /// <summary>
        /// Generate all possible combinations of the root word
        /// the initial letter is fixed, so to work out all
        /// anagrams of a word, prefix with space.
        /// </summary>
        /// <param name="headNode">The head node of the anagrams list</param>
        /// <param name="dlbHeadNode">The head node of the dictionary</param>
        /// <param name="guess">the current guess</param>
        /// <param name="remain">the remaining letters</param>
        /// <returns>Nothing, the anagram's linkedlist is updated by reference</returns>

        public static void Ag(ref Node headNode, Dlb_node dlbHeadNode, string guess, string remain)
        {
            char[] newGuess = guess.ToCharArray();
            char[] newRemain = remain.ToCharArray();

            int totalLen = 0, guessLen = 0, remainLen = 0;

            guessLen = guess.Length;
            remainLen = remain.Length;
            totalLen = guessLen + remainLen;

            // add the element at last position of newRemain to newGuess
            newGuess[guessLen] = newRemain[remainLen - 1];

            // remove the last element of newRemain
            newRemain = newRemain[..^1];

            // If the newGuess word is more than 3 char
            if (newGuess.Length > 3)
            {
                // Shift its letters left dropping the first one
                string shiftLeftKilledString = ShitfLeftKill(new string(newGuess));

                // If this is a word in the dictionary, add it to the anagrams linkedlist
                if (Dlb_lookup(dlbHeadNode, shiftLeftKilledString))
                {
                    Push(ref headNode, shiftLeftKilledString);
                }
            }

            if (newRemain.Length > 0)
            {
                // Recursively check other words
                Ag(ref headNode, dlbHeadNode, new string(newGuess), new string(newRemain));

                // Then for all the total letters
                for (int i = totalLen - 1; i > 0; i--)
                {
                    // recursively try all the combinations of newRemain letters with the guess
                    if (newRemain.Length > i)
                    {
                        newRemain = ShiftLeft(new string(newRemain)).ToCharArray();
                        Ag(ref headNode, dlbHeadNode, new string(newGuess), new string(newRemain));
                    }
                }
            }

            // //if (guess.Length != 0) 
            // for (int j = 0; j < guess.Length; j++) newGuess[j] = guess[j];
            // newRemain = remain.ToCharArray();

                // newGuess[guessLen] = newRemain[remainLen - 1];
                // newGuess[guessLen + 1] = '\0';  // null char
                // newRemain[remainLen - 1] = '\0';

                // string newGuessString = new string(newGuess).Trim('\0');
                // string newRemainString = new string(newRemain).Trim('\0');

                // if (newGuessString.Length > 3)
                // {
                //     string str = ShitfLeftKill(newGuessString);
                //     if (Dlb_lookup(dlbHeadNode, str))
                //     {
                //         headNode = Push(headNode, str);
                //     }
                // }

                // if (newRemainString.Length != 0)
                // {
                //     //Ag(head, dlbHead, newGuess.ToString(), newRemain.ToString());
                //     headNode = Ag(ref headNode, dlbHeadNode, newGuessString, newRemainString);

                //     for (int i = totalLen - 1; i > 0; i--)
                //     {
                //         if (newRemainString.Length > i)
                //         {
                //             //newRemain = ShiftLeft(newRemain.ToString()).ToCharArray();
                //             //Ag(head, dlbHead, newGuess.ToString(), newRemain.ToString());
                //             newRemainString = ShiftLeft(newRemainString);
                //             headNode = Ag(ref headNode, dlbHeadNode, newGuessString, newRemainString);
                //         }
                //     }
                // }
                // return headNode;
        }


        /// <summary>
        /// Get a random word in the dictionary file (not the dlb dictionary linked list)
        /// [note from original C file] spin the word file to a random location and then loop until a 7 or 8 letter word is found
        /// This is quite a weak way to get a random word considering we've got a nice dbl Dictionary to hand - 
        /// but it works for now.
        /// </summary>
        /// <param name="randomWord">the referenced random word variable (will contain the random word found)</param>
        /// <param name="randomWordMinLength">The min length desired for the random word (7 or 8 in the original game)</param>
        /// <returns>Nothing as the result is passed by reference</returns>

        public static void GetRandomWord(ref string randomWord, int randomWordMinLength)
        {
            string randomWordTemp;

            string filename = DictLanguagePath() + "wordlist.txt";

            string[] lines = File.ReadAllLines(filename);

            int lineCount = lines.Length;

            Random rnd = new();
            do
            {
                randomWordTemp = lines[rnd.Next(lineCount)];
            }
            while (randomWordTemp.Length < randomWordMinLength);

            randomWord = randomWordTemp;
        }

    }
}