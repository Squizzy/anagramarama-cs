namespace ag
{
    partial class Program
    {
        /// <summary>
        /// Node containing an anagram from the list of anagrams that can be made from the root word
        /// </summary>
        public class Node
        {
            /// <summary> The anagram word </summary>
            public string? anagram;
            /// <summary> This is marked if the user guessed, or if the game timed out and the game found it </summary>
            public bool found;
            /// <summary> This is marked if the user guessed </summary>
            public bool guessed;
            /// <summary> length of the word anagram of the node, used for counting points </summary>
            public int length;
            /// <summary> pointer to the next node </summary>
            public Node? next;
        }

    }
}