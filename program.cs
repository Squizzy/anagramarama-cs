// See https://aka.ms/new-console-template for more information

using System;

namespace ag
{
    internal class Node 
    {
        internal List<char> anagram = [];
        internal int found;
        internal int guessed;
        internal int length;
        internal Node next;
        public Node(List<char> d)
        {
            anagram = d;
            next = null;
        }
    }
    internal class SingleLinkedList {
        public Node First { get; set; }
    }

    class Program
    {
        Node newhead = new Node(null);
    }

}