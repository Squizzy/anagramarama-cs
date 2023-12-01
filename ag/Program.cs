using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
/*// using System.Reflection.Metadata;
// using System.IO.Compression;
*/

namespace ag
{
    class Program
    {
        // Debug
        // Private static bool myDEBUG = true;
        private static bool myDEBUGmacos = false;
        private static bool myDEBUGfr = false;

        // dbl_node contains the letters of the dictionary, and the links for the making up the words
        public class dlb_node
        {
            public char letter;
            public int valid; // indicates end of a word
            public dlb_node? sibling; //a letter that belongs to a new word which shares the same initial letters until this point.
            public dlb_node? child; // letter of the same word as previous word
            //e.g.: abaci: 5 children. Aback: sibling at position 4, only 'k' is recorded
        }

        //dbl_linkedlist contains the whole dictionary, a collection of linked dbl_nodes.
        public class dlb_linkedlist
        {
            private int countLoop = 0;

            public dlb_node dlb_node_create_node(char c)
            {
                dlb_node newNode = new dlb_node();
                newNode.letter = c;
                newNode.valid = 0;
                newNode.sibling = null;
                newNode.child = null;
                return newNode;
            }

            public delegate int dlb_node_operation(dlb_node node);  // typeset to define dbl_node_operation used only in the method below

            public void dlb_walk(dlb_node node, dlb_node_operation op)
            {
                while (node != null)
                {
                    dlb_node tempNode = node;
                    if (node.child != null)
                    {
                        dlb_walk(node.child, op);
                    }
                    node = node.sibling;
                    op(tempNode);
                }
            }
        
            // method to load words into the dictionary
            public void dlb_push(dlb_node dlbHead, string word)
            {
                dlb_node current = dlbHead;
                dlb_node previous = null;
                bool child = false;
                bool sibling = false;
                //bool newHead = (dlbHead.letter == '\0');  // not sure why this is or how to solve this - need to check that dlbHeader is empty but not sure how to do it
                bool newHead = (dlbHead == null);

                countLoop++;

                int currentWordLetterNum=0;
                char[] letters = word.ToCharArray();
                
                do
                {
                    char letter = letters[currentWordLetterNum];
                    if (current==null)
                    {
                        current = dlb_node_create_node(letter);
                        if (newHead)
                        {
                            dlbHead = current;
                            newHead = false;
                        }
                        if (child)
                        {
                            previous.child = current;
                        }
                        if (sibling)
                        {
                            previous.sibling = current;
                        }
                    }
                    child = false;
                    sibling = false;
                    previous = current;

                    if (letter == previous.letter)
                    {
                        currentWordLetterNum++;
                        child = true;
                        current = previous.child;
                    }
                    else
                    {
                        Console.WriteLine("found a sibling at letter count: " + currentWordLetterNum);
                        sibling = true;
                        current = previous.sibling;
                    }
                } while (currentWordLetterNum < word.Length);

                previous.valid = 1;
                //}
            }

            // method to load the dictionary from the file
            public void dlb_create(dlb_node dlbHead, string filename)
            {
                int lineCount = File.ReadLines(filename).Count();
                string? currentWord;
                StreamReader sr = new StreamReader(filename);

                try
                {
                    for (int i=0; i<lineCount; i++)
                    {
                        currentWord = sr.ReadLine();
                        if (currentWord != null) dlb_push(dlbHead, currentWord);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {
                    Console.WriteLine("executing final block");
                }
                sr.Close();
            }

            // method to looksup a word in the linked list (dictionary)
            public int dbl_lookup(dlb_node dlbHead, string word)
            {
                dlb_node current = dlbHead;
                dlb_node previous = null;
                int retval = 0;


                int currentWordLetterNum=0;
                char[] letters = word.ToCharArray();
                
                do
                {
                    char letter = letters[currentWordLetterNum];
                    if (current==null)
                    {
                        retval = 0;
                        break;
                    }

                    previous = current;

                    if (letter == previous.letter)
                    {
                        currentWordLetterNum++;
                        current = previous.child;
                        retval = previous.valid;
                    }
                    else
                    {
                        current = previous.sibling;
                        retval = 0;
                    }
                } while (currentWordLetterNum < word.Length);

                return retval;
            }
        }


        // node contains ???
        public class node
        {
            public string? anagram;
            public int found;
            public int guessed;
            public int length;
            public node? next;
        }

        // method to count the length of the linked list
        public int Length(node nodeHead)
        {
            node? current = new node();
            current = nodeHead;
            int count = 0;

            while (current != null)
            {
                ++count;
                current = current.next;
            }
            return count;
        }

        // method to swap the content from two linkedlist nodes without changing the position of the node
        public void Swap(node nodeFrom, node nodeTo)
        {
            string? word = nodeFrom.anagram;
            int len = nodeFrom.length;

            nodeFrom.anagram = nodeTo.anagram;
            nodeFrom.length = nodeTo.length;
            nodeTo.anagram = word;
            nodeTo.length = len;
        }

        // method to sort the list first alphabetically then by increasing word length
        public void Sort(node nodeHead)
        {
            node? left, right;
            bool completed = false;

            while (!completed)
            {
                left = nodeHead;
                right = left.next;
                completed = true;
                do
                {
                    if (String.Compare(left.anagram, right.anagram) >0)
                    {
                        Swap(left, right);
                        completed = false;
                        left = left.next;
                        right = right.next;
                    }
                } while ((left != null) && (right != null));
            }
        }

        // method to reset the answers
        public void DestroyAnswers(node? nodeHead)
        {
            node? current = nodeHead;
            node? previous = nodeHead;

            while (current != null)
            {
                current.anagram = null;
                previous = current;
                current = current.next;
                previous = null;
            }
            nodeHead = null;
        }


        // method to identify the local language path for the locale files (dictionarity, background, ...)
        private static string DictPathLanguage()
        {
            string path = "i18n/";
            if (!myDEBUGmacos) path = "../../../i18n/"; 
            
            string lang;
            lang = System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag;
            //backup in case no locale was returned: en-GB
            if (lang == null) lang = "en-GB";
            if (myDEBUGfr) { lang = "fr-FR";}
            //To be extended with checks for "isValidLocale"?
            
            return path + lang + "/";
        }

        public static void Main()
        {
            // initiate the reference to the first node (first letter to be gathered from the input file)
            dlb_node newNode = new dlb_node();  

            // initiate the reference to the linked list, ie the sequence of letters each stored in a new node.
            dlb_linkedlist newLinkedList = new dlb_linkedlist(); 

            //find the local path with the IETF international code
            string dictonaryPathLanguage = DictPathLanguage();

            // load the dictionary
            newLinkedList.dlb_create(newNode, dictonaryPathLanguage + "wordlist.txt");
            
            Console.WriteLine("end");
        }
    }
}

        