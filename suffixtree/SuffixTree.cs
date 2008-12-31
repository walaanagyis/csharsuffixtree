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
using System.IO;

namespace Algorithms
{

    public class SuffixTree
    {
        Edge edges;
        public string theString;       
        public Dictionary<int, Edge> Edges = null;
        public Dictionary<int, Node> Nodes = null;
        public SuffixTree(string theString)
        {
            this.theString = theString;
            Nodes = new Dictionary<int, Node>(theString.Length * 2);
            Edges = new Dictionary<int, Edge>(theString.Length * 2);
            edges = new Edge(this.theString);
        }

        public void BuildTree()
        {
            Suffix active = new Suffix(this.theString, Edges, 0, 0, -1);
            for (int i = 0; i <= theString.Length - 1; i++)
            {
                AddPrefix(active, i);
            }
        }
        public static void Save(BinaryWriter writer, SuffixTree tree)
        {
            writer.Write(tree.Edges.Count);
            writer.Write(tree.theString.Length);
            writer.Write(tree.theString);
            foreach (KeyValuePair<int, Edge> edgePair in tree.Edges)
            {
                writer.Write(edgePair.Key);
                writer.Write(edgePair.Value.endNode);
                writer.Write(edgePair.Value.startNode);
                writer.Write(edgePair.Value.indexOfFirstCharacter);
                writer.Write(edgePair.Value.indexOfLastCharacter);
            }

        }


        public static void Save(Stream stream, SuffixTree tree)
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                Save(writer, tree);
            }
        }

        public static SuffixTree LoadFromFile(BinaryReader reader)
        {
            SuffixTree tree;
            int count = reader.ReadInt32();
            int theStringLength = reader.ReadInt32();
            string theString = reader.ReadString();
            tree = new SuffixTree(theString);
            for (int i = 0; i < count; i++)
            {
                int key = reader.ReadInt32();
                Edge readEdge = new Edge(theString);
                readEdge.endNode = reader.ReadInt32();
                readEdge.startNode = reader.ReadInt32();
                readEdge.indexOfFirstCharacter = reader.ReadInt32();
                readEdge.indexOfLastCharacter = reader.ReadInt32();
                tree.Edges.Add(key, readEdge);
            }
            return tree;
        }

        public static SuffixTree LoadFromFile(Stream stream)
        {
            SuffixTree tree;
            using (BinaryReader reader = new BinaryReader(stream))
            {
                tree = LoadFromFile(reader);
            }
            return tree;
        }


        public bool Search(string search)
        {
            search = search.ToLower();
            //try
            //{
                if (search.Length == 0)
                {
                    return false;
                }
                int index = 0;
                Edge edge;
                if (!this.Edges.TryGetValue((int)Edge.Hash(0, search[0]), out edge))
                {
                    return false;
                }                

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
                            char test = theString[j];
                            if (this.theString[j] != search[index++])
                            {
                                return false;
                            }
                        }
                        if (index < search.Length)
                        {
                            Edge value;                            
                            if (this.Edges.TryGetValue(Edge.Hash(edge.endNode, search[index]), out value))
                            {
                                edge = new Edge(value);
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                }

            //}
            //catch (KeyNotFoundException)
            //{
            //    return false;
            //}
        }

        public string[] DumpEdges()
        {
            List<string> edges = new List<string>();
            int count = this.theString.Length;
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
                Edge edge = new Edge(theString);
                parentNode = active.originNode;

                if (active.IsExplicit)
                {
                    edge = new Edge(Edge.Find(this.theString, this.Edges, active.originNode, theString[indexOfLastCharacter]));
                    if (edge.startNode != -1)
                    {
                        break;
                    }
                }
                else
                {
                    edge = new Edge(Edge.Find(this.theString, this.Edges, active.originNode, theString[active.indexOfFirstCharacter]));
                    int span = active.indexOfLastCharacter - active.indexOfFirstCharacter;
                    if (theString[edge.indexOfFirstCharacter + span + 1] == theString[indexOfLastCharacter])
                    {
                        break;
                    }
                    parentNode = Edge.SplitEdge(active, theString, Edges, Nodes, edge);
                }

                Edge newEdge = new Edge(this.theString, indexOfLastCharacter, this.theString.Length - 1 /*this.theString.Length - 1*/, parentNode);                
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
