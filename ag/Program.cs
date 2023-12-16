//#define myDEBUG
//#define myDEBUGmacos
//#define meDEBUGfr

using System;
using System.Security.Principal;
using SDL2;
//using System.ComponentModel.Design.Serialization;
//using System.Formats.Asn1;
//using System.ComponentModel.DataAnnotations;
//using System.Security.Cryptography;
// using System.Reflection.Metadata;
// using System.IO.Compression;

namespace ag
{
    partial class Program
    {
        // method to identify the local language path for the locale files (dictionarity, background, ...)
        private static string DictPathLanguage()
        {
            string path = "i18n/";
            //if (!myDEBUGmacos) path = "i18n/"; 
            #if DEBUG
                path = "../../../" + path;
            #endif

            string lang;
            lang = System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag;
            //backup in case no locale was returned: en-GB
            if (lang == null) lang = "en-GB";
            #if myDEBUGfr
                lang = "fr-FR";
            #endif
            //To be extended with checks for "isValidLocale"?
            
            return path + lang + "/";
        }

        #if !UNITS_TESTS  // defined in ag.csproj
        public static void Main()
        {
            // initiate the reference to the first node for the list of anagrams
            Node head = new Node();
            head = null;

            // initiate the reference to the first node for the dictionary
            // (first letter to be gathered from the input file)
            dlb_node dlbHead = new dlb_node();  
            dlbHead = null;

            // initiate the reference to the first node for the sprites
            Sprite letters = new Sprite();
            letters = null;

            // initiate the reference to the linked list, ie the sequence of letters each stored in a new node.
            // dlb_linkedlist newLinkedList = new dlb_linkedlist(); 

            // find the local path with the IETF international code
            // TO CHECK - adjust for if using a command line, or using a configuration popup etc...
            string dictonaryPathLanguage = DictPathLanguage();

            // load the dictionary
            // newLinkedList.dlb_create(newNode, dictonaryPathLanguage + "wordlist.txt");
            Dlb_create(dlbHead, dictonaryPathLanguage + "wordlist.txt");
            
            // Initiate SDL
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_AUDIO | SDL.SDL_INIT_TIMER) < 0) 
            { 
                Console.WriteLine($"There is an issue with initialising SDL. {SDL.SDL_GetError()}"); 
            }
            else { Console.WriteLine($"SDL Init for Video, Audio, timer Init ok."); }
            
            // Create a new window given a title, size, and passes it a flag indicating it should be shown.
            IntPtr window = SDL.SDL_CreateWindow("Anagramarama", 
                                        SDL.SDL_WINDOWPOS_UNDEFINED, 
                                        SDL.SDL_WINDOWPOS_UNDEFINED, 
                                        800, 600, 
                                        SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
            if (window == null) Console.WriteLine($"There was an issue creating the 800x600 window. {SDL.SDL_GetError()}");  
                
            // Creates a new SDL hardware renderer in that window using the default graphics device with VSYNC enabled.
            IntPtr renderer = SDL.SDL_CreateRenderer(window, 
                                                    -1, 
                                                    SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | 
                                                    SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
            
            // Sets the renderer colour that the screen will be cleared with.
            if(SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255) <0) Console.WriteLine($"There was an issue with setting the render draw color. {SDL.SDL_GetError()}");

            // Clear the current render surface.
            if (SDL.SDL_RenderClear(renderer) <0) Console.WriteLine($"There was an issue with clearing the render surface. {SDL.SDL_GetError()}");

            // Switches out the currently presented render surface with the one we just did work on.
            SDL.SDL_RenderPresent(renderer);

            // cache in-game graphics
            // Load the background image as texture in SDL, and set it as the texture for background
            string backgroundImage = dictonaryPathLanguage + "images/background.png";
            backgroundTex = SDL.SDL_CreateTextureFromSurface(renderer, SDL_image.IMG_Load(backgroundImage));

            // Load the large letter bank, small letter bank, and number bank
            string letterBankImage = dictonaryPathLanguage + "images/letterBank.png";
            IntPtr letterBank = SDL.SDL_CreateTextureFromSurface(renderer, SDL_image.IMG_Load(letterBankImage));
            
            string smallLetterBankImage = dictonaryPathLanguage + "images/smallLetterBank.png";
            IntPtr smallLetterBank = SDL.SDL_CreateTextureFromSurface(renderer, SDL_image.IMG_Load(smallLetterBankImage));

            string numberImage = dictonaryPathLanguage + "images/numberBank.png";
            IntPtr numberBank = SDL.SDL_CreateTextureFromSurface(renderer, SDL_image.IMG_Load(numberImage));

            // TO CHECK - load from the config.ini

            newGame(head, dlbHead, renderer, letters);



            Console.ReadLine();
            Console.WriteLine("end");
        }
        #endif
    }
}

        