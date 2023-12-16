using System.Runtime.InteropServices;

namespace ag
{
    partial class Program
    {
        /**************
         dbl_node contains the key building blocks ("node") for the dictionary that is loaded from the text file provided. 
         
         each node contains:
         - letter:  a letter that has been read from the dictionary file
         - valid:   the indication that this letter is the end of a word
         - child:   the link to the next "node" which, together with the previous nodes since the last valid, 
                    and with all other children until the next valid, makes up a word
         - sibling: the link to the next "node" for a new word that uses the same letters that have been used until now, 
                    with its letter replacing the current node's for that word. ie "branches out" into a new word

         e.g.
            "head node" (nothing) 
                -> child -
                -> sibling 'a'
                    -> child 'b'
                        -> child 'b'
                            -> child 'a'
                                -> child 'c'
                                    -> child 'i' 
                                        -> child -, valid -> word "abaci"
                                        -> sibling 'k', valid -> word "aback"
                                            -> child -
                                            -> sibling 'u'
                                                -> child s
                                                    -> child -, sibling -, valid -> word "abacus"
                                                -> sibling -
                                -> sibling 'f'
                                    -> child 't'
                                        -> child -, valid -> word "abaft"
                                    
                    -> sibling 'b' -> start of words starting with 'b'         

        **************/

        public class dlb_node
        {
            public char letter;
            public bool valid; // indicates end of a word
            public dlb_node? sibling; //a letter that belongs to a new word which shares the same initial letters until this point.
            public dlb_node? child; // letter of the same word as previous word

        }

        // dlb_linkedlist contains the whole dictionary, a collection of linked dbl_nodes. However it is not needed in c# ?
        // from dlb.c
        /*public class dlb_linkedlist
        {*/

        // define a root node so we can easily point to the begining of the dictionary
        // but not sure I'll use it
        // CHECK LATER
        private dlb_node head = null; 
        

        /**************
        // This creates a new node and initialises it with a character - in reality probably not required with c# any more? -> use constructor instead would achieve the same
        // CHECK LATER
        **************/
        public static dlb_node Dlb_node_create_node(char c)
        {
            dlb_node newNode = new dlb_node();
            newNode.letter = c;
            newNode.valid = false;
            newNode.sibling = null;
            newNode.child = null;
            return newNode;
        }
        /*}*/

        /**************
        //
        // typeset to define dbl_node_operation used only in the method below
        // not sure why it was used, maybe problems with overloading?
        // CHECK LATER
        //
        **************/
        public delegate int dlb_node_operation(dlb_node node);  

        /**************
        //
        // method that goes through the whole dictonary and lists the words
        // takes the dictionary as input "node", and recursively goes through the whole of it.
        // This is used in the latest dulsi version of anagramarama to clear the whole of the dictionary linked list. (the op operation called was to memfree the current node)
        // probably not required here as there is no need to manuall free memory in c# 
        //
        **************/
        public static void Dlb_walk(dlb_node? node, dlb_node_operation op)
        {
            while (node != null)
            {
                dlb_node tempNode = node;
                if (node.child != null)
                {
                    Dlb_walk(node.child, op);
                }
                node = node.sibling;
                op(tempNode);
            }
        }
        
            /**************
            //
            // method to load a new word into the dictionary link list, taking into account children and siblings possibilities.
            //
            **************/
            public static void Dlb_push(dlb_node dlbHead, string word)
            {
                dlb_node? current = new dlb_node();
                dlb_node? previous = new dlb_node();
                current = dlbHead;
                previous = null;
                bool child = false;
                bool sibling = false;
                bool newHead = (dlbHead == null);

                int currentWordLetterNum=0;
                char[] letters = word.ToCharArray();
                
                do
                {
                    char letter = letters[currentWordLetterNum];
                    if (current==null)
                    {
                        current = Dlb_node_create_node(letter);
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
//                        Console.WriteLine("found a sibling at letter count: " + currentWordLetterNum);
                        sibling = true;
                        current = previous.sibling;
                    }
                } while (currentWordLetterNum < word.Length);

                previous.valid = true;
                //}
            }

            // method to load the dictionary from the file
            public static void Dlb_create(dlb_node dlbHead, string filename)
            {
                int lineCount = File.ReadLines(filename).Count();
                string? currentWord;
                StreamReader sr = new StreamReader(filename);

                try
                {
                    for (int i=0; i<lineCount; i++)
                    {
                        currentWord = sr.ReadLine();
                        if (currentWord != null) Dlb_push(dlbHead, currentWord);
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
            public static bool Dlb_lookup(dlb_node dlbHead, string word)
            {
                dlb_node? current = dlbHead;
                dlb_node? previous = null;
                bool retval = false;


                int currentWordLetterNum=0;
                char[] letters = word.ToCharArray();
                
                do
                {
                    char letter = letters[currentWordLetterNum];
                    if (current==null)
                    {
                        retval = false;
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
                        retval = false;
                    }
                } while (currentWordLetterNum < word.Length);

                return retval;
            }
//        }
    }
}