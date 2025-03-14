using SDL2;

namespace ag
{
    partial class Program
    {
        /// <summary>
        /// This is an attempt to convert the c function from the original anagramarama 0.7
        /// but this is not needed in c#
        /// It is currently not working
        /// </summary>
        /// <param name="letters"></param>
        public static void destroyLetters(ref Sprite? letters)
        {
            Sprite? current = letters;
            while (current != null) {
                Sprite tmp = current;
                if (current.numSpr > 0)
                    current.spr = null;
                current = current.next;
                tmp = null;
            }
	        letters = null;
        }

    }
}
