using System.Dynamic;

namespace ag
{
    partial class Program
    {

        /// <summary>
        /// Constructor for Dlb_node node
        /// Creates a new Dlb_node with the specified character and initializes its properties. 
        /// Not really needed in c# but kept for sake of keeping...
        /// </summary>
        /// <param name="c">The character for the new node.</param>
        /// <returns>The newly created Dlb_node</returns>
        public static Dlb_node DlbNodeCreateNode(char c)
        {
            Dlb_node newNode = new()
            {
                letter = c,
                valid = false,
                sibling = null,
                child = null
            };
            return newNode;
        }

        /// <summary>
        /// Destructor for a Dlb_node node. 
        /// Not really needed in c# but kept for sake of keeping...
        /// </summary>
        /// <param name="dlbHeadNode">The node to be cleared</param>
        /// <returns>Nothing</returns>

        public static void DlbFreeNode(Dlb_node? dlbHeadNode)
        {
            dlbHeadNode = null;
        }


        /// <summary>
        /// Walk through the whole of the dictionary linkedlist and perform the op delegate on the node
        /// In this particular application, op is called to free the node
        /// This method just walks the whole linkedlist and clears all the nodes.
        /// Not really needed in c# but kept for sake of keeping...
        /// </summary>
        /// <param name="dlbHeadNode">The node to be freed</param>
        /// <param name="op">The method to be called to be applied to the node (in this app: free the memory)</param>
        /// <returns>Nothing</returns>
        public static void DlbWalk(ref Dlb_node? dlbHeadNode, Dlb_node_operation op)
        {
            while (dlbHeadNode != null)
            {
                Dlb_node tempNode = dlbHeadNode;
                if (dlbHeadNode.child != null)
                {
                    DlbWalk(ref dlbHeadNode.child, op);
                }
                dlbHeadNode = dlbHeadNode.sibling;
                op(tempNode);
            }
        }

        /// <summary>
        /// Frees each Dlb_node node of the dictionary linked list
        /// Not really needed in c# but kept for sake of keeping...
        /// </summary>
        /// <param name="headNode">The headnode of the dictionary to clear</param>
        /// <returns>Nothing</returns>
        public static void DlbFree(ref Dlb_node? headNode)
        {
            DlbWalk(ref headNode, DlbFreeNode);
        }

        
        /// <summary>
        /// add a new word to the De La Briandais Trie Dlb dicionary linked list
        /// load a new word into the dictionary link list, taking into account children and siblings possibilities.
        /// </summary>
        /// <param name="dlbHeadNode">The head node of the dictionary</param>
        /// <param name="word">The word to insert in the linked list</param>
        /// <returns>Nothing</returns>
        public static void DlbPush(ref Dlb_node? dlbHeadNode, string word)
        {
            Dlb_node? current = dlbHeadNode;
            Dlb_node? previous = null;
            bool child = false;
            bool sibling = false;
            bool newHead = dlbHeadNode == null;

            while (word.Length > 0)
            {
                char letter = word[0];

                if (current == null)
                // This position can be reached when starting a new head (new dictionary linked list), 
                // or current had been set to previous - which means child or sibling will have been set
                {
                    current = DlbNodeCreateNode(letter);
                    if (newHead)
                    {
                        dlbHeadNode = current;
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

                // Reset the detection of child or sibling
                child = false;
                sibling = false;
                // and set the previous node to the current one.
                previous = current;

                // if the current letter already exists in the tree, we are on a child, then move to the next letter
                if (letter == previous.letter)
                {
                    // currentWordLetterNum++;
                    // Move to the next letter in the word (remove the first letter of the word)
                    word = word[1..];
                    // Declare that we are working with a child
                    child = true;
                    // set the current node to the child of the previous node (node will be null but the "child" will be set)
                    current = previous.child;
                }
                // Otherwise we are on a sibling
                else
                {
                    //                        Console.WriteLine("found a sibling at letter count: " + currentWordLetterNum);
                    // declare that we are working with a sibling
                    sibling = true;
                    // set the current not to the sibling of the previous node (node will be null but the "sibling" will be set)
                    current = previous.sibling;
                }
                // } while (currentWordLetterNum < word.Length);

                previous.valid = true;
                //}
            }
        }


        /// <summary>
        /// This method is used to create a dictionary linked list from a file. 
        /// It reads each line of the file, adds the words to the dictionary, and sets the necessary links between nodes. 
        /// </summary>
        /// <param name="dlbHeadNode">The head node of the dictionary.</param>
        /// <param name="filename">The name of the file containing the dictionary words.</param>
        /// <returns>Nothing</returns>

        public static bool DlbCreate(ref Dlb_node? dlbHeadNode, string filename)
        {
            int lineCount = File.ReadLines(filename).Count();
            string? currentWord;
            using StreamReader sr = new(filename);

            try
            {
                for (int i = 0; i < lineCount; i++)
                {
                    currentWord = sr.ReadLine();
                    if (currentWord != null)
                    {
                        DlbPush(ref dlbHeadNode, currentWord);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return false;
            }
            finally
            {
                Console.WriteLine("executing final block");
            }
            return true;

        }

        // method to looksup a word in the linked list (dictionary)

        /// <summary>
        /// Determine if a given word is in the dictionary 
        /// essentially the same as a push, but doesn't add any of the new letters
        /// </summary>
        /// <param name="dlbHeadNode">the dictionary linked list</param>
        /// <param name="word">the word to find</param>
        /// <returns> return true if the word is in the dictionary else return false </returns>
        public static bool DlbLookup(Dlb_node? dlbHeadNode, string word)
        {
            Dlb_node? current = dlbHeadNode;
            Dlb_node? previous;
            // bool retval = false;
            bool wordInDictionary = false;

            while (word.Length > 0)
            {
                char letter = word[0];

                if (current == null)
                {
                    return wordInDictionary;
                }

                previous = current;

                if (letter == previous.letter)
                {
                    word = word[1..];
                    current = previous.child;
                    wordInDictionary = previous.valid;
                }
                else
                {
                    current = previous.sibling;
                    wordInDictionary = false;
                }
            }

            return wordInDictionary;

            // int currentWordLetterNum = 0;
            // char[] letters = word.ToCharArray();

            // do
            // {
            //     char letter = letters[currentWordLetterNum];
            //     if (current == null)
            //     {
            //         retval = false;
            //         break;
            //     }

            //     previous = current;

            //     if (letter == previous.letter)
            //     {
            //         currentWordLetterNum++;
            //         current = previous.child;
            //         retval = previous.valid;
            //     }
            //     else
            //     {
            //         current = previous.sibling;
            //         retval = false;
            //     }
            // } while (currentWordLetterNum < word.Length);

            // return retval;
        }
    }
}