using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Extensions.Tests {
    [TestClass()]
    public class MyLinqTests {
        [TestMethod()]
        public void MaxTest() {
            int selector(string item) {
                return item.Length;
            }
            string[] TestData = { "DDD", "rehdbs", "", "   " };
            Assert.AreEqual(6, TestData.MaxValue(selector));
        }
    }
}