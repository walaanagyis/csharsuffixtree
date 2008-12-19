/* SuffixTree.cs
 * To Do: add comments
 * 
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

namespace Algorithms
{
    public class SuffixTree
    {
        Edge edges;
        public string theString;
        
        public Edge[] Edges = new Edge[Edge.HASH_TABLE_SIZE];
        public Dictionary<int, Node> Nodes = null;

        public SuffixTree(string theString)
        {
            this.theString = theString;
            Nodes = new Dictionary<int, Node>(theString.Length*2);
            edges = new Edge(this.theString);
            for (int i = 0; i < Edges.Length; i++ )
            {
                Edges[i] = new Edge(theString);
            }
        }

        public void BuildTree()
        {
            Suffix active = new Suffix(this.theString, Edges, 0, 0, -1);
            for (int i = 0; i <= theString.Length-1; i++)
            {
                AddPrefix(active, i);
            }
        }

        public bool Search(string search)
        {
            if (search.Length == 0)
            {
                return false;
            }
            int index = 0;
            Edge edge = this.Edges[Edge.Hash(0, search[0])];

            if (edge.startNode == -1)
            {
                return false;
            }
            else
            {
                for (; ; )
                {
                    for (int j = edge.indexOfFirstCharacter; j <= edge.indexOfLastCharacter; j++)
                    {
                        if (index >= search.Length)
                        {
                            return true;
                        }
                        if (this.theString[j] != search[index++])
                        {
                            return false;
                        }
                    }
                    if (index < search.Length)
                    {
                        edge = this.Edges[Edge.Hash(edge.endNode, search[index])];
                    }
                    else
                    {
                        return true;
                    }

                }
            }            
        }

        public string[] DumpEdges()
        {
            List<string> edges = new List<string>();
            int count = this.theString.Length - 1;
            for (int j = 0; j < Edge.HASH_TABLE_SIZE; j++)
            {
                Edge edge = this.Edges[j];
                if (edge.startNode == -1)
                {
                    continue;
                }
                int top = 0;
                if (count > edge.indexOfLastCharacter)
                {
                    top = edge.indexOfLastCharacter;
                }
                else
                {
                    top = count;
                }
                StringBuilder builder = new StringBuilder();
                for (int i = edge.indexOfFirstCharacter; i <= top; i++)
                {                    
                    builder.Append(this.theString[i]);                    
                }
                edges.Add(builder.ToString());                
            }
            return edges.ToArray();
        }


        private void AddPrefix(Suffix active, int indexOfLastCharacter)
        {
            int parentNode;
            int lastParentNode = -1;

            for (; ; )
            {
                Edge edge;
                parentNode = active.originNode;

                if (active.IsExplicit)
                {
                    edge = Edge.Find(this.theString, this.Edges, active.originNode, theString[indexOfLastCharacter]);
                    if (edge.startNode != -1)
                    {
                        break;
                    }
                }
                else
                {
                    edge = Edge.Find(this.theString, this.Edges, active.originNode, theString[active.indexOfFirstCharacter]);
                    int span = active.indexOfLastCharacter - active.indexOfFirstCharacter;
                    if (theString[edge.indexOfFirstCharacter + span + 1] == theString[indexOfLastCharacter])
                    {
                        break;
                    }
                    parentNode = Edge.SplitEdge(active, theString, Edges, Nodes, edge);
                }

                Edge newEdge = new Edge(this.theString, indexOfLastCharacter, this.theString.Length - 1, parentNode);                
                Edge.Insert(theString, Edges, newEdge);
                if (lastParentNode > 0)
                {
                    Nodes[lastParentNode].suffixNode = parentNode;                   
                }
                lastParentNode = parentNode;

                if (active.originNode == 0)
                {
                    active.indexOfFirstCharacter++;
                }
                else
                {
                    active.originNode = Nodes[active.originNode].suffixNode;
                }                
                active.Canonize();
            }
            if (lastParentNode > 0)
            {
                Nodes[lastParentNode].suffixNode = parentNode;
            }
            active.indexOfLastCharacter++;
            active.Canonize();
        }
    }
}
