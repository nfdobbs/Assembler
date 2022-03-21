/*
 * Name:         Nathan Dobbs
 * Class:        CSC 354
 * Assignment:   5, Linking Loader
 * Due Date:     12/11/19
 * Instructor:   Gamradt
 *
 * Description: LinkingLoader class handles all parts of linking and loading object code
 */
using System;
using System.IO;

namespace Dobbs5
{
    class LinkingLoader
    {
        char[] memArray = new char[800];
        int BASE_ADDRESS= 8560; // =0x2170
        string programName;
        int programStartAddress;
        int programLength;
        string executionAddress;
        ExtSymTab table = new ExtSymTab();

        int endIterator = 0;

        /*
        * Function: LinkFiles()
        * 
        * Description: Primary driver for linking loader accepts three string arrays
        * Input Args:  string[][] files, int size
        * Output Args: ---
        * In/Out Args: ---
        * Return:      void
        */
        public void LinkFiles(string[][] files, int size)
        {
            int[] programStart = new int[10];

            //Console.WriteLine(file1[1]);
            //Console.WriteLine(file2[1]);
            //Console.WriteLine(file3);

            for(int j = 0; j < size; j++)
                for (int i = 0; i < files[j].Length; i++)
                {
                    if (files[j][i][0] == 'H')
                    {
                        HRecord(files[j][i].Trim());
                        programStart[j] = programStartAddress;
                    }

                    else if (files[j][i][0] == 'D')
                    {
                        DRecord(files[j][i].Trim());
                    }

                    else if (files[j][i][0] == 'T')
                    {
                        TRecord(files[j][i].Trim());
                    }

                    else if(files[j][i][0] == 'E')
                    {
                        string tempLine = files[j][i].Remove(0, 1);

                        if(j == 0)
                        {
                            int temporaryInt = int.Parse(tempLine, System.Globalization.NumberStyles.HexNumber);
                            executionAddress = (programStart[0] + temporaryInt).ToString("X");


                            while (executionAddress.Length < 6)
                                executionAddress = executionAddress.Insert(0, "0");
                        }
                    }
                }

            for(int j = 0; j < size; j++)
            {
                programStartAddress = programStart[j];

                for (int i = 0; i < files[j].Length; i++)
                {
                    if (files[j][i][0] == 'M')
                    {
                        MRecord(files[j][i].Trim());
                    }
                }
            }
        }

        /*
        * Function: LinkingLoader()
        * 
        * Description: Constructor for the class
        * Input Args:  ---
        * Output Args: ---
        * In/Out Args: ---
        * Return:      ---
        */
        public LinkingLoader()
        {
            for(int i = 0; i < memArray.Length; i++)
            {
                memArray[i] = 'U';
            }
        }

        /*
       * Function: HRecord()
       * 
       * Description: Handles the H Record portion of the object code
       * Input Args:  string line
       * Output Args: ---
       * In/Out Args: ---
       * Return:      void
       */
        private void HRecord(string line)
        {
            string[] hold = line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries); //Changed

            if (hold.Length == 1)
            {
                hold[0] = hold[0].Remove(0, 1);
                programName = hold[0].Remove(6);
                hold[0] = hold[0].Remove(0,6);
                programStartAddress = int.Parse(hold[0].Remove(6), System.Globalization.NumberStyles.HexNumber);
                hold[0] = hold[0].Remove(0,6);
                programLength = int.Parse(hold[0], System.Globalization.NumberStyles.HexNumber);
            }

            else
            {
                programName = hold[0].Remove(0, 1);
                programStartAddress = int.Parse(hold[1].Remove(6), System.Globalization.NumberStyles.HexNumber);
                programLength = int.Parse(hold[1].Remove(0, 6), System.Globalization.NumberStyles.HexNumber);
            }

            // Console.WriteLine(programName);
            // Console.WriteLine(programStartAddress);
            // Console.WriteLine(programLength);
            programStartAddress = BASE_ADDRESS + programStartAddress + endIterator;
            //Console.WriteLine(programStartAddress);
            endIterator += programLength;

            table.root = table.Insert(table.root, programName, programName, 0, programStartAddress, programLength.ToString("X6"));
        }

        /*
        * Function: DRecord()
        * 
        * Description: Handles the D Record portion of the object code
        * Input Args:  string line
        * Output Args: ---
        * In/Out Args: ---
        * Return:      void
        */
        private void DRecord(string line)
        {
            line = line.Remove(0, 1);
            line = line.Trim();

            int counter = 0;
            string loadAddress = "";
            string tempName = "";

            for(int i = 0; i < line.Length; i++)
            {
                if(counter++ >= 5)    //Changed
                {
                    for (int j = 0; j < 7; j++)
                    {
                         loadAddress += line[i + j];
                    }
                    
                    i = i + 5;
                    table.root = table.Insert(table.root, tempName.Trim(), programName, int.Parse(loadAddress.Trim(), System.Globalization.NumberStyles.HexNumber), programStartAddress); //Changed
                    counter = 0;
                    loadAddress = "";
                    tempName = "";
                    i++;
                    
                }
                else
                {
                    tempName += line[i];
                }
            }


        }

        /*
        * Function: TRecord()
        * 
        * Description: Handles the T Record portion of the object code
        * Input Args:  string line
        * Output Args: ---
        * In/Out Args: ---
        * Return:      void
        */
        private void TRecord(string line)
        {
            line = line.Remove(0, 1);
            string TStart = line.Remove(6);
            line = line.Remove(0, 8);
            int StartIterator = (int.Parse(TStart, System.Globalization.NumberStyles.HexNumber) * 2) + (endIterator - programLength)*2;

            for (int i = 0; i < line.Length; i++)
                memArray[StartIterator + i] = line[i];
            
        }

        /*
       * Function: MRecord()
       * 
       * Description: Handles the M Record portion of the object code
       * Input Args:  string line
       * Output Args: ---
       * In/Out Args: ---
       * Return:      void
       */
        private void MRecord(string line)
        {
            int iterator = programStartAddress - BASE_ADDRESS;

            string tempString = "";

            line = line.Remove(0, 1);
            int starter = int.Parse(line.Remove(6), System.Globalization.NumberStyles.HexNumber);
            line = line.Remove(0, 6);
            int size = int.Parse(line.Remove(2), System.Globalization.NumberStyles.HexNumber);
            line = line.Remove(0, 2);

            iterator += starter;
            iterator = iterator * 2;

            if (size % 2 == 0)
                iterator = iterator - 1;
            
            for(int i = 0; i < size; i++)
            {
                tempString += memArray[iterator + i + 1];
            }
            
            //Console.WriteLine(tempString);
            int tempInt = int.Parse(tempString, System.Globalization.NumberStyles.HexNumber);
            //Console.WriteLine(tempInt);

            if (line[0] == '+')
            {
                line = line.Remove(0, 1);
                //Console.WriteLine(tempInt);
                tempInt += table.SymbolsLoadAddress(line);

                
            }
            else if(line[0] == '-')
            {
                line = line.Remove(0, 1);
                tempInt -= table.SymbolsLoadAddress(line);
            }
            else
            {
                //Console.WriteLine("Error in Object Code");
            }

            tempString = tempInt.ToString("X");
            while(tempString.Length < size)
            {
                tempString = tempString.Insert(0, "0");
            }

            while(tempString.Length > size)
            {
                tempString = tempString.Remove(0, 1);
            }

            for(int i = 0; i < size; i++)
            {
                if (size % 2 != 0)
                    memArray[iterator + i + 1] = tempString[i];
                else
                    memArray[iterator + i + 1] = tempString[i];
            }
        }

        /*
       * Function: PrintMemory()
       * 
       * Description: Prints the memory contents to both the string and to a file memory.txt
       * Input Args:  ---
       * Output Args: ---
       * In/Out Args: ---
       * Return:      void
       */
        public void PrintMemory()
        {
            Console.Write("         0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F");
            for (int i = 0; i + 1 < endIterator*2 + 1; i=i+2)
            {
                if (i % 32 == 0)
                    Console.Write("\n{0,5}   ", (BASE_ADDRESS+(i/2)).ToString("X5"));
                else
                    Console.Write(" ");

                Console.Write(memArray[i]);
                Console.Write(memArray[i + 1]);
            }

            Console.WriteLine();
            Console.WriteLine("\nExecution begins at address: " + executionAddress);
            Console.WriteLine("Memory contents also output to memory.txt");


            string dirPath = new string(Directory.GetCurrentDirectory());
            dirPath = Path.GetDirectoryName(dirPath);
            dirPath = Path.GetDirectoryName(dirPath);
            dirPath = Path.GetDirectoryName(dirPath);
            string searchPath = dirPath + "\\" + "memory.txt";

            string fileText = "";
            fileText += "         0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F";

            for (int i = 0; i + 1 < endIterator * 2 + 1; i = i + 2)
            {
                if (i % 32 == 0)
                {
                    fileText += "\n";
                    fileText += (BASE_ADDRESS + (i / 2)).ToString("X5");
                    fileText += "   ";
                }
                else
                    fileText += " ";

                fileText += memArray[i];
                fileText += memArray[i + 1];
            }

            fileText += "\n\nExecution begins at address: " + executionAddress;

            System.IO.File.WriteAllText(searchPath, fileText);

            Console.WriteLine("\nSYMBOL   CSECT    ADDR  CSADDR   LDADDR    LENGTH");
            table.ViewPrint(table.root);
        }


    }
}
