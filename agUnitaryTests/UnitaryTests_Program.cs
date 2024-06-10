using SDL2;
//using SDL_image;
using System;


namespace ag.tests;
public partial class Tests
{
/// <summary>
/// tests of methods of:  program.cs
///
///     public void test_initialize_SDL_create_window_renderer_successfully()
///
/// Category is used to test the test individually using:
///     dotnet test --filter "FullyQualifiedName~ProgramTests&TestCategory=CategoryA" 
/// </summary>
 

    // Creating a new dlb_node with a character initializes its properties.
    [TestFixture]
    public class ProgramTests
    {
        [Test, Category("CategoryA")]
         public void test_initialize_SDL_create_window_renderer_successfully()
        {
            // Arrange
            string[] mainArgs = {""};
            mainArgs[0] = "path=..\\..\\..\\..\\ag\\";
            // Act
            Program.Main(mainArgs);

            // Assert
            // Check if SDL is initialized successfully
            Assert.That(SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_AUDIO | SDL.SDL_INIT_TIMER), Is.EqualTo(0));

            // Check if the window is created successfully
            IntPtr window = SDL.SDL_CreateWindow("Anagramarama",
                                                SDL.SDL_WINDOWPOS_UNDEFINED,
                                                SDL.SDL_WINDOWPOS_UNDEFINED,
                                                800, 600,
                                                SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
            Assert.IsNotNull(window);

            // Check if the renderer is created successfully
            IntPtr renderer = SDL.SDL_CreateRenderer(window,
                                                    -1,
                                                    SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
                                                    SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
            Assert.IsNotNull(renderer);
        }
    }
}