/*
 * Name:         Nathan Dobbs
 * Class:        CSC 354
 * Assignment:   5, Linking Loader
 * Due Date:     12/11/19
 * Instructor:   Gamradt
 *
 * Description: External Symbol Table used by linking loader implemented as binart tree
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Dobbs5
{
    public class Node
    {
        public string symbol;
        public string controlSection;
        public int address;
        public int controlAddress;
        public int loadAddress;
        public string length;
        public Node left;
        public Node right;
    }

    public class ExtSymTab
    {
        //Class Attributes//
        public Node root;

        //Class Methods//
        /*
         * Function: ExtSymTab()
         * 
         * Description: Constructor for Class
         * Input Args:  ---
         * Output Args: ---
         * In/Out Args: ---
         * Return:      ---
         */
        public ExtSymTab()
        {
            
        }

        /*
         * Function: Insert()
         * 
         * Description: Inserts the given data into a node in the tree if it qualifies
         * Input Args:  Node root, string newSymbol, string newControlSection, int newAddress, int newControlAddress
         * Output Args: Node root
         * In/Out Args: ---
         * Return:      Node root
         */
        public Node Insert(Node root, string newSymbol, string newControlSection, int newAddress, int newControlAddress)
        {
            Node searchNode;
            searchNode = Search(root, newSymbol); //Remove Colon
            
            if (searchNode == null)
            {
                //Actually Adding the Data
                if (root == null)
                {
                    int tempLoadAddress = newAddress + newControlAddress;

                    root = new Node
                    {
                        symbol = newSymbol,
                        controlSection = newControlSection,
                        address = newAddress,
                        controlAddress = newControlAddress,
                        loadAddress = tempLoadAddress,
                        length = "---"
                    };
                }

                else if (string.Compare(root.symbol, newSymbol) == -1)
                {
                    root.right = Insert(root.right, newSymbol, newControlSection, newAddress, newControlAddress);
                }

                else
                {
                    root.left = Insert(root.left, newSymbol, newControlSection, newAddress, newControlAddress);
                }
            }

            else
            {
                Console.WriteLine("Error: \"" + newSymbol + "\" Already Defined");
            }
        
            
            return root;
        }

        /*
         * Function: Search
         * 
         * Description: Searches for a given Symbol in the tree and returns its node, private for class use only
         * Input Args:  Node root, string searchSymbol
         * Output Args: ---
         * In/Out Args: ---
         * Return:      Node (null or Symbols Node)
         */
        private Node Search(Node root, string searchSymbol)
        {
            if (root == null)
                return root;

            else if (root.symbol == searchSymbol)
                return root;

            else if (string.Compare(root.symbol, searchSymbol) == 1)
                return Search(root.left, searchSymbol);

            else
                return Search(root.right, searchSymbol);
        }

       
        /*
         * Function: Destroy()
         * 
         * Description: Private function used to completly destroy symbol table, C# handles the data, 
         *              Destroy unlinks all the nodes;
         * Input Args:  Node root
         * Output Args: ---
         * In/Out Args: ---
         * Return:      void
         */
        private void Destroy(Node root)
        {
            if (root != null)
            {
                Destroy(root.left);
                Destroy(root.right);
                root = null;
                ;
            }
        }

        /*
        * Function: SymbolsLoadAddress()
        * 
        * Description: Returns value of symbols loadAddress
        * Input Args:  string searchSymbol
        * Output Args: ---
        * In/Out Args: ---
        * Return:      void
        */
        public int SymbolsLoadAddress(string searchSymbol)
        {
            Node searchNode = Search(root, searchSymbol);

            if (searchNode == null)
            {
                Console.WriteLine("Error: \"" + searchSymbol + "\" Not Found");
            }

            else
                return searchNode.loadAddress;

            return 0;
        }
 
        /*
        * Function: ~ExtSymTab()
        * 
        * Description: Deconstructor for Class, Makes use of Destroy
        * Input Args:  ---
        * Output Args: ---
        * In/Out Args: ---
        * Return:      ---
        */
        ~ExtSymTab()
        {
            Destroy(root);
        }


        /*
        * Function: ViewPrint()
        * 
        * Description: Prints External Symbol Table for Reference
        * Input Args:  Node root
        * Output Args: ---
        * In/Out Args: ---
        * Return:      ---
        */
        public void ViewPrint(Node root)
        {

            if (root != null)
            {
                ViewPrint(root.left);
                Console.WriteLine(String.Format("{0,6}|{1,7}|{2,7}|{3,7}|{4,8}|{5,9}", root.symbol, root.controlSection, root.address.ToString("X5"), root.controlAddress.ToString("X5"), root.loadAddress.ToString("X5"), root.length));
                ViewPrint(root.right);
            }

       }

        /*
        * Function: Insert()
        * 
        * Description: Inserts the given data into a node in the tree if it qualifies, alternal insert allows lenght to be added
        * Input Args:  Node root, string newSymbol, string newControlSection, int newAddress, int newControlAddress, string newLength
        * Output Args: Node root
        * In/Out Args: ---
        * Return:      Node root
        */
        public Node Insert(Node root, string newSymbol, string newControlSection, int newAddress, int newControlAddress, string newLength)
        {
            Node searchNode;
            searchNode = Search(root, newSymbol); //Remove Colon

            if (searchNode == null)
            {
                //Actually Adding the Data
                if (root == null)
                {
                    int tempLoadAddress = newAddress + newControlAddress;

                    root = new Node
                    {
                        symbol = newSymbol,
                        controlSection = newControlSection,
                        address = newAddress,
                        controlAddress = newControlAddress,
                        loadAddress = tempLoadAddress,
                        length = newLength

                    };
                }
                else if (string.Compare(root.symbol, newSymbol) == -1)
                {
                    root.right = Insert(root.right, newSymbol, newControlSection, newAddress, newControlAddress, newLength);
                }

                else
                {
                    root.left = Insert(root.left, newSymbol, newControlSection, newAddress, newControlAddress, newLength);
                }
            }

            else
            {
                Console.WriteLine("Error: \"" + newSymbol + "\" Already Defined");
            }


            return root;
        }
    }
}
