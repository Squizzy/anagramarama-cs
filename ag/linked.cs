namespace ag
{
    partial class Program
    {
        // node contains ???
        public class Node
        {
            public string? anagram;
            public int found;
            public int guessed;
            public int length;
            public Node? next;
        }

        // method to count the length of the linked list
        public int Length(Node nodeHead)
        {
            Node? current = new Node();
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
        public void Swap(Node nodeFrom, Node nodeTo)
        {
            string? word = nodeFrom.anagram;
            int len = nodeFrom.length;

            nodeFrom.anagram = nodeTo.anagram;
            nodeFrom.length = nodeTo.length;
            nodeTo.anagram = word;
            nodeTo.length = len;
        }

        // method to sort the list first alphabetically then by increasing word length
        public void Sort(Node nodeHead)
        {
            Node? left, right;
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
        // from linked.c
        public void DestroyAnswers(Node? nodeHead)
        {
            Node? current = nodeHead;
            Node? previous = nodeHead;

            while (current != null)
            {
                current.anagram = null;
                previous = current;
                current = current.next;
                previous = null;
            }
            nodeHead = null;
        }

        // method to add a new word as long as it is not a duplicate
        // from linked.c
        public void Push(Node headRef, string anagram)
        {
            Node? current = new Node();
            current = headRef;
            int len;
            bool duplicate = false;

            while (current != null)
            {
                if (!string.Equals(anagram, current.anagram))
                {
                    duplicate = true;
                    break;
                }
                current = current.next;
            }

            if (!duplicate)
            {
                Node newNode = new Node();
                len = anagram.Length;
                newNode.anagram = anagram;
                newNode.length = len;
                newNode.found = 0;
                newNode.guessed = 0;
                newNode.next = headRef;

                headRef = newNode;
            }

        }
    }
}
