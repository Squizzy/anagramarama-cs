using SDL2;

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
///     dotnet test --filter "FullyQualifiedName~SpriteTests&TestCategory=CategoryA" 
/// </summary>
 

    [TestFixture]
    public class SpriteTests
    {
        // The 'Element' struct can be initialized with valid values.
        [Test, Category("CategoryA")]
        public void Test_Element_Initialized_With_Valid_Values()
        {
            // Arrange
            Program.Element element;

            // Act
            element.t = IntPtr.Zero;
            element.w = new SDL.SDL_Rect();
            element.x = 0;
            element.y = 0;

            // Assert
            Assert.That(element.t, Is.EqualTo(IntPtr.Zero));
            Assert.That(element.w, Is.EqualTo(new SDL.SDL_Rect()));
            Assert.That(element.x, Is.EqualTo(0));
            Assert.That(element.y, Is.EqualTo(0));
        }    

        //The 'Sprite' struct can be initialized with valid values.
        [Test, Category("CategoryB")]
        public void Test_Sprite_Initialized_With_Valid_Values()
        {
            // Arrange
            Program.Sprite sprite = new Program.Sprite(3);
            Program.Element element = new Program.Element();
            element.t = IntPtr.Zero;
            element.w = new SDL.SDL_Rect();
            element.x = 0;
            element.y = 0;

            // Act
            sprite.spr[0] = element;
            sprite.numSpr = 1;
            sprite.letter = 'A';
            sprite.x = 0;
            sprite.y = 0;
            sprite.w = 10;
            sprite.h = 10;
            sprite.next = new Program.Sprite();
            sprite.index = 0;
            sprite.box = 0;

            // Assert
            Assert.IsNotNull(sprite.spr);
            Assert.That(sprite.numSpr, Is.EqualTo(1));
            Assert.That(sprite.letter, Is.EqualTo('A'));
            Assert.That(sprite.x, Is.EqualTo(0));
            Assert.That(sprite.y, Is.EqualTo(0));
            Assert.That(sprite.w, Is.EqualTo(10));
            Assert.That(sprite.h, Is.EqualTo(10));
            Assert.IsNotNull(sprite.next);
            Assert.That(sprite.index, Is.EqualTo(0));
            Assert.That(sprite.box, Is.EqualTo(0));
        }


        // The 'destroyLetters' method can be called with a valid 'Sprite' object.
        // However this is not needed in C# as garbage collection is done automatically
        /*
        [Test, Category("CategoryC")]
        public void Test_DestroyLetters_With_Valid_Sprite()
        {
            // Arrange
            Program.Sprite sprite = new Program.Sprite();
            sprite.spr = new Program.Element();
            sprite.numSpr = 1;
            sprite.next = new Program.Sprite();

            // Act
            Program.destroyLetters(sprite);

            // Assert
            Assert.IsNull(sprite.spr);
            Assert.IsNull(sprite.next);
        }*/

    }
}