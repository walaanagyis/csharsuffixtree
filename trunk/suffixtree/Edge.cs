/* Edge.cs
 * To Do: add comments
 * 
 * 
 * This is a suffix tree algorithm for .NET written in C#. Feel free to use it as you please!
 * This code was derived from Mark Nelson's article located here: http://marknelson.us/1996/08/01/suffix-trees/
 * Have Fun 
 * 
 * Zikomo A. Fields 2008
 * 

 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms
{
    public class Edge
    {
        public int indexOfFirstCharacter;
        public int indexOfLastCharacter;
        public int startNode;
        public int endNode;
        string theString;

        public const int HASH_TABLE_SIZE = 2179;        
        
        public Edge(string theString)
        {
            this.theString = theString;
            this.startNode = -1;
        }

        public Edge(string theString, int indexOfFirstCharacter, int indexOfLastCharacter, int parentNode)
        {
            this.theString = theString;
            this.indexOfFirstCharacter = indexOfFirstCharacter;
            this.indexOfLastCharacter = indexOfLastCharacter;
            this.startNode = parentNode;
            this.endNode = Node.Count++;
        }

        static public void Insert(string theString, Edge[] edges, Edge edge)
        {
            int i = Hash(edge.startNode, theString[edge.indexOfFirstCharacter]);
            while (edges[i].startNode != -1)
            {
                i = ++i % HASH_TABLE_SIZE;
            }
            edges[i] = edge;
        }

        static public void Remove(string theString, Edge[] edges, Edge edge)
        {
            int i = Hash(edge.startNode, theString[edge.indexOfFirstCharacter]);
            while (edges[i].startNode != edge.startNode || edges[i].indexOfFirstCharacter != edge.indexOfFirstCharacter)
            {
                i = ++i % HASH_TABLE_SIZE;
            }

            for (; ; )
            {
                edges[i].startNode = -1;
                int j = i;
                for (; ; )
                {
                    i = ++i % HASH_TABLE_SIZE;
                    if (edges[i].startNode == -1)
                    {
                        return;
                    }

                    int r = Hash(edges[i].startNode, theString[edges[i].indexOfFirstCharacter]);
                    if (i >= r && r > j)
                    {
                        continue;
                    }
                    if (r > j && j > i)
                    {
                        continue;
                    }
                    if (j > i && i >= r)
                    {
                        continue;
                    }
                    break;
                }
                edges[j] = edges[i];
            }
        }

        static public int SplitEdge(Suffix s, string theString, Edge[] edges, Dictionary<int, Node> nodes, Edge edge)
        {
            Remove(theString, edges, edge);
            Edge newEdge = new Edge(theString, edge.indexOfFirstCharacter,
                edge.indexOfFirstCharacter + s.indexOfLastCharacter 
                - s.indexOfFirstCharacter, s.originNode);
            Edge.Insert(theString, edges, newEdge);
            //newEdge.Insert();
            if (nodes.ContainsKey(newEdge.endNode))
            {
                nodes[newEdge.endNode].suffixNode = s.originNode;
            }
            else
            {
                nodes.Add(newEdge.endNode, new Node());
            }

            edge.indexOfFirstCharacter += s.indexOfLastCharacter - s.indexOfFirstCharacter + 1;
            edge.startNode = newEdge.endNode;
            Edge.Insert(theString, edges, edge);
            //Insert();
            return newEdge.endNode;
           
        }

        static public Edge Find(string theString, Edge[] edges, int node, int c)
        {
            int i = Hash(node, c);
            for (; ; )
            {
                if (edges[i].startNode == node)
                {
                    if (c == theString[edges[i].indexOfFirstCharacter])
                    {
                        return edges[i];
                    }                   
                }
                if (edges[i].startNode == -1)
                {
                    return edges[i];
                }
                i = ++i % HASH_TABLE_SIZE;
            }
            //return null;
        }

        public static int Hash(int node, int c)
        {
            int rtnValue = ((node << 8) + c) % HASH_TABLE_SIZE;
            return rtnValue;
        }
    }
}
