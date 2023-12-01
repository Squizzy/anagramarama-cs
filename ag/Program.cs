using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
/*// using System.Reflection.Metadata;
// using System.IO.Compression;
*/

namespace ag
{
    partial class Program
    {
        // Debug
        // Private static bool myDEBUG = true;
        private static bool myDEBUGmacos = false;
        private static bool myDEBUGfr = false;

        // method to identify the local language path for the locale files (dictionarity, background, ...)
        private static string DictPathLanguage()
        {
            string path = "i18n/";
            if (!myDEBUGmacos) path = "../../../i18n/"; 
            
            string lang;
            lang = System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag;
            //backup in case no locale was returned: en-GB
            if (lang == null) lang = "en-GB";
            if (myDEBUGfr) { lang = "fr-FR";}
            //To be extended with checks for "isValidLocale"?
            
            return path + lang + "/";
        }

        public static void Main()
        {
            // initiate the reference to the first node (first letter to be gathered from the input file)
            dlb_node newNode = new dlb_node();  

            // initiate the reference to the linked list, ie the sequence of letters each stored in a new node.
            dlb_linkedlist newLinkedList = new dlb_linkedlist(); 

            //find the local path with the IETF international code
            string dictonaryPathLanguage = DictPathLanguage();

            // load the dictionary
            newLinkedList.dlb_create(newNode, dictonaryPathLanguage + "wordlist.txt");
            
            Console.WriteLine("end");
        }
    }
}

        