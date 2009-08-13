/* Edge.cs
 * To Do: add comments
 * I need a better hashing system for large data sets. 
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
    public struct Edge
    {
        public int indexOfFirstCharacter;
        public int indexOfLastCharacter;
        public int startNode;
        public int endNode;
        string theString;

        public const int HASH_TABLE_SIZE = 306785407;        
        
        public Edge(string theString)
        {
            this.theString = theString;
            this.startNode = -1;
            this.indexOfFirstCharacter = 0;
            this.indexOfLastCharacter = 0;
            this.endNode = 0;
        }

        public Edge(string theString, int indexOfFirstCharacter, int indexOfLastCharacter, int parentNode)
        {
            this.theString = theString;
            this.indexOfFirstCharacter = indexOfFirstCharacter;
            this.indexOfLastCharacter = indexOfLastCharacter;
            this.startNode = parentNode;
            this.endNode = Node.Count++;
        }

        public Edge(Edge edge)
        {
            this.startNode = edge.startNode;
            this.endNode = edge.endNode;
            this.indexOfFirstCharacter = edge.indexOfFirstCharacter;
            this.indexOfLastCharacter = edge.indexOfLastCharacter;
            this.theString = edge.theString;
        }

        public void Copy(Edge edge)
        {
            this.startNode = edge.startNode;
            this.endNode = edge.endNode;
            this.indexOfFirstCharacter = edge.indexOfFirstCharacter;
            this.indexOfLastCharacter = edge.indexOfLastCharacter;
            this.theString = edge.theString;
        }

        static public void Insert(string theString, Dictionary<int, Edge> edges,ref Edge edge)
        {
            int i = Hash(edge.startNode, theString[edge.indexOfFirstCharacter]);
            if (!edges.ContainsKey(i))
            {
                edges.Add(i, new Edge(theString));
            }
            while (edges[i].startNode != -1)
            {
                i = ++i % HASH_TABLE_SIZE;
                if (!edges.ContainsKey(i))
                {
                    edges.Add(i, new Edge(theString));
                }

            }
            edges[i] = edge;
        }

        static public void Remove(string theString, Dictionary<int, Edge> edges,ref Edge edge)
        {
            edge.Copy(edge);
            int i = Hash(edge.startNode, theString[edge.indexOfFirstCharacter]);
            while (edges[i].startNode != edge.startNode || edges[i].indexOfFirstCharacter != edge.indexOfFirstCharacter)
            {
                i = ++i % HASH_TABLE_SIZE;
            }

            for (; ; )
            {
                //edges[i].startNode = -1;
                Edge tempEdge = edges[i];
                tempEdge.startNode = -1;
                edges[i] = tempEdge;
                int j = i;
                for (; ; )
                {
                    i = ++i % HASH_TABLE_SIZE;
                    if (!edges.ContainsKey(i))
                    {
                        edges.Add(i, new Edge(theString));
                    }
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
                edges[j].Copy(edges[i]);
            }
        }

        static public int SplitEdge(Suffix s, string theString, Dictionary<int, Edge> edges, Dictionary<int, Node> nodes,ref Edge edge)
        {
            Remove(theString, edges, ref edge);
            Edge newEdge = new Edge(theString, edge.indexOfFirstCharacter,
                edge.indexOfFirstCharacter + s.indexOfLastCharacter 
                - s.indexOfFirstCharacter, s.originNode);
            Edge.Insert(theString, edges,ref newEdge);
            //nodes[newEdge.endNode].suffixNode = s.originNode;
            //newEdge.Insert();
            if (nodes.ContainsKey(newEdge.endNode))
            {
                nodes[newEdge.endNode].suffixNode = s.originNode;
            }
            else
            {
                Node newNode = new Node();
                newNode.suffixNode = s.originNode;
                nodes.Add(newEdge.endNode, newNode);
            }

            edge.indexOfFirstCharacter += s.indexOfLastCharacter - s.indexOfFirstCharacter + 1;
            edge.startNode = newEdge.endNode;
            Edge.Insert(theString, edges,ref edge);
            //Insert();
            return newEdge.endNode;
           
        }

        static public Edge Find(string theString, Dictionary<int, Edge> edges, int node, int c)
        {
            int i = Hash(node, c);
            for (; ; )
            {
                if (!edges.ContainsKey(i))
                {
                    edges.Add(i,new Edge(theString));
                }
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
            int rtnValue = ((node << 8) + c) % (int)HASH_TABLE_SIZE;
            if (rtnValue == 1585)
            {
                rtnValue = 1585;
            }
            return rtnValue;
        }
    }
}
