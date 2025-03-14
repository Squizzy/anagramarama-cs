#define DEBUG

using System.Runtime.InteropServices;
using SDL2;


namespace ag
{

    public partial class Program
    {
        
        public enum HotBoxes { boxSolve, boxNew, boxQuit, boxShuffle, boxEnter, boxClear };

        public static Box[] hotbox = new Box[]
        {
            new() { x = 612, y =   0, width = 66, height = 30 },  /* boxSolve */  
            new() { x = 686, y =   0, width = 46, height = 30 },  /* boxNew */    
            new() { x = 742, y =   0, width = 58, height = 30 },  /* boxQuit */   
            new() { x = 618, y = 206, width = 66, height = 16 },  /* boxShuffle */
            new() { x = 690, y = 254, width = 40, height = 35 },  /* boxEnter */  
            new() { x = 690, y = 304, width = 40, height = 40 }   /* boxClear */
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
        /// <summary>gameTime</summary>
        public static DateTime gameTime;
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
        /// <summary>gotBigWord</summary>
        public static bool gotBigWord = false;
        /// <summary>bigWordLen</summary>
        public static int bigWordLen = 0;
        /// <summary>updateTheScore</summary>
        public static bool updateTheScore = false;
        /// <summary>gamePaused</summary>
        public static bool gamePaused = false;
        /// <summary>foundDuplicate</summary>
        public static bool foundDuplicate = false;
        /// <summary>quitGame</summary>
        public static bool quitGame = false;
        /// <summary>winGame</summary>
        public static bool winGame = false;
 
        /// <summary>letterSpeed</summary>
        public static int letterSpeed = LETTER_FAST;
 
        /// <summary>fullscreen</summary>
        public static bool fullscreen = false;

        // SDL summarys
        /// <summary>window</summary>
        public static IntPtr window;

        /// <summary>backgroundTex</summary>
        public static IntPtr backgroundTex = IntPtr.Zero;
        /// <summary>letterBank</summary>
        public static IntPtr letterBank = IntPtr.Zero;
        /// <summary>smallLetterBank</summary>
        public static IntPtr smallLetterBank = IntPtr.Zero;
        /// <summary>numberBank</summary>
        public static IntPtr numberBank = IntPtr.Zero;
        /// <summary>answerBoxUnknown</summary>
        public static IntPtr answerBoxUnknown = IntPtr.Zero;
        /// <summary>answerBoxKnown</summary>
        public static IntPtr answerBoxKnown = IntPtr.Zero;
        /// <summary>clockSprite</summary>
        public static Sprite? clockSprite = null;
        /// <summary>scoreSprite</summary>
        public static Sprite? scoreSprite = null;

        // audio vars
        /// <summary>audio_enabled</summary>
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

        /// <summary>
        /// Search through the list of sound names and return the corresponding audio chunk
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

        /// <summary>
        /// push all the game sounds onto the soundCache linked list.  
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


        /// <summary>
        /// Free the memory of the sound chunks
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

        /// <summary>
        /// Display the answer boxes (small boxes at the bottom I think)
        /// </summary>
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


        /// <summary>
        /// Declare all all the anagrams as found (but not necessarily guessed)
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


        /// <summary>
        /// determine the next blank space in a string 
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

        /// <summary>
        /// handle the keyboard events:
        ///  - BACKSPACE & ESCAPE - clear letters
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
                    CheckGuess(new string (Answer), headNode);
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


        // STOPPED HERE ###########################

        public static void newGame(ref Node answers, ref dlb_node dict, IntPtr backgroundTex, IntPtr screen, Sprite letters, char[] rootWord, string wordsListPath = "")
        {
            // letters in the guess box
            char[] guess = new char[9];
            char[] remain = new char[9];
            // happy is true if we have <=77 anagrams and => 6
            bool happy = false;
            int answerSought = 0;
            int bigWordLen = 0;

            SDL.SDL_Rect dest;
            dest.x = 0;
            dest.y = 0;
            dest.w = 800;
            dest.h = 600;
            //IntPtr temp = BackgroundText

            SDL.SDL_Rect firstrect;
            firstrect.x = 0;
            firstrect.y = 0;
            firstrect.w = 800;
            firstrect.h = 600;

            SDLScale_RenderCopy(screen, backgroundTex, ref firstrect, ref dest);

            //destroyLetters(letters);
            Console.WriteLine("About to look for Happy");
            while (!happy)
            {
                char[] buffer = new char[9];
                buffer = GetRandomWord(wordsListPath).ToCharArray();
                //guess = "".ToCharArray();
                rootWord = buffer;
                bigWordLen = rootWord.Length - 1;

                for (int i = 0; i < rootWord.Length; i++) remain[i] = rootWord[i];
                //remain =rootWord;


                // Not needed in C# as garbage collection is handled already
                //destroyAnswers(answers);

                answerSought = Length(answers);
                string newGuessString = new string(guess).Trim('\0');
                string newRemainString = new string(remain).Trim('\0');
                Ag(ref answers, dict, newGuessString, newRemainString);
                guess = newGuessString.ToCharArray();
                char[] rem = newRemainString.ToCharArray();
                for (int i = 0; i < newRemainString.Length; i++) remain[i] = rem[i];
                //remain = newRemainString.ToCharArray();

                answerSought = Length(answers);

                // happy if the number of anagrams are 6 or more, and less than 77
                happy = ((answerSought < 77) && (answerSought >= 6));

#if DEBUG
                if (!happy) Console.WriteLine($"Too Many Answers!  word: {new string(rootWord)}, answers: {answerSought}");
#endif
            }
            Console.WriteLine("Happy found");
#if DEBUG
            if (happy) Console.WriteLine($"Selected word: {new string(rootWord)}, answers: {answerSought}");
#endif

            Sort(ref answers);

            for (int i = bigWordLen; i < 7; i++)
            {
                remain[i] = SPACE_CHAR;
            }
            remain[7] = '\0';
            remain[bigWordLen] = '\0';

            ShuffleWord(remain);
            shuffle = remain;
            answer = SPACE_FILLED_STRING;

            // HERE




        }


        /// <summary>
        /// Shuffles the characters in a given word array.
        /// </summary>
        /// <param name="word">The word array to be shuffled.</param>
        /// <remarks>
        /// This method generates a random number between 20 and 26, and then swaps two characters in the word array
        /// for the generated number of times. The characters to be swapped are randomly selected using the Random class.
        /// </remarks>

        public static void ShuffleWord(char[] word)
        {
            char tmp;
            
            // generate a random number between 20 and 26. The rand() function in C no longer exists in C#
            Random randCount = new Random();
            int count = randCount.Next(20, 27);

            // generate two random values, using the same random generator to prevent possible repeated values.
            Random randPos = new Random();
            int a = randPos.Next(0, 7);
            int b = randPos.Next(0, 7);

            for (int n=0; n < count; ++n )
            {
                a = randPos.Next(0, 7);
                b = randPos.Next(0, 7);
                tmp = word[a];
                word[a] = word[b];
                word[b] = tmp;
            }

        }


        public static void BuildLetters(ref Sprite letters, IntPtr screen)
        {
            Sprite thisLetter = new Sprite();
            Sprite previousLetter = new Sprite();
            SDL.SDL_Rect rect = new SDL.SDL_Rect();
            int index = 0;

            rect.y = 0;
            rect.w = GAME_LETTER_WIDTH;
            rect.h = GAME_LETTER_HEIGHT;

            int len = shuffle.Length;

            for (int i=0; i<len; i++)

            {
                thisLetter.numSpr = 0;

                // determine which letter we're wanting and load it from 
                // the letterbank*/

                if ((int)shuffle[i] != ASCII_SPACE && shuffle[i] != SPACE_CHAR )
                {
                    int chr = (int)(shuffle[i] - 'a');
                    rect.x = chr * GAME_LETTER_WIDTH;
                    thisLetter.numSpr = 1;


                    thisLetter.spr[0].t = letterBank;
                    thisLetter.spr[0].w = rect;
                    thisLetter.spr[0].x = 0;
                    thisLetter.spr[0].y = 0;


                    thisLetter.x = rnd.Next(0, 799); // Dulsi comment did not seem to align with his code: i * (GAME_LETTER_WIDTH + GAME_LETTER_SPACE) + BOX_START_X;
                    thisLetter.y = rnd.Next(0, 599); // Dulsi comment did not seem to align with his code:  SHUFFLE_BOX_Y;
                    thisLetter.letter = shuffle[i];
                    thisLetter.h = GAME_LETTER_HEIGHT;
                    thisLetter.w = GAME_LETTER_WIDTH;
                    thisLetter.toX = i * (GAME_LETTER_WIDTH + GAME_LETTER_SPACE) + BOX_START_X;
                    thisLetter.toY = SHUFFLE_BOX_Y;
                }
            }



        }
// HERE TOO
    }
}