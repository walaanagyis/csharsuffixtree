using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using NUnit.Framework;
using Algorithms;
using System.IO;


namespace NUnitTests
{
    [TestFixture]
    public class UnitTests
    {
        protected string theString ="";
        protected List<string> individualStrings = new List<string>();

        [SetUp]
        public void SetUp()
        {
            
            using (StreamReader reader = new StreamReader(@"..\..\words\2of4brif.txt"))
            {
                while (!reader.EndOfStream)
                {
                    string individualString = reader.ReadLine();
                    theString += individualString;
                    individualStrings.Add(individualString);
                    Console.WriteLine("Adding: " + individualString);
                }
            }
        }

        [Test]
        public void VerifyValidWords()
        {
            SuffixTree tree = new SuffixTree(theString);
            tree.BuildTree();
            foreach (string individualString in individualStrings)
            {
                Assert.IsTrue(tree.Search(individualString));
            }
        }

        //[Test]
        //public void VerifyInvalidWords()

    }
}
