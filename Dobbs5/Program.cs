/*
 * Name:         Nathan Dobbs
 * Class:        CSC 354
 * Assignment:   5, Linking Loader
 * Due Date:     12/11/19
 * Instructor:   Gamradt
 *
 * Description: Driver Program For Linking Loader
 */
using System;
using System.IO;

namespace Dobbs5
{
    class Program
    {
        /*
         * Function: Main()
         * 
         * Description: Driver Function for Program
         * Input Args:  string[] args - Accepts 3 files only
         * Output Args: ---
         * In/Out Args: ---
         * Return:      void
         */
        static void Main(string[] args)
        {
            LinkingLoader LL = new LinkingLoader();
            int MAX_FILES = 10;
           
            if (args.Length == 0)
            {
                Console.WriteLine("Expected Program File");
                Console.WriteLine("Program wil now exit");
                return;
            }

            string dirPath = new string(Directory.GetCurrentDirectory());
            dirPath = Path.GetDirectoryName(dirPath);
            dirPath = Path.GetDirectoryName(dirPath);
            dirPath = Path.GetDirectoryName(dirPath);

            string[][] files = new string[MAX_FILES][];
            
            for(int i = 0; i < args.Length; i++)
            {
                if (!File.Exists(dirPath + "\\" + args[i]))
                {
                    Console.WriteLine("File " + args[i] + " not found program will now exit");
                    return;
                }
                
                files[i] = File.ReadAllLines(dirPath + "\\" + args[i]);
            }

           LL.LinkFiles(files, args.Length);
           LL.PrintMemory();
        }
    }
}
