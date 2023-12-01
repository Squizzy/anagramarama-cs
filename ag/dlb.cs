namespace ag
{
    partial class Program
    {
        // dbl_node contains the letters of the dictionary, and the links for the making up the words
        // from dlb.c
        public class dlb_node
        {
            public char letter;
            public int valid; // indicates end of a word
            public dlb_node? sibling; //a letter that belongs to a new word which shares the same initial letters until this point.
            public dlb_node? child; // letter of the same word as previous word
            //e.g.: abaci: 5 children. Aback: sibling at position 4, only 'k' is recorded
        }

        // dbl_linkedlist contains the whole dictionary, a collection of linked dbl_nodes.
        // from dlb.c
        public class dlb_linkedlist
        {
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

            public void dlb_walk(dlb_node? node, dlb_node_operation op)
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
                dlb_node? current = dlbHead;
                dlb_node? previous = null;
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
    }
}