using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Extensions.Tests
{
    [TestClass()]
    public class MyListTests
    {
        [TestMethod()]
        public void AddTest()
        {
            int[] a = { 1, 2, 3, 4 };
             a = a.Add(5);
            Assert.AreEqual(5,a[4]);
           
        }
    }
}