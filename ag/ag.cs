using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using SDL2;



namespace ag
{

    public partial class Program
    {

        public enum HotBoxes { boxSolve, boxNew, boxQuit, boxShuffle, boxEnter, boxClear };

        public static Box[] hotbox = new Box[]
        {
            new Box() { x = 612, y =   0, width = 66, height = 30 },  /* boxSolve */  
            new Box() { x = 686, y =   0, width = 46, height = 30 },  /* boxNew */    
            new Box() { x = 742, y =   0, width = 58, height = 30 },  /* boxQuit */   
            new Box() { x = 618, y = 206, width = 66, height = 16 },  /* boxShuffle */
            new Box() { x = 690, y = 254, width = 40, height = 35 },  /* boxEnter */  
            new Box() { x = 690, y = 304, width = 40, height = 40 }   /* boxClear */
        };

        public static string[] boxnames = ["solve", "new", "quit", "shuffle", "enter", "clear"];

        // shuffle is an array that can be modified so it needs to have a field that is set and get
        /// <summary>Represents a shuffled array of characters.</summary>
        private static readonly char[] _shuffle = new char[8];

        /// <summary>Gets or sets the shuffled array of characters.</summary>
        public static char[] Shuffle
        {
            get { return _shuffle; }
            set
            {
                if (value != null && value.Length == _shuffle.Length)
                {
                    value.CopyTo(_shuffle, 0);
                }
                else
                {
                    throw new ArgumentException($"Array must be of length {_shuffle.Length}.");
                }
            }
        }

        // answer is an array that can be modified so it needs to have a field that is set and get
        /// <summary>Represents an answer array of characters.</summary>
        public static readonly char[] _answer = new char[8];

        /// <summary>Gets or sets the answer array of characters.</summary>
        public static char[] Answer
        {
            get { return _answer; }
            set
            {
                if (value != null && value.Length == _answer.Length)
                {
                    value.CopyTo(_answer, 0);
                }
                else
                {
                    throw new ArgumentException($"Array must be of length {_answer.Length}.");
                }
            }
        }

        /// <summary>language</summary>
        public static char[] language = new char[256];
        /// <summary>userPath</summary>
        public static char[] userPath = new char[256];
        /// <summary>basePath</summary>
        public static char[] basePath = new char[256];
        /// <summary>txt</summary>
        public static char[] txt = new char[256];
        /// <summary>rootword</summary>
        public static char[] rootword = new char[9];
        /// <summary>updateAnswers</summary>
        public static bool updateAnswers = false;
        /// <summary>startNewGame</summary>
        public static bool startNewGame = false;
        /// <summary>solvePuzzle</summary>
        public static bool solvePuzzle = false;
        /// <summary>shuffleRemaining</summary>
        public static bool shuffleRemaining = false;
        /// <summary>chearGuess</summary>
        public static bool clearGuess = false;

        /// <summary>gameStart</summary>
        public static DateTime gameStart;
        /// <value>The number of seconds elapsed since the begining of the game</value>
        public static int gameTime;
        /// <summary>stopTheclock</summary>
        public static bool stopTheClock = false;

        /// <summary>totalScore</summary>
        public static int totalScore = 0;
        /// <summary>score</summary>
        public static int score = 0;
        /// <summary>answersSought</summary>
        public static int answersSought = 0;
        /// <summary>answersGot</summary>
        public static int answersGot = 0;
        /// <value>The flag repesenting that the biggest word was found</value>
        public static bool gotBigWord = false;
        /// <value>The length of the biggest word</value>
        public static int bigWordLen = 0;
        /// <value>The flag representing that the score was updated</value>
        public static bool updateTheScore = false;
        /// <value>The flag representing that the game was paused</value>
        public static bool gamePaused = false;
        /// <value>The flag representing that a duplicate was found</value>
        public static bool foundDuplicate = false;
        /// <value>the flag representing that quitting the game was requested</value>
        public static bool quitGame = false;
        /// <value>The flag representing that the game was won</value>
        public static bool winGame = false;

        /// <value>The value representing the speed at which the letters move from one box to the other</value>
        public static int letterSpeed = LETTER_FAST;

        /// <value>The flag indicating the full screen selection</value>
        public static bool fullscreen = false;

        // SDL summarys
        /// <value>the SDL window</value>
        public static IntPtr window;

        /// <value>the SDL texture containing the background image</value>
        public static IntPtr backgroundTex = IntPtr.Zero;
        /// <value>The SDL texture containing all the large letters</value>
        public static IntPtr letterBank = IntPtr.Zero;
        /// <value>The SDL texture containing all the small letters</value>
        public static IntPtr smallLetterBank = IntPtr.Zero;
        /// <value>The SDL texture containing all the numbers</value>
        public static IntPtr numberBank = IntPtr.Zero;
        /// <summary>answerBoxUnknown</summary>
        public static IntPtr answerBoxUnknown = IntPtr.Zero;
        /// <summary>answerBoxKnown</summary>
        public static IntPtr answerBoxKnown = IntPtr.Zero;
        /// <value>The list of sprites containing the graphical time representation</value>
        public static Sprite? clockSprite = null;
        /// <value>The list of sprites containing the graphical score representation</value>
        public static Sprite? scoreSprite = null;

        // audio vars
        /// <value>The flag representing if the audio is enabled</value>
        public static bool audio_enabled = true;
        /// <summary>audio_len</summary>
        public static uint audio_len;
        /// <summary>audio_pos</summary>
        public static IntPtr audio_pos;

        /// <summary> defines the Sound class </summary>
        /// <remarks> Constructor</remarks>
        /// <param name="name">Name of the sound</param>
        /// <param name="audioChunk">pointer to the audio chunk</param>
        public class Sound(string? name, IntPtr audioChunk)
        {
            /// <value> Property <c>Name</c> name of the sound </value>
            public string? Name { get; set; } = name;
            /// <value> Property <c>audio_chunk</c> audio chunk </value>
            public IntPtr Audio_chunk { get; set; } = audioChunk;
            /// <value> Property <c>Next</c> next sound </value>
            public Sound? Next { get; set; } = null;
        }

        /// <summary>soundCache</summary>
        public static Sound? soundCache = new(null, IntPtr.Zero);


        // SKIPPED the Error and Debug functions of the original C as this is handled differently in C#


        /// <summary> Search through the list of sound names and return the corresponding audio chunk
        /// walk the module level soundCache until the requiredname is found.  
        /// when found, return the audio data
        /// if name is not found, return NULL instead.
        /// </summary>
        /// <param name="name">name - the unique id string of the required sound</param>
        /// <returns>a chunk of audio or NULL if not found</returns>
        public static IntPtr GetSound(string name)
        {
            Sound? currentSound = soundCache;

            while (currentSound != null)
            {
                if (currentSound.Name == name)
                {
                    return currentSound.Audio_chunk;
                }
                currentSound = currentSound.Next;
            }
            return IntPtr.Zero;
        }


        /// <summary> push a sound onto the soundCache </summary>
        /// <param name="soundCache">pointer to the head of the soundCache</param>
        /// <param name="name">unique id string for the sound</param>
        /// <param name="filename">the filename of the WAV file</param>
        /// <returns>Nothing</returns>
        public static void PushSound(ref Sound soundCache, string name, string filename)
        {
            Sound? thisSound = null;

            thisSound.Name = name;
            thisSound.Next = soundCache;

            thisSound.Audio_chunk = IntPtr.Zero;

            string tempFileName = new string(basePath);
            if ((tempFileName[0] != 0) && (tempFileName[tempFileName.Length] != '/'))
            {
                tempFileName += '/';
            }
            tempFileName += filename;

            thisSound.Audio_chunk = SDL_mixer.Mix_LoadWAV(tempFileName);

            soundCache = thisSound;
        }


        /// <summary> push all the game sounds onto the soundCache linked list.  
        /// Note that soundCache is passed into pushSound by reference, 
        /// so that the head pointer can be updated
        /// </summary>
        /// <param name="soundCache">the sound cache</param>
        /// <returns>Nothing</returns>
        public static void BufferSounds(ref Sound soundCache)
        {
            PushSound(ref soundCache, "click-answer", "audio/click-answer.wav");
            PushSound(ref soundCache, "click-shuffle", "audio/click-shuffle.wav");
            PushSound(ref soundCache, "foundbig", "audio/foundbig.wav");
            PushSound(ref soundCache, "found", "audio/found.wav");
            PushSound(ref soundCache, "clear", "audio/clearword.wav");
            PushSound(ref soundCache, "duplicate", "audio/duplicate.wav");
            PushSound(ref soundCache, "badword", "audio/badword.wav");
            PushSound(ref soundCache, "shuffle", "audio/shuffle.wav");
            PushSound(ref soundCache, "clock-tick", "audio/clock-tick.wav");
        }


        /// <summary> Free the memory of the sound chunks
        /// No longer needed in c# but kept for the sake of keeting
        /// </summary>
        /// <returns>Nothing</returns>
        public static void ClearSoundBuffer()
        {
            soundCache = null;
            // Sound currentSound = soundCache, previousSound = null;

            // while (currentSound != null)
            // {
            //     SDL_mixer.Mix_FreeChunk(currentSound.Audio_chunk);
            //     currentSound.Name = null;
            //     previousSound = currentSound;
            //     currentSound = currentSound.Next;
            //     previousSound = null;
            // }
        }


        /// <summary> load the named image to position x,y onto the required surface </summary>
        /// <param name="file">the filename to load (.BMP)</param>
        /// <param name="screen">the SDL_Surface to display the image</param>
        /// <returns>Nothing</returns>
        public static void ShowBMP(string file, IntPtr screen)
        {
            IntPtr imageSurf;
            IntPtr image;
            SDL.SDL_Rect dest;

            // load the BMP file into a surface
            imageSurf = SDL.SDL_LoadBMP(file);
            if (imageSurf == null)
            {
                Console.WriteLine("Couldn't load %s: %s\n", file, SDL.SDL_GetError());
            }
            dest.x = 0;
            dest.y = 0;
            dest.w = 800;
            dest.h = 600;
            image = SDL.SDL_CreateTextureFromSurface(screen, imageSurf);
            SDLScale_RenderCopy(screen, image, null, ref dest);

            SDL.SDL_FreeSurface(imageSurf);
            SDL.SDL_DestroyTexture(image);
        }


        /// <summary> Display the answer boxes (small boxes at the bottom I think) </summary>
        /// <param name="headNode">The head node of anagrams (with the info on if they have been found or guessed)</param>
        /// <param name="screen">The renderer</param>
        /// <returns>Nothing</returns>
        public static void DisplayAnswerBoxes(Node headNode, IntPtr screen)
        {
            Node current = headNode;
            SDL.SDL_Rect outerRect, innerRect, letterBankRect;
            int numWords = 0;
            int acrossOffset = 70;
            int numLetters = 0;
            int listLetters = 0;

            if (answerBoxUnknown == IntPtr.Zero)
            {
                outerRect.w = 16;
                outerRect.h = 16;
                outerRect.x = 0;
                outerRect.y = 0;
                IntPtr box = SDL.SDL_CreateRGBSurface(0, 16, 16, 32, 0, 0, 0, 0);
                SDL.SDL_FillRect(box, ref outerRect, 0);
                innerRect.w = outerRect.w - 1;
                innerRect.h = outerRect.h - 1;
                innerRect.x = outerRect.x + 1;
                innerRect.y = outerRect.y + 1;

                SDL.SDL_Surface surface = Marshal.PtrToStructure<SDL.SDL_Surface>(box);

                SDL.SDL_FillRect(box, ref innerRect, SDL.SDL_MapRGB(surface.format, 217, 220, 255));
                answerBoxUnknown = SDL.SDL_CreateTextureFromSurface(screen, box);

                SDL.SDL_FillRect(box, ref innerRect, SDL.SDL_MapRGB(surface.format, 255, 255, 255));
                answerBoxKnown = SDL.SDL_CreateTextureFromSurface(screen, box);
                SDL.SDL_FreeSurface(box);
            }

            // Width ad height are always the same
            outerRect.w = 16;
            outerRect.h = 16;
            outerRect.x = acrossOffset;
            outerRect.y = 380;

            letterBankRect.w = 10;
            letterBankRect.h = 16;
            letterBankRect.x = 0;
            letterBankRect.y = 0; // letter is chosen by 10x letter where a is 0

            while (current != null)
            {
                // new word
                numWords++;
                numLetters = 0;

                // update the x for each letter
                for (int i = 0; i < current.length; i++)
                {
                    numLetters++;
                    if (current.guessed)
                    {
                        SDLScale_RenderCopy(screen, answerBoxKnown, null, ref outerRect);
                    }
                    else
                    {
                        SDLScale_RenderCopy(screen, answerBoxUnknown, null, ref outerRect);
                    }

                    innerRect.w = outerRect.w - 1;
                    innerRect.h = outerRect.h - 1;
                    innerRect.x = outerRect.x + 1;
                    innerRect.y = outerRect.y + 1;

                    if (current.found)
                    {
                        int c = (int)(current.anagram[i] - 'a');

                        innerRect.x += 2;
                        letterBankRect.x = 10 * c;
                        innerRect.w = letterBankRect.w;
                        innerRect.h = letterBankRect.h;
                        SDLScale_RenderCopy(screen, smallLetterBank, letterBankRect, ref innerRect);
                    }
                    outerRect.x += 18;
                }

                if (numLetters > listLetters)
                {
                    listLetters = numLetters;
                }

                if (numWords == 11)
                {
                    numWords = 0;
                    acrossOffset += (listLetters * 18) + 9;
                    outerRect.y = 380;
                    outerRect.x = acrossOffset;
                }
                else
                {
                    outerRect.x = acrossOffset;
                    outerRect.y += 19;
                }

                current = current.next;
            }
        }


        /// <summary> Declare all all the anagrams as found (but not necessarily guessed)
        /// </summary>
        /// <param name="headNode">The head node of the anagrams list</param>
        /// <returns>Nothing</returns>
        public static void SolveIt(Node headNode)
        {
            Node current = headNode;

            while (current != null)
            {
                current.found = true;
                current = current.next;
            }
        }


        /// <summary> Check if the guess is a word that is to be found </summary>
        /// <param name="answer">the word proposed</param>
        /// <param name="headNode">The head node of anagrams</param>
        /// <returns>Nothing</returns>
        public static void CheckGuess(string answer, Node headNode)
        {
            Node current = headNode;
            bool foundWord = false;
            bool foundAllLengths = true;
            char[] test = new char[8];

            int len = NextBlank(answer) - 1;
            if (len == -1)
            {
                len = test.Length - 1;
            }
            for (int i = 0; i < len; i++)
            {
                test[i] = answer[i];
            }

            while (current != null)
            {
                if (current.anagram == new string(test))
                {
                    if (!current.found)
                    {
                        score += current.length;
                        totalScore += current.length;
                        answersGot++;
                        if (len == bigWordLen)
                        {
                            gotBigWord = true;
                            if (audio_enabled)
                            {
                                SDL_mixer.Mix_PlayChannel(-1, GetSound("foundbig"), 0);
                            }
                            else
                            {
                                if (audio_enabled)
                                {
                                    // just a normal word
                                    SDL_mixer.Mix_PlayChannel(-1, GetSound("found"), 0);
                                }
                            }
                        }
                        if (answersSought == answersGot)
                        {
                            // getting all answers gives us the game score again!!
                            totalScore += score;
                            winGame = true;
                        }
                        current.found = true;
                        current.guessed = true;
                        updateTheScore = true;
                    }
                    else
                    {
                        foundDuplicate = true;
                        if (audio_enabled)
                        {
                            SDL_mixer.Mix_PlayChannel(-1, GetSound("duplicate"), 0);
                        }
                    }
                    updateAnswers = true;
                    break;
                }
                current = current.next;
            }

            current = headNode;

            while (current != null)
            {
                if ((!current.found) && (len == current.anagram.Length))
                {
                    foundAllLengths = false;
                }
                current = current.next;
            }

            if (!foundWord)
            {
                if (audio_enabled)
                {
                    SDL_mixer.Mix_PlayChannel(-1, GetSound("badword"), 0);
                }
            }
        }


        /// <summary> determine the next blank space in a string 
        /// blanks are indicated by pound not space.
        /// When a blank is found, move the chosen letter from one box to the other.
        /// i.e. If we're using the ANSWER box, 
        ///   - move the chosen letter from the SHUFFLE box to the ANSWER box 
        ///   - and move a SPACE back to the SHUFFLE box. 
        /// and if we're using the SHUFFLE box 
        ///  - move the chosen letter from ANSWER to SHUFFLE 
        ///  - and move a SPACE into ANSWER.
        /// </summary>
        /// <param name="box">the ANSWER or SHUFFLE box</param>
        /// <param name="index">pointer to the letter we're interested in</param>
        /// <returns>the coords of the next blank position</returns>
        public static int NextBlankPosition(int box, int index)
        {
            int i = 0;

            switch (box)
            {
                case ANSWER:
                    for (i = 0; i < 7; i++)
                    {
                        if (Answer[i] == SPACE_CHAR)
                        {
                            break;
                        }
                    }
                    Answer[i] = Shuffle[index];
                    Shuffle[index] = SPACE_CHAR;
                    break;

                case SHUFFLE:
                    for (i = 0; i < 7; i++)
                    {
                        if (Shuffle[i] == SPACE_CHAR)
                        {
                            break;
                        }
                    }
                    Shuffle[i] = Answer[index];
                    Answer[index] = SPACE_CHAR;
                    break;

                default:
                    break;
            }

            index = i;

            return i * (GAME_LETTER_WIDTH + GAME_LETTER_SPACE) + BOX_START_X;
        }


        /// <summary> handle the keyboard events:
        ///  - BACKSPACE and ESCAPE - clear letters
        ///  - RETURN - check guess
        ///  - SPACE - shuffle
        /// - a-z - select the first instance of that letter in the shuffle box and move to the answer box
        /// </summary>
        /// <param name="SDLevent">The event from the keyboard</param>
        /// <param name="headNode">the head node of the anagrams</param>
        /// <param name="letters">The letters</param>
        /// <returns>Nothing</returns>
        public static void HandleKeyboardEvent(SDL.SDL_Event SDLevent, Node headNode, Sprite letters)
        {
            Sprite current = letters;
            var keyedLetter = SDLevent.key.keysym.sym;
            int maxIndex = 0;

            if (keyedLetter == SDL.SDL_Keycode.SDLK_F1)
            {
                if (!fullscreen)
                {
                    SDL.SDL_SetWindowFullscreen(window, (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN);
                }
                else
                {
                    SDL.SDL_SetWindowFullscreen(window, 0);
                }
                fullscreen = !fullscreen;
            }
            else if (!gamePaused)
            {
                switch (keyedLetter)
                {
                    case SDL.SDL_Keycode.SDLK_ESCAPE:
                        // clear has been pressed
                        clearGuess = true;
                        break;

                    case SDL.SDL_Keycode.SDLK_BACKSPACE:
                        while (current != null && current.box != CONTROLS)
                        {
                            current = current.next;
                        }

                        current = letters;
                        while (current != null)
                        {
                            if (current.box == ANSWER && current.index == maxIndex)
                            {
                                current.toX = NextBlankPosition(SHUFFLE, current.index);
                                current.toY = SHUFFLE_BOX_Y;
                                current.box = SHUFFLE;
                                if (audio_enabled)
                                {
                                    SDL_mixer.Mix_PlayChannel(-1, GetSound("click-answer"), 0);
                                }
                                break;
                            }
                            current = current.next;
                        }
                        break;

                    case SDL.SDL_Keycode.SDLK_RETURN:
                        // enter has been pressed
                        CheckGuess(new string(Answer), headNode);
                        break;

                    case SDL.SDL_Keycode.SDLK_SPACE:
                        // shuffle has been pressed
                        shuffleRemaining = true;
                        if (audio_enabled)
                        {
                            SDL_mixer.Mix_PlayChannel(-1, GetSound("shuffle"), 0);
                        }
                        break;

                    default:
                        // loop round until we find the first instance of the  selected letter in SHUFFLE
                        while (current != null && current.box != CONTROLS)
                        {
                            if (current.box == SHUFFLE)
                            {
                                current.toX = NextBlankPosition(ANSWER, current.index);
                                current.toY = ANSWER_BOX_Y;
                                current.box = ANSWER;
                                if (audio_enabled)
                                {
                                    SDL_mixer.Mix_PlayChannel(-1, GetSound("click-shuffle"), 0);
                                }
                                break;
                            }
                        }
                        current = current.next;
                        break;
                }
            }
        }


        /// <summary> Returns a boolean indicating whether the click was inside a box </summary>
        /// <param name="box"> The box </param>
        /// <param name="x"> the x position clicked </param>
        /// <param name="y">The y position clicked</param>
        /// <returns>true if clicked inside the box, false otherwise</returns>
        public static bool IsInside(Box box, int x, int y)
        {
            return (x > box.x) && (x < (box.x - box.width)) && (y > box.y) && (y < (box.y = box.height));
        }


        /// <summary> checks where the mouse click occurred
        ///  - if it's in a defined hotspot then perform the appropriate action
	    /// Hotspot	        Action
	    /// -----------------------------------------------------
	    /// A letter		set the new x,y of the letter and play the appropriate sound
        /// 
	    /// ClearGuess		set the clearGuess flag
        /// 
	    /// checkGuess		pass the current answer to the checkGuess routine
        /// 
	    /// solvePuzzle		set the solvePuzzle flag
        /// 
	    /// shuffle		    set the shuffle flag and play the appropriate sound
        /// 
	    /// newGame		    set the newGame flag
        /// 
	    /// quitGame		set the quitGame flag
        /// </summary>
        /// <param name="button">mouse button that has been clicked</param>
        /// <param name="x">the x coords of the mouse</param>
        /// <param name="y">the y coords of the mouse</param>
        /// <param name="screen">the SDL_Surface to display the image</param>
        /// <param name="headNode">pointer to the top of the answers list</param>
        /// <param name="letters">pointer to the letters sprites</param>
        /// <returns>Nothing</returns>
        public static void ClickDetect(int button, int x, int y, IntPtr screen, Node headNode, Sprite letters)
        {
            Sprite current = letters;

            if (!gamePaused)
            {
                while (current != null && current.box != CONTROLS)
                {
                    if (x >= current.x && x <= (current.x + current.w) && y >= current.y && y <= (current.y + current.h))
                    {
                        if (current.box == SHUFFLE)
                        {
                            current.toX = NextBlankPosition(ANSWER, current.index);
                            current.toY = ANSWER_BOX_Y;
                            current.box = ANSWER;
                            if (audio_enabled)
                            {
                                SDL_mixer.Mix_PlayChannel(-1, GetSound("click-shuffle"), 0);
                            }
                        }
                        else
                        {
                            current.toX = NextBlankPosition(SHUFFLE, current.index);
                            current.toY = SHUFFLE_BOX_Y;
                            current.box = SHUFFLE;
                            if (audio_enabled)
                            {
                                SDL_mixer.Mix_PlayChannel(-1, GetSound("click-shuffle"), 0);
                            }
                        }
                        break;
                    }
                    current = current.next;
                }

                if (IsInside(hotbox[(int)HotBoxes.boxClear], x, y))
                {
                    //clear has been pressed
                    clearGuess = true;
                }

                if (IsInside(hotbox[(int)HotBoxes.boxEnter], x, y))
                {
                    //enter has been pressed
                    CheckGuess(new string(Answer), headNode);
                }

                if (IsInside(hotbox[(int)HotBoxes.boxSolve], x, y))
                {
                    //solve has been pressed
                    solvePuzzle = true;
                }

                if (IsInside(hotbox[(int)HotBoxes.boxShuffle], x, y))
                {
                    shuffleRemaining = true;
                    if (audio_enabled)
                    {
                        SDL_mixer.Mix_PlayChannel(-1, GetSound("shuffle"), 0);
                    }
                }
            }

            if (IsInside(hotbox[(int)HotBoxes.boxNew], x, y))
            {
                startNewGame = true;
            }

            if (IsInside(hotbox[(int)HotBoxes.boxQuit], x, y))
            {
                quitGame = true;
            }
        }


        // TODO: Clarify what the return is
        /// <summary> move all letters from answer to shuffle </summary>
        /// <param name="letters">the letter sprites</param>
        /// <returns>the count of??? letters cleared, used to play a sound if not null??</returns>
        public static int ClearWord(Sprite letters)
        {
            Sprite? current = letters;
            Sprite[] orderedLetters = new Sprite[7];
            int count = 0;

            // There is a constructor so not needed
            // for (int i = 0; i < orderedLetters.Length / orderedLetters[0].Length; i++)
            // {
            //     orderedLetters[i] = null;
            // }

            while (current != null)
            {
                if (current.box == ANSWER)
                {
                    count++;
                    orderedLetters[current.index] = current;
                    current.toX = SHUFFLE_BOX_Y;
                    current.box = SHUFFLE;
                }
                current = current.next;
            }

            for (int i = 0; i < 7; i++)
            {
                if (orderedLetters[i] != null)
                {
                    orderedLetters[i].toX = NextBlankPosition(SHUFFLE, orderedLetters[i].index);
                }
            }

            return count;
        }


        /// <summary>Displays the score graphically</summary>
        /// <param name="screen">the SDL_Surface to display the image</param>
        /// <returns>Nothing</returns>
        private static void UpdateScore(IntPtr screen)
        {
            SDL.SDL_Rect fromRect, toRect, blankRect;

            blankRect.x = SCORE_WIDTH * 11;
            blankRect.y = 0;
            blankRect.w = SCORE_WIDTH;
            blankRect.h = SCORE_HEIGHT;

            fromRect.x = 0;
            fromRect.y = 0;
            fromRect.w = SCORE_WIDTH;
            fromRect.h = SCORE_HEIGHT;

            toRect.y = 0;
            toRect.w = SCORE_WIDTH;
            toRect.h = SCORE_HEIGHT;

            string buffer = totalScore.ToString();

            for (int i = 0; i < buffer.Length; i++)
            {
                fromRect.x = SCORE_WIDTH * ((int)buffer[i] - 48);
                toRect.x = SCORE_WIDTH * i;
                scoreSprite.sprite[i].sprite_band_dimensions = fromRect;
                scoreSprite.sprite[i].sprite_x_offset = toRect.x;
            }
        }


        /// <summary>Displays the time graphically</summary>
        /// <param name="screen">the SDL_Surface on which to display the image</param>
        /// <returns>Nothing</returns>
        public static void UpdateTime(IntPtr screen)
        {
            SDL.SDL_Rect fromRect;
            fromRect.x = 0;
            fromRect.y = 0;
            fromRect.w = CLOCK_WIDTH;
            fromRect.h = CLOCK_HEIGHT;

            int thisTime = AVAILABLE_TIME - gameTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(thisTime);

            int minutes = timeSpan.Minutes;
            int minutesTens = minutes / 10;
            int minutesUnits = minutes % 10;
            int seconds = timeSpan.Seconds;
            int secondsTens = seconds / 10;
            int secondsUnits = seconds % 10;

            fromRect.x = CLOCK_WIDTH * minutesTens;
            clockSprite.sprite[0].sprite_band_dimensions = fromRect;
            fromRect.x = CLOCK_WIDTH * minutesUnits;
            clockSprite.sprite[1].sprite_band_dimensions = fromRect;
            fromRect.x = CLOCK_WIDTH * secondsTens;
            clockSprite.sprite[3].sprite_band_dimensions = fromRect;
            fromRect.x = CLOCK_WIDTH * secondsUnits;
            clockSprite.sprite[4].sprite_band_dimensions = fromRect;
        }


        /// <summary> Shuffles the characters in a given word array. </summary>
        /// <param name="word">The word array to be shuffled.</param>
        /// <remarks>
        /// This method generates a random number between 20 and 26, and then swaps two characters in the word array
        /// for the generated number of times. The characters to be swapped are randomly selected using the Random class.
        /// </remarks>
        /// <returns>Nothing, the word is passed by reference</returns>
        public static void ShuffleWord(ref char[] word)
        {
            int a, b;
            char tmp;
            Random random = new Random();

            // generate a random number between 20 and 26. The rand() function in C no longer exists in C#
            // This was done in the initial c app, not sure why. Probably to increase the randomness result?
            int count = random.Next(20, 27);

            for (int n = 0; n < count; n++)
            {
                a = random.Next(0, 7);
                b = random.Next(0, 7);
                tmp = word[a];
                word[a] = word[b];
                word[b] = tmp;
            }

            // char tmp;

            // Random randCount = new Random();
            // int count = randCount.Next(20, 27);

            // // generate two random values, using the same random generator to prevent possible repeated values.
            // Random randPos = new Random();
            // int a = randPos.Next(0, 7);
            // int b = randPos.Next(0, 7);

            // for (int n=0; n < count; ++n )
            // {
            //     a = randPos.Next(0, 7);
            //     b = randPos.Next(0, 7);
            //     tmp = word[a];
            //     word[a] = word[b];
            //     word[b] = tmp;
            // }

        }


        /// <summary>Returns the index of first occurrence of a specific letter in a string.</summary>
        /// <param name="word">The word to check</param>
        /// <param name="letter">The char to find in the word</param>
        /// <returns>The position of the letter if it is found or -1 if not</returns>
        public static int WhereInString(string word, char letter)
        {
            int pos = word.IndexOf(letter);
            return pos != -1 ? pos : 0;
        }


        /// <summary> shuffle word, but also the Sprite letter </summary>
        /// <param name="word">The word to shuffle</param>
        /// <param name="letters">The sprite letters to shuffle at the same time</param>
        /// <returns>Nothing as passed by reference</returns>
        public static void ShuffleAvailableLetters(ref string word, ref Sprite letters)
        {
            Sprite thisLetter = letters;
            int from, to;
            char swap, posSwap;
            // char[] shuffleChars = new char[8];
            char[] shuffleChars;

            char[] shufflePos = new char[8];
            int numSwaps;

            Random random = new Random();


            for (int i = 0; i < 7; i++)
            {
                shufflePos[i] = (char)(i + 1);
            }
            shufflePos[7] = '\0';

            shuffleChars = word.ToCharArray();

            numSwaps = random.Next(20, 30);

            for (int i = 0; i < numSwaps; i++)
            {
                from = random.Next(0, 7);
                to = random.Next(0, 7);

                swap = shuffleChars[from];
                shuffleChars[from] = shuffleChars[to];
                shufflePos[to] = swap;

                posSwap = shufflePos[from];
                shufflePos[from] = shufflePos[to];
                shufflePos[to] = posSwap;
            }

            while (thisLetter != null)
            {
                if (thisLetter.box == SHUFFLE)
                {
                    thisLetter.toX = (WhereInString(new string(shufflePos), (char)(thisLetter.index + 1)) * (GAME_LETTER_WIDTH + GAME_LETTER_SPACE)) + BOX_START_X;
                    thisLetter.toY = WhereInString(new string(shufflePos), (char)(thisLetter.index + 1));
                }
                thisLetter = thisLetter.next;
            }
            word = new string(shuffleChars);
        }


        /// <summary>Build letter string into linked list of letter graphics </summary>
        /// <param name="letters">letter sprites head node (in/out)</param>
        /// <param name="screen">SDL_Surface to display the image</param>
        /// <returns>Nothing</returns>
        public static void BuildLetters(ref Sprite letters, IntPtr screen)
        {
            Sprite thisLetter = null, previousLetter = null;

            SDL.SDL_Rect rect;
            int index = 0;

            Random random = new Random();

            rect.y = 0;
            rect.w = GAME_LETTER_WIDTH;
            rect.h = GAME_LETTER_HEIGHT;

            int len = Shuffle.Length;

            for (int i = 0; i < len; i++)
            {
                thisLetter.numSpr = 0;

                if (Shuffle[i] != ASCII_SPACE && Shuffle[i] != SPACE_CHAR)
                {
                    int chr = (int)(Shuffle[i] - 'a');
                    rect.x = chr * GAME_LETTER_WIDTH;
                    thisLetter.numSpr = 1;

                    thisLetter.sprite[0].sprite_band_texture = letterBank;
                    thisLetter.sprite[0].sprite_band_dimensions = rect;
                    thisLetter.sprite[0].sprite_x_offset = 0;
                    thisLetter.sprite[0].sprite_y_offset = 0;

                    thisLetter.x = random.Next(800);/*i * (GAME_LETTER_WIDTH + GAME_LETTER_SPACE) + BOX_START_X;*/
                    thisLetter.y = random.Next(600); /* SHUFFLE_BOX_Y; */
                    thisLetter.letter = Shuffle[i];
                    thisLetter.h = GAME_LETTER_HEIGHT;
                    thisLetter.w = GAME_LETTER_WIDTH;
                    thisLetter.toX = i * (GAME_LETTER_WIDTH + GAME_LETTER_SPACE) + BOX_START_X;
                    thisLetter.toY = SHUFFLE_BOX_Y;
                    thisLetter.next = previousLetter;
                    thisLetter.box = SHUFFLE;
                    thisLetter.index = index++;

                    previousLetter = thisLetter;

                    letters = thisLetter;

                    thisLetter = null;
                }
                else
                {
                    Shuffle[i] = SPACE_CHAR;
                    // rect.x = 26 * GAME_LETTER_WIDTH;
                }

            }
        }


        /// <summary>add the clock to the sprites
        /// keep a module reference to it for quick and easy update
        /// this sets the clock to a fixed 5:00 start 
        /// </summary>
        /// <param name="letters">letter sprites head node (in/out)</param>
        /// <param name="screen">SDL_Surface to display the image</param>
        /// <returns>Nothing</returns>
        public static void AddClock(ref Sprite letters, IntPtr screen)
        {
            Sprite? thisLetter = new Sprite(5);
            Sprite? previousLetter = null;
            Sprite? current = letters;
            int index = 0;

            SDL.SDL_Rect fromRect;
            fromRect.x = 0; // probably unnecessary!
            fromRect.y = 0;
            fromRect.w = CLOCK_WIDTH;
            fromRect.h = CLOCK_HEIGHT;

            while (current != null)
            {
                previousLetter = current;
                current = current.next;
            }

            thisLetter.numSpr = 5;

            // initialise with 05:00
            // TODO: Probably could be done better - as in using the "AvailableTime" value
            for (int i = 0; i < 5; i++)
            {

                switch (i)
                {
                    case 0: // tens of mins ("0")
                        fromRect.x = 0;
                        break;
                    case 1:
                        fromRect.x = 5 * CLOCK_WIDTH; // units of minutes ("5")
                        break;
                    case 2:
                        fromRect.x = 10 * CLOCK_WIDTH; // colon (":")
                        break;
                    case 3: // tens of secs ("0")
                        fromRect.x = 0;
                        break;
                    case 4: // units of secs ("0")
                        fromRect.x = 0;
                        break;
                    default:
                        break;
                }

                thisLetter.sprite[i].sprite_band_texture = numberBank;
                thisLetter.sprite[i].sprite_band_dimensions = fromRect;
                thisLetter.sprite[i].sprite_x_offset = CLOCK_WIDTH * i;
                thisLetter.sprite[i].sprite_y_offset = 0;
            }

            thisLetter.x = CLOCK_X;
            thisLetter.y = CLOCK_Y;
            thisLetter.h = CLOCK_HEIGHT;
            thisLetter.w = CLOCK_WIDTH * thisLetter.numSpr;
            thisLetter.toX = thisLetter.x;
            thisLetter.toY = thisLetter.y;
            thisLetter.next = null;
            thisLetter.box = CONTROLS;
            thisLetter.index = index++;

            previousLetter.next = thisLetter;
            clockSprite = thisLetter;

        }


        /// <summary> add the Score to the sprites
        ///  a module reference to it for quick and easy update
        /// 
        /// </summary>
        /// <param name="letters">letter sprites head node (in/out)</param>
        /// <param name="screen">SDL_Surface to display the image</param>
        /// <returns>Nothing</returns>
        public static void AddScore(ref Sprite letters, IntPtr screen)
        {
            Sprite? thisLetter = new Sprite(5);
            Sprite? previousLetter = null;
            Sprite? current = letters;

            SDL.SDL_Rect fromRect;   // dimensions of the numbers band image in px
            SDL.SDL_Rect toRect;     // position of the letter in the score box

            int index = 0;

            fromRect.x = 0; // probably not needed?
            fromRect.y = 0;
            fromRect.w = SCORE_WIDTH;
            fromRect.h = SCORE_HEIGHT;

            toRect.y = 0;
            toRect.w = SCORE_WIDTH;
            toRect.h = SCORE_HEIGHT;

            while (current != null)
            {
                previousLetter = current;
                current = current.next;
            }

            // pre-loading: "    0"
            for (int i = 0; i < 5; i++)
            {
                if (i == 0) // rightmost? number "0"
                {
                    fromRect.x = 0;
                }
                else // space " "
                {
                    fromRect.x = SCORE_WIDTH * 11;
                }

                toRect.x = SCORE_WIDTH * i;

                thisLetter.sprite[i].sprite_band_texture = numberBank;
                thisLetter.sprite[i].sprite_band_dimensions = fromRect;
                thisLetter.sprite[i].sprite_x_offset = toRect.x;
                thisLetter.sprite[i].sprite_y_offset = 0;
            }

            thisLetter.x = SCORE_X;
            thisLetter.y = SCORE_Y;
            thisLetter.h = SCORE_HEIGHT;
            thisLetter.w = SCORE_WIDTH * 5; // initialise the first score on the RHS of the box
            thisLetter.toX = thisLetter.x;
            thisLetter.toY = thisLetter.y;
            thisLetter.next = null;
            thisLetter.box = CONTROLS;
            thisLetter.index = index++;

            previousLetter.next = thisLetter;
            scoreSprite = thisLetter;
        }


        /// <summary> Do all of the initialisation for a new game:
        /// build the screen
        /// get a random word and generate anagrams
        /// (must get less than 66 anagrams to display on screen)
        /// initialise all the game control flags
        /// </summary>
        /// <param name="headNode">first node in the answers list (in/out)</param>
        /// <param name="dlbHeadNode">first node in the dictionary list</param>
        /// <param name="screen">SDL_Surface to display the image</param>
        /// <param name="letters">first node in the letter sprites (in/out)</param>
        /// <returns>Nothing</returns>
        public static void NewGame(ref Node headNode, Dlb_node dlbHeadNode, IntPtr screen, ref Sprite letters)
        {
            // letters in the guess box
            char[] guess = new char[9];
            char[] remain = new char[9];
            // happy is true if we have < 67 anagrams and => 6
            bool happy = false;

            int answerSought = 0;
            int bigWordLen = 0;

            SDL.SDL_Rect dest;
            dest.x = 0;
            dest.y = 0;
            dest.w = 800;
            dest.h = 600;
            SDLScale_RenderCopy(screen, backgroundTex, null, ref dest);

            DestroyLetters(ref letters);


            //IntPtr temp = BackgroundText

            // SDL.SDL_Rect firstrect;
            // firstrect.x = 0;
            // firstrect.y = 0;
            // firstrect.w = 800;
            // firstrect.h = 600;

            // SDLScale_RenderCopy(screen, backgroundTex, ref firstrect, ref dest);

            //destroyLetters(letters);
            // Console.WriteLine("About to look for Happy");


            while (!happy)
            {
                char[] buffer = new char[9];
                string bufferString = new string(buffer);
                GetRandomWord(ref bufferString, buffer.Length);
                rootword = bufferString.ToCharArray();
                bigWordLen = rootword.Length;
                remain = rootword;

                DestroyAnswers(headNode);

                string guessString = new string(guess);
                string remainString = new string(remain);
                Ag(ref headNode, dlbHeadNode, guessString, remainString);


                answerSought = Length(headNode);

                happy = (answerSought <= 77) && (answerSought >= 6);

            }

            /* now we have a good set of words - sort them alphabetically */
            Sort(ref headNode);

            for (int i = bigWordLen; i < 7; i++)
            {
                remain[i] = SPACE_CHAR;
            }
            remain[7] = '0';  // might be superfluous with C#
            remain[bigWordLen] = '\0'; // might be superfluous with C#
            ShuffleWord(ref remain);
            Shuffle = remain;

            Answer = SPACE_FILLED_CHARS.ToCharArray();

            /* build up the letter sprites */

            BuildLetters(ref letters, screen);
            AddClock(ref letters, screen);
            AddScore(ref letters, screen);

            /* display all answer boxes */
            DisplayAnswerBoxes(headNode, screen);

            gotBigWord = false;
            score = 0;
            updateTheScore = true;
            gamePaused = false;
            winGame = false;
            answersGot = 0;

            gameStart = DateTime.Now;
            gameTime = 0;
            stopTheClock = false;
            // }


            //             // buffer = GetRandomWord(wordsListPath).ToCharArray();
            //             //guess = "".ToCharArray();
            //             // rootWord = buffer;
            //             // bigWordLen = rootWord.Length - 1;

            //             for (int i = 0; i < rootWord.Length; i++) remain[i] = rootWord[i];
            //                 //remain =rootWord;


            //                 // Not needed in C# as garbage collection is handled already
            //                 //destroyAnswers(answers);

            //                 answerSought = Length(answers);
            //                 string newGuessString = new string(guess).Trim('\0');
            //                 string newRemainString = new string(remain).Trim('\0');
            //                 Ag(ref answers, dict, newGuessString, newRemainString);
            //                 guess = newGuessString.ToCharArray();
            //                 char[] rem = newRemainString.ToCharArray();
            //                 for (int i = 0; i < newRemainString.Length; i++) remain[i] = rem[i];
            //                 //remain = newRemainString.ToCharArray();

            //                 answerSought = Length(answers);

            //                 // happy if the number of anagrams are 6 or more, and less than 77
            //                 happy = ((answerSought < 77) && (answerSought >= 6));

            // #if DEBUG
            //                 if (!happy) Console.WriteLine($"Too Many Answers!  word: {new string(rootWord)}, answers: {answerSought}");
            // #endif
            //             }
            //             Console.WriteLine("Happy found");
            // #if DEBUG
            //             if (happy) Console.WriteLine($"Selected word: {new string(rootWord)}, answers: {answerSought}");
            // #endif

            //             Sort(ref answers);

            //             for (int i = bigWordLen; i < 7; i++)
            //             {
            //                 remain[i] = SPACE_CHAR;
            //             }
            //             remain[7] = '\0';
            //             remain[bigWordLen] = '\0';

            //             ShuffleWord(remain);
            //             shuffle = remain;
            //             answer = SPACE_FILLED_STRING;

            //             // HERE

        }



        /// <summary> Callback method for SDL timer events
        /// Attempt at rewrite of the timer callback from the original C
        /// </summary>
        /// <param name="interval">The interval in milliseconds for the timer.</param>
        /// <param name="param">Additional parameters for the callback (unused).</param>
        /// <returns>The interval for the next timer event.</returns>        

        public static uint TimerCallBack(uint interval, IntPtr param)
        {
            SDL.SDL_UserEvent userEvent = new SDL.SDL_UserEvent()
            {
                type = (uint)SDL.SDL_EventType.SDL_USEREVENT,
                code = 0,
                data1 = IntPtr.Zero,
                data2 = IntPtr.Zero,
            };

            SDL.SDL_Event sdlEvent = new SDL.SDL_Event()
            {
                type = SDL.SDL_EventType.SDL_USEREVENT,
                user = userEvent,
            };

            SDL.SDL_PushEvent(ref sdlEvent);

            return interval;
        }


        /// <summary>
        /// a big while loop that runs the full length of the game, 
        /// checks the game events and responds accordingly
        ///
        /// event		    action
        /// -------------------------------------------------
        /// winGame	        stop the clock and solve puzzle
        /// timeRemaining   update the clock tick
        /// timeUp	        stop the clock and solve puzzle
        /// solvePuzzle	    trigger solve puzzle and stop clock
        /// updateAnswers   trigger update answers
        /// startNew        trigger start new
        /// updateScore	    trigger update score
        /// shuffle	        trigger shuffle
        /// clear		    trigger clear answer
        /// quit		    end loop
        /// poll events     check for keyboard/mouse and quit
        ///
        /// finally, move the sprites - this is always called so the sprites 
        /// are always considered to be moving no "move sprites" event exists 
        /// - sprites x&y just needs to be updated and they will always be moved
        /// </summary>
        /// <param name="headNode">first node in the answers list (in/out)</param>
        /// <param name="dldHeadNode">first node in the dictionary list</param>
        /// <param name="screen">SDL_Surface to display the image</param>
        /// <param name="letters">first node in the letter sprites (in/out)</param>
        /// <returns>Nothing</returns>
        public static void GameLoop(ref Node headNode, Dlb_node dldHeadNode, IntPtr screen, ref Sprite letters)
        {
            bool done = false;
            SDL.SDL_Event sdlEvent;
            int timeNow;
            // TimeSpan timeNow;

            SDL.SDL_Init(SDL.SDL_INIT_TIMER);
            uint timer_delay = 20;
            int timer = SDL.SDL_AddTimer(timer_delay, TimerCallBack, IntPtr.Zero);

            SDL.SDL_Rect dest;
            dest.x = 0;
            dest.y = 0;
            dest.w = 800;
            dest.h = 600;

            while (!done)
            {
                SDL.SDL_SetRenderDrawColor(screen, 0, 0, 0, 255);
                SDL.SDL_RenderClear(screen);
                SDLScale_RenderCopy(screen, backgroundTex, null, ref dest);

                if (winGame)
                {
                    stopTheClock = true;
                    solvePuzzle = true;
                }

                if ((gameTime < AVAILABLE_TIME) && !stopTheClock)
                {
                    timeNow = (DateTime.Now - gameStart).Seconds;
                    if (timeNow != gameTime)
                    {
                        gameTime = timeNow;
                        UpdateTime(screen);
                    }
                    else
                    {
                        if (!stopTheClock)
                        {
                            stopTheClock = true;
                            solvePuzzle = true;
                        }
                    }

                    // Check messages
                    if (solvePuzzle)
                    {
                        // Walk the list, setting everything to found
                        SolveIt(headNode);
                        ClearWord(letters);
                        Shuffle = SPACE_FILLED_STRING.ToCharArray();
                        Answer = rootword;
                        gamePaused = true;
                        if (!stopTheClock)
                        {
                            stopTheClock = true;
                        }
                        solvePuzzle = false;
                    }

                    if (updateAnswers)
                    {
                        // move letters back down again
                        ClearWord(letters);
                        updateAnswers = false;
                    }
                    DisplayAnswerBoxes(headNode, screen);

                    if (startNewGame)
                    {
                        // move letters back down again
                        if (!gotBigWord)
                        {
                            totalScore = 0;
                        }
                        NewGame(ref headNode, dldHeadNode, screen, ref letters);

                        startNewGame = false;
                    }

                    if (shuffleRemaining)
                    {
                        // shuffle up the shuffle box
                        string shuffler;
                        shuffler = new string(Shuffle);
                        ShuffleAvailableLetters(ref shuffler, ref letters);
                        Shuffle = shuffler.ToCharArray();
                        shuffleRemaining = false;
                    }

                    if (clearGuess)
                    {
                        // clear the guess
                        if (ClearWord(letters) > 0)
                        {
                            if (audio_enabled)
                            {
                                SDL_mixer.Mix_PlayChannel(-1, GetSound("clear"), 0);
                            }
                            clearGuess = false;
                        }
                    }

                    if (quitGame)
                    {
                        done = true;
                    }

                    while (SDL.SDL_WaitEvent(out sdlEvent) != 0)  // need to use int as return is not a bool
                    {
                        if (sdlEvent.type == SDL.SDL_EventType.SDL_USEREVENT)
                        {
                            timer_delay = (uint)(AnySpriteMoving(letters) ? 10 : 100);
                            SDL.SDL_SetRenderDrawColor(screen, 0, 0, 0, 255);
                            SDL.SDL_RenderClear(screen);
                            SDLScale_RenderCopy(screen, backgroundTex, null, ref dest);
                            DisplayAnswerBoxes(headNode, screen);
                            MoveSprites(ref screen, letters, letterSpeed);
                            timer = SDL.SDL_AddTimer(timer_delay, TimerCallBack, IntPtr.Zero);
                            break;
                        }

                        else if (sdlEvent.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
                        {
                            SDLScale_MouseEvent(ref sdlEvent);
                        }

                        else if (sdlEvent.type == SDL.SDL_EventType.SDL_KEYUP)
                        {
                            HandleKeyboardEvent(sdlEvent, headNode, letters);
                        }

                        else if (sdlEvent.type == SDL.SDL_EventType.SDL_QUIT)
                        {
                            done = true;
                            break;
                        }

                        else if (sdlEvent.type == SDL.SDL_EventType.SDL_WINDOWEVENT)
                        {
                            if ((sdlEvent.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED) ||
                                    (sdlEvent.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED))
                            {
                                double scalew = sdlEvent.window.data1 / 800.0;
                                double scaleh = sdlEvent.window.data2 / 600.0;
                                SDLScaleSet(scalew, scaleh);
                            }
                        }
                        SDL.SDL_SetRenderDrawColor(screen, 0, 0, 0, 255);
                        SDL.SDL_RenderClear(screen);
                        SDLScale_RenderCopy(screen, backgroundTex, null, ref dest);
                        DisplayAnswerBoxes(headNode, screen);
                        MoveSprites(ref screen, letters, letterSpeed);
                    }
                }
            }

        }


        /// <summary> Check that the dictionary in the local language exists </summary>
        /// <param name="path">The path, including the locale, to the wordfile in the desired language</param>
        /// <returns>true if the file exists otherwise false</returns>
        public static bool IsValidLocale(string path)
        {
            string filePath = path;
            if (filePath.ToCharArray()[filePath.Length] != '/')
            {
                filePath += '/';
            }
            filePath += "wordlist.txt";

            return File.Exists(filePath);
        }



        public static bool ConfigBox(Box pbox, string line)
        {
            int x, y, w, h;

        }

    }
}