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
        public static Dlb_node Dlb_node_create_node(char c)
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
        /// <param name="Node">The node to be cleared</param>
        /// <returns>Nothing</returns>

        public static void Dlb_free_node(Dlb_node? Node)
        {
            Node = null;
        }


        /// <summary>
        /// Walk through the whole of the dictionary linkedlist and perform the op delegate on the node
        /// In this particular application, op is called to free the node
        /// This method just walks the whole linkedlist and clears all the nodes.
        /// Not really needed in c# but kept for sake of keeping...
        /// </summary>
        /// <param name="node">The node to be freed</param>
        /// <param name="op">The method to be called to be applied to the node (in this app: free the memory)</param>
        /// <returns>Nothing</returns>
        public static void Dlb_walk(ref Dlb_node? node, Dlb_node_operation op)
        {
            while (node != null)
            {
                Dlb_node tempNode = node;
                if (node.child != null)
                {
                    Dlb_walk(ref node.child, op);
                }
                node = node.sibling;
                op(tempNode);
            }
        }

        /// <summary>
        /// Frees each Dlb_node node of the dictionary linked list
        /// Not really needed in c# but kept for sake of keeping...
        /// </summary>
        /// <param name="headNode">The headnode of the dictionary to clear</param>
        /// <returns>Nothing</returns>
        public static void Dlb_free(ref Dlb_node? headNode)
        {
            Dlb_walk(ref headNode, Dlb_free_node);
        }

        
        /// <summary>
        /// add a new word to the De La Briandais Trie Dlb dicionary linked list
        /// load a new word into the dictionary link list, taking into account children and siblings possibilities.
        /// </summary>
        /// <param name="dlbHead">The head node of the dictionary</param>
        /// <param name="word">The word to insert in the linked list</param>
        /// <returns>Nothing</returns>
        public static void Dlb_push(ref Dlb_node? dlbHead, string word)
        {
            Dlb_node? current = dlbHead;
            Dlb_node? previous = null;
            bool child = false;
            bool sibling = false;
            bool newHead = dlbHead == null;

            while (word.Length > 0)
            {
                char letter = word[0];

                if (current == null)
                // This position can be reached when starting a new head (new dictionary linked list), 
                // or current had been set to previous - which means child or sibling will have been set
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
        /// <param name="dlbHead">The head node of the dictionary.</param>
        /// <param name="filename">The name of the file containing the dictionary words.</param>
        /// <returns>Nothing</returns>

        public static void Dlb_create(ref Dlb_node? dlbHead, string filename)
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
                        Dlb_push(ref dlbHead, currentWord);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("executing final block");
            }

        }

        // method to looksup a word in the linked list (dictionary)

        /// <summary>
        /// Determine if a given word is in the dictionary 
        /// essentially the same as a push, but doesn't add any of the new letters
        /// </summary>
        /// <param name="dlbHead">the dictionary linked list</param>
        /// <param name="word">the word to find</param>
        /// <returns> return true if the word is in the dictionary else return false </returns>
        public static bool Dlb_lookup(Dlb_node? dlbHead, string word)
        {
            Dlb_node? current = dlbHead;
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