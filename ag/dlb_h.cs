namespace ag
{
    partial class Program
    {
        /*
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

        */

        /// <summary> Node for the linkedlist containing all the possible words of the dictionary loaded for the game </summary>
        public class Dlb_node
        {
            /// <summary> The letter in the word </summary>
            public char letter;
            /// <summary> end of a valid word composed from the previous nodes </summary>
            public bool valid;
            /// <summary> pointer a letter that belongs to a new word which shares the same initial letters until this point. </summary>
            public Dlb_node? sibling;
            /// <summary> pointer to the next letter of the same word </summary>
            public Dlb_node? child; // 

            /// <summary> Constructor for the new node </summary>
            public Dlb_node()
            {
                letter = '\0';
                valid = false;
                sibling = null;
                child = null;
            }
        }


        // typedef int (*dlb_node_operation)(struct dlb_node *node);
        
        /// <summary>
        /// Delegate for operations on a Dlb_node.
        /// Modified to return nothing as the original app did not make use of the return value anyway
        /// </summary>
        /// <param name="node">The Dlb_node to operate on.</param>
        /// <returns>Nothing</returns>
        public delegate void Dlb_node_operation(Dlb_node node);

    }
}