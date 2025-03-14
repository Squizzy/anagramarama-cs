namespace ag
{
    partial class Program
    {

        /// <summary> returns the number of anagrams from the root word </summary>
        /// <param name="headNode">pointer to the first node</param>
        /// <returns> integer value of the number of anagrams in the list </returns>
        public static int Length(Node headNode)
        {
            Node? current = new();
            current = headNode;
            int count = 0;

            while (current != null)
            {
                ++count;
                current = current.next;
            }
            return count;
        }

        /// <summary> 
        /// swap the content from two linkedlist nodes without changing the position of the node 
        /// This is used when sorting the list alphabetically and by anagram's length
        /// </summary>
        /// <param name="fromNode">first node</param>
        /// <param name="toNode">second node</param>
        /// <returns>Nothing</returns>
        public static void Swap(ref Node fromNode, ref Node toNode)
        {
            string? word = fromNode.anagram;
            int len = fromNode.length;

            fromNode.anagram = toNode.anagram;
            fromNode.length = toNode.length;
            toNode.anagram = word;
            toNode.length = len;
        }

        /// <summary>
        /// sort the anagrams list first alphabetically then by increasing word length
        /// </summary>
        /// <param name="headNode">the node head</param>
        /// <returns>Nothing</returns>
        public static void Sort(ref Node headNode)
        {
            Node? left, right;
            bool completed = false;

            while (!completed)
            {
                left = headNode;
                right = left.next;
                completed = true;
                while ((left != null) && (right != null))
                {
                    if (String.Compare(left.anagram, right.anagram) > 0)
                    {
                        Swap(ref left, ref right);
                        completed = false;
                    }
                    left = left.next;
                    right = right.next;
                }
            }

            completed = false;
            while (!completed)
            {
                left = headNode;
                right = left.next;
                completed = true;
                while ((left != null) && (right != null))
                {
                    if (left.length > right.length)
                    {
                        Swap(ref left, ref right);
                        completed = false;
                    }
                    left = left.next;
                    right = right.next;
                }
            }
        }

        // method to reset the answers
        /// <summary>
        /// Resets the linkedlist of the anagrams from the root word
        /// unlike with C, the garbage collector of C# will take care of reclaiming the meory
        /// </summary>
        /// <param name="headNode">the head node</param>
        /// <returns>Nothing</returns>
        public static void DestroyAnswers(Node? headNode)
        {
            headNode = null;
        }


        /// <summary> 
        /// add a new word at the front of the linkedlist of anagrams 
        /// as long as it is not a duplicate 
        /// </summary>
        /// <param name="headNode"></param>
        /// <param name="new_anagram">The word to add to the list</param>
        /// <returns>Nothing as the linkedlist itself is modified</returns>
        public static void Push(Node headNode, string new_anagram)
        {
            Node? current = headNode;
            // int len;
            bool duplicate = false;

            // check if the word is already in the list
            while (current != null)
            {
                if (string.Equals(new_anagram, current.anagram))
                {
                    duplicate = true;
                    break;
                }
                current = current.next;
            }

            if (!duplicate)
            {
                Node newNode = new()
                {
                    anagram = new_anagram,
                    length = new_anagram.Length,
                    found = false,
                    guessed = false,
                    next = headNode
                };

                headNode = newNode;
            }
            // return headNode;
        }
    }
}
