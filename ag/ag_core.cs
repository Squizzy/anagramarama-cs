using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;

namespace ag
{
    
    partial class Program
    {
       public static Random rnd;

        /// <summary>
        /// returns the first occurrence of SPACE_CHAR in a string
        /// </summary>
        /// <param name="thisString"></param>
        /// <returns></returns>
        public static int NextBlank(string thisString)
        {
            return thisString.IndexOf(SPACE_CHAR);
        }

        /// <summary>
        /// shift a string one character to the left, truncating the leftmost character
        /// </summary>
        /// <param name="thisString"></param>
        /// <returns></returns>
        public static string ShitfLeftKill(string thisString)
        {
            return thisString.Remove(0,1);
        }

        /// <summary>
        /// shift a string one character to the left and move the first character to the end so it wraps around
        /// </summary>
        /// <param name="thisString"></param>
        /// <returns></returns>
        public static string ShiftLeft(string thisString)
        {
            return string.Concat(thisString.Remove(0,1) , thisString.Substring(0,1));
        }

        // Generate all possible combinations of the root word "remain" the initial letter is fixed (save under "head"), so to work out all anagrams in the dictionarydlbHead, prefix with space.
        //public void Ag(Node head, dlb_node dlbHead, string guess, string remain)
        public static Node Ag(ref Node head, dlb_node dlbHead, string guess, string remain)
        {
            char[] newRemain;
            int totalLen = 0, guessLen = 0, remainLen = 0;
            //newGuess[0] = '\0';

            guessLen = guess.Length;
            remainLen = remain.Length;
            totalLen = guessLen + remainLen;
            
            char[] newGuess = new char[totalLen+1];
            
            //if (guess.Length != 0) 
            for (int j=0; j<guess.Length; j++) newGuess[j] = guess[j];
            newRemain = remain.ToCharArray();

            newGuess[guessLen] = newRemain[remainLen-1];
            newGuess[guessLen+1] = '\0';  // null char
            newRemain[remainLen-1] = '\0';

            string newGuessString = new string(newGuess).Trim('\0');
            string newRemainString = new string(newRemain).Trim('\0');

            if (newGuessString.Length > 3)
            {
                string str = ShitfLeftKill(newGuessString);
                if (Dlb_lookup(dlbHead, str))
                {
                    head = Push(head, str);
                }
            }

            if (newRemainString.Length !=0 )
            {
                //Ag(head, dlbHead, newGuess.ToString(), newRemain.ToString());
                head = Ag(ref head, dlbHead, newGuessString, newRemainString);

                for (int i = totalLen-1 ; i>0 ; i--)
                {
                    if (newRemainString.Length >i)
                    {
                        //newRemain = ShiftLeft(newRemain.ToString()).ToCharArray();
                        //Ag(head, dlbHead, newGuess.ToString(), newRemain.ToString());
                        newRemainString = ShiftLeft(newRemainString);
                        head = Ag(ref head, dlbHead, newGuessString, newRemainString);
                    }
                }
            }
            return head;
        }

        
        /// <summary>
        /// Get a random word in the dictionary file:
        /// </summary>
        /// <param name="wordsListPath">Variable that contains relative path additions, used esp for debug at time of development, taken as an args of Main()</param>
        /// <returns>Returns the random word selected as a string</returns>
        /// <remarks>point randomly in the dictionary and then read words until a word >=7 letters if found (ie 7 or 8)</remarks>
        public static string GetRandomWord(string wordsListPath = "")
        {
            string filename = DictPathLanguage(wordsListPath) + "wordlist.txt";
            int lineCount = File.ReadLines(filename).Count();

            rnd = new Random();
            int randomPos = rnd.Next(0, lineCount-1);
            int lineNum = randomPos;
            //string line = File.ReadLines(filename).Skip(randomPos - 1);

            StreamReader sr = new StreamReader(filename);
            //for (int i=0; i<randomPos; i++) sr.ReadLine();   // jump to the random line 
            
            string? line = "";
            while (line.Length < 7 || line == null)
            {
                /*line = sr.ReadLine();
                if (line == "") sr.*/

                line = File.ReadLines(filename).ElementAtOrDefault(lineNum++);
                if (line == null) lineNum = 0; // if we have reached the end of the file loop back and continue from the begining.
            }

            return line;
        }

    }
}