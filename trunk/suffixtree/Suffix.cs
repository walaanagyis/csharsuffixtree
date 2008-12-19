/* Suffix.cs
 * To Do: Add comments
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

namespace suffixtree
{
    public class Suffix
    {
        public int originNode = 0;
        public int indexOfFirstCharacter;
        public int indexOfLastCharacter;
        public string theString;
        public Edge[] edges;
        public Suffix(string theString, Edge[] edges, int node, int start, int stop)
        {
            this.originNode = node;
            this.indexOfFirstCharacter = start;
            this.indexOfLastCharacter = stop;
            this.theString = theString;
            this.edges = edges;
        }

        public bool IsExplicit
        {
            get
            {
                return indexOfFirstCharacter > indexOfLastCharacter;
            }        
        }

        public void Canonize()
        {
            if (!IsExplicit)
            {
                Edge edge = Edge.Find(theString, edges, originNode, theString[indexOfFirstCharacter]);
                int edgeSpan = edge.indexOfLastCharacter - edge.indexOfFirstCharacter;
                while (edgeSpan <= (this.indexOfLastCharacter - this.indexOfFirstCharacter))
                {
                    this.indexOfFirstCharacter = this.indexOfFirstCharacter + edgeSpan + 1;
                    this.originNode = edge.endNode;
                    if (this.indexOfFirstCharacter <= this.indexOfLastCharacter)
                    {
                        edge = Edge.Find(theString, edges, edge.endNode, theString[this.indexOfFirstCharacter]);
                        edgeSpan = edge.indexOfLastCharacter - edge.indexOfFirstCharacter;
                    }
                }
            }
        }
    }
}
