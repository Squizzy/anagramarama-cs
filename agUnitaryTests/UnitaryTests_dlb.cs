using System;
using System.IO;

namespace ag.tests;
public partial class Tests
{
/// <summary>
/// tests of methods of:  dlb.cs
///     public void test_dlb_node_create_node()
///     public void test_dlb_push()
///     public void test_dlb_create()
///     public void test_dlb_push_null_dlbHead()
///     public void test_dlb_push_empty_word()
///     public void test_dlb_create_empty_file()
///     public void test_dlb_create_empty_file()
///     
///  Category is used to test the test individually using:
///     dotnet test --filter "FullyQualifiedName~dlbTests&TestCategory=CategoryA" 
/// </summary>
 

    // Creating a new dlb_node with a character initializes its properties.
    [TestFixture]
    public class dlbTests
    {
        [Test, Category("CategoryA")]
        public void test_dlb_node_create_node()
        {
            // Arrange
            char c = 'a';

            // Act
            Program.dlb_node node = Program.Dlb_node_create_node(c);

            // Assert
            Assert.That(c, Is.EqualTo(node.letter));
            Assert.IsFalse(node.valid);
            Assert.IsNull(node.sibling);
            Assert.IsNull(node.child);
        }

        // Dlb_push method adds a new word to the dictionary linked list.
        [Test, Category("CategoryB")]
        public void test_dlb_push()
        {
            // Arrange
            Program.dlb_node dlbHead = null;
            string word = "test";

            // Act
            Program.Dlb_push(ref dlbHead, word);

            // Assert
            Assert.IsNotNull(dlbHead);
            Assert.That(dlbHead.letter, Is.EqualTo('t'));
            Assert.IsNotNull(dlbHead.child);
            Assert.That(dlbHead.child.letter, Is.EqualTo('e'));
            Assert.IsNotNull(dlbHead.child.child);
            Assert.That(dlbHead.child.child.letter, Is.EqualTo('s'));
            Assert.IsNotNull(dlbHead.child.child.child);
            Assert.That(dlbHead.child.child.child.letter, Is.EqualTo('t'));
            Assert.IsTrue(dlbHead.child.child.child.valid);
        }

        // Dlb_create method reads a file and creates a dictionary linked list.
        [Test, Category("CategoryC")]
        public void test_dlb_create()
        {
            // Arrange
            Program.dlb_node dlbHead = null;
            string filename = "./tmpTestdictionary.txt";

            if (File.Exists(filename)) File.Delete(filename);

            if (!File.Exists(filename))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filename))
                {
                    sw.WriteLine("abac");
                    sw.WriteLine("And");
                    sw.WriteLine("Welcome");
                }	
            }

            // Act
            Program.Dlb_create(ref dlbHead, filename);

            // Assert
            Assert.IsNotNull(dlbHead);
            Assert.That(dlbHead.letter, Is.EqualTo('a'));
            Assert.IsNotNull(dlbHead.child);
            Assert.That(dlbHead.child.letter, Is.EqualTo('b'));
            Assert.IsNotNull(dlbHead.child.child);
            Assert.That(dlbHead.child.child.letter, Is.EqualTo('a'));
            Assert.IsNotNull(dlbHead.child.child.child);
            Assert.That(dlbHead.child.child.child.letter, Is.EqualTo('c'));
            Assert.IsTrue(dlbHead.child.child.child.valid);

            if (File.Exists(filename)) File.Delete(filename);
        }

        // Dlb_push method handles null dlbHead.
        [Test, Category("CategoryD")]
        public void test_dlb_push_null_dlbHead()
        {
            // Arrange
            Program.dlb_node dlbHead = null;
            string word = "test";

            // Act
            Program.Dlb_push(ref dlbHead, word);

            // Assert
            Assert.IsNotNull(dlbHead);
            Assert.That(dlbHead.letter, Is.EqualTo('t'));
            Assert.IsNotNull(dlbHead.child);
            Assert.That(dlbHead.child.letter, Is.EqualTo('e'));
            Assert.IsNotNull(dlbHead.child.child);
            Assert.That(dlbHead.child.child.letter, Is.EqualTo('s'));
            Assert.IsNotNull(dlbHead.child.child.child);
            Assert.That(dlbHead.child.child.child.letter, Is.EqualTo('t'));
            Assert.IsTrue(dlbHead.child.child.child.valid);
        }

        // Dlb_push method handles empty word.
        [Test, Category("CategoryE")]
        public void test_dlb_push_empty_word()
        {
            // Arrange
            Program.dlb_node dlbHead = null;
            string word = "";

            // Act
            Program.Dlb_push(ref dlbHead, word);

            // Assert
            Assert.IsNull(dlbHead);
        }

        // Dlb_create method handles empty file.
        [Test, Category("CategoryF")]
        public void test_dlb_create_empty_file()
        {
            // Arrange
            Program.dlb_node dlbHead = null;
            string filename = "empty.txt";

            if (File.Exists(filename)) File.Delete(filename);

            if (!File.Exists(filename))
            {
                 using (StreamWriter sw = File.CreateText(filename))
                {
                }	
            }
            // Act
            Program.Dlb_create(ref dlbHead, filename);

            // Assert
            Assert.IsNull(dlbHead);

            if (File.Exists(filename)) File.Delete(filename);
        }

        [Test, Category("CategoryG")]
        public void test_dlb_lookup()
        {
            // Arrange
            Program.dlb_node dlbHead = null;
            string word = "hello";
            Program.Dlb_push(ref dlbHead, word);

            // Act
            bool result = Program.Dlb_lookup(dlbHead, word);

            // Assert
            Assert.IsTrue(result);
        }

/* Skipped as Dlb_walk is not needed in C# as malloc is not needed and memory is cleared automatically

        [Test, Category("CategoryH")]
        public void test_dlb_walk()
        {
            // Arrange
            Program.dlb_node node1 = new Program.dlb_node();
            node1.letter = 'a';
            node1.valid = true;
            Program.dlb_node node2 = new Program.dlb_node();
            node2.letter = 'b';
            node2.valid = false;
            Program.dlb_node node3 = new Program.dlb_node();
            node3.letter = 'c';
            node3.valid = true;
            Program.dlb_node node4 = new Program.dlb_node();
            node4.letter = 'd';
            node4.valid = false;
            Program.dlb_node node5 = new Program.dlb_node();
            node5.letter = 'e';
            node5.valid = true;

            node1.child = node2;
            node2.sibling = node3;
            node3.child = node4;
            node4.sibling = node5;

            List<Program.dlb_node> visitedNodes = new List<Program.dlb_node>();

            // Act
            Program.Dlb_walk(node1, (node) =>
            {
                visitedNodes.Add(node);
                return 0;
            });

            // Assert
            Assert.AreEqual(5, visitedNodes.Count);
            Assert.AreEqual(node1, visitedNodes[0]);
            Assert.AreEqual(node2, visitedNodes[1]);
            Assert.AreEqual(node3, visitedNodes[2]);
            Assert.AreEqual(node4, visitedNodes[3]);
            Assert.AreEqual(node5, visitedNodes[4]);
        }
*/


    }
}