//#define UNITS_TESTS
//#define TEST_DLB
//#define TEST_AG_CORE  // not finished
//#define TEST_LINKED // not finished

namespace ag
{
    partial class Program
    {
        #if UNITS_TESTS
        public static void Main()
        {
            dlb_node dictionaryHeadNode = new dlb_node();  // initiate the reference to the first node => dictionary (first letter to be gathered from the input file)
            string dictonaryPathLanguage = DictPathLanguage(); //find the local path with the IETF international code
            Dlb_create(dictionaryHeadNode, dictonaryPathLanguage + "wordlist.txt"); // load the dictionary   

            // Tests for ag_core
            #if TEST_AG_CORE
                /*string s = "ABC#DEF";
                Console.WriteLine("S: " + s);
                Console.WriteLine("Next Blank position: " + NextBlank(s));
                Console.WriteLine("ShiftLeftKill: " + ShitfLeftKill(s));
                Console.WriteLine("ShiftLeft: " + ShiftLeft(s));*/

                // test ag

                Node anagramsList = new Node();
                Node root = new Node();
                
                anagramsList = Ag(anagramsList, dictionaryHeadNode, "", "saturnin");
                // root = anagramsList; needed if I want to reuse the list
                Console.WriteLine("Anagrams Of \"toure\" created");
                do 
                {
                    Console.WriteLine(anagramsList.anagram);
                    anagramsList = anagramsList.next;
                } while (anagramsList.next != null) ;
                
                /*anagramsList = root;  // test to go through the list again
                do 
                {
                    Console.WriteLine(anagramsList.anagram);
                    anagramsList = anagramsList.next;
                } while (anagramsList.next != null) ;*/

                // test getRandomWord
                for (int i=0; i<5; i++) Console.WriteLine(GetRandomWord());
            #endif


            // tests for dlb
            #if (TEST_DLB || TEST_LINKED)
                //init - loads the english dictionary - tests dlb_node_create_node, dlb_push, dlb_create
                // dlb_node newNode = new dlb_node();  // initiate the reference to the first node (first letter to be gathered from the input file)
                //dlb_linkedlist newLinkedList = new dlb_linkedlist(); // initiate the reference to the linked list, ie the sequence of letters each stored in a new node.
                 // string dictonaryPathLanguage = DictPathLanguage(); //find the local path with the IETF international code
                // /*newLinkedList.*/Dlb_create(newNode, dictonaryPathLanguage + "wordlist.txt"); // load the dictionary           
                
                Console.WriteLine("Is word \"ASDF\" in the list: " + /*newLinkedList.*/Dlb_lookup(dictionaryHeadNode, "ASDF"));
                Console.WriteLine("Is word \"entity\" in the list: " + /*newLinkedList.*/Dlb_lookup(dictionaryHeadNode, "entity"));
            #endif

            // tests for linked
            #if TEST_LINKED
                //Console.WriteLine("Length of dictionary: " + newNode.);
            #endif

        }
        #endif
    }
}