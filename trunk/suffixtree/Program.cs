/* Program.cs
 * To Do: add comments
 * 
 * This is a suffix tree algorithm for .NET written in C#. Feel free to use it as you please!
 * This code was derived from Mark Nelson's article located here: http://marknelson.us/1996/08/01/suffix-trees/
 * Have Fun 
 * 
 * Zikomo A. Fields 2008
 *  
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Algorithms
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter a string: ");
            //StreamReader reader = new StreamReader("c:\\development\\data\\2of4brif.txt");
            //string something = reader.ReadToEnd();
            //string something = "\r\naah\r\naardvark\r\naardvarks\r\nabacus\r\nabacuses\r\nabalone\r\nabalones\r\nabandon\r\nabandoned\r\nabandoning\r\nabandonment\r\nabandons\r\nabase\r\nabased\r\nabasement\r\nabases\r\nabashed\r\nabasing\r\nabate\r\nabated\r\nabatement\r\nabates\r\nabating\r\nabattoir\r\nabattoirs\r\nabbess\r\nabbesses\r\nabbey\r\nabbeys\r\nabbot\r\nabbots\r\nabbreviate\r\nabbreviated\r\nabbreviates\r\nabbreviating\r\nabbreviation\r\nabbreviations\r\nabdicate\r\nabdicated\r\nabdicates\r\nabdicating\r\nabdication\r\nabdications\r\nabdomen\r\nabdomens\r\nabdominal\r\nabduct\r\nabducted";
            string something = Console.ReadLine();
            //something = 
            SuffixTree tree = new SuffixTree(something);
            tree.BuildTree();
            //string[] edges = tree.DumpEdges();
            //foreach (var edge in edges)
            //{
            //    Console.WriteLine(edge);
            //}

            while (true)
            {
                Console.Write("Search for: ");
                string searchTerm = Console.ReadLine();
                if (tree.Search(searchTerm))
                {
                    Console.WriteLine("It's in there!");
                }
                else
                {
                    Console.WriteLine("Nope not in there!");
                }
                if (searchTerm.ToLower() == "quit")
                {
                    break;
                }
            }            
        }
    }
}
