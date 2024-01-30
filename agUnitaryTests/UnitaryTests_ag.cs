namespace ag.tests;
public partial class Tests
{
/// <summary>
/// tests performed in this file test ag.cs methods
///     
///     public void test_new_game()
///     public void test_shuffle_word()
///     public void test_ag()
///     
///     Category is used to test the test individually using:
///     dotnet test --filter "FullyQualifiedName~AgTests&TestCategory=CategoryA" 
/// </summary>
 

    // Creating a new dlb_node with a character initializes its properties.
    [TestFixture]
    public class AgTests
    {
    
/* TO BE ADDED WHEN COMPLETE
        
        [Test, Category("CategoryA")]
        // newGame method successfully generates a random 7-letter word from the dictionary and sets up the list of anagrams based on rootWord
        
        public void test_new_game()
        {
            // Arrange
            //Program program = new Program();
            Program.Node answers = new Program.Node();
            Program.dlb_node dict = new Program.dlb_node();
            IntPtr backgroundTex = new IntPtr();
            IntPtr screen = new IntPtr();
            Program.Sprite letters = new Program.Sprite();
            char[] rootWord = new char[7];
            string wordsListPath = "..\\..\\..\\..\\ag\\";

            // Act
            Program.newGame(ref answers, ref dict, backgroundTex, screen, letters, rootWord, wordsListPath);

            // Assert
            Assert.IsNotNull(rootWord);
            Assert.IsTrue(rootWord.Length == 7);
            Assert.IsNotNull(answers);
        }
*/

         [Test, Category("CategoryB")]
         // ShuffleWord method successfully shuffles the characters in a given word array
        public void test_shuffle_word()
        {
            // Arrange
            //Program program = new Program();
            char[] word = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };
            char[] originalWord = new char[word.Length];
            Array.Copy(word, originalWord, word.Length);

            // Act
            Program.ShuffleWord(word);

            // Assert
            Assert.AreNotEqual(originalWord, word);
        }
/*
        [Test, Category("CategoryC")]
        // Ag method successfully generates anagrams from the given guess and remaining letters

        public void test_ag()
        {
            // Arrange
            //Program program = new Program();
            Program.Node answers = new Program.Node();
            Program.dlb_node dict = new Program.dlb_node();
            string guess = "guess";
            string remain = "remain";

            // Act
            Program.Ag(ref answers, dict, guess, remain);

            // Assert
            Assert.IsNotNull(answers);
            Assert.IsTrue(answers.length > 0);
        }


        [Test, Category("CategoryD")]
        // GetRandomWord method fails to retrieve a random word from the dictionary
        // NOTE: Not really necessary as this would be useless to randomise a null wod
        public void test_get_random_word_failure()
        {
            // Arrange
            //Program program = new Program();
            string wordsListPath = "..\\..\\..\\..\\ag\\";

            // Act
            string randomWord = Program.GetRandomWord(wordsListPath);

            // Assert
            Assert.IsNull(randomWord);
        }*/
    }
}