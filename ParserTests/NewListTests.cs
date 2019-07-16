using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using Parser.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Parser.Tests
{
    [TestClass()]
    public class NewListTests
    {
        [TestMethod()]
        public void AddTest()
        {
            NewList<int> a = new NewList<int>();
            a.Add(5);
            a.Add(10);
            a.Add(21);
            a.Add(12);
            Assert.AreEqual(12, a[3]);
        }

        [TestMethod()]
        public void ClearTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ContainsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CopyToTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetEnumeratorTest()
        {
            NewList<JSONObject> a = new NewList<JSONObject>();
            JSONObject Obj1 = new JSONObject();
            Obj1.FirstName = "FistName1";
            Obj1.LastName = "LastName1";
            JSONObject Obj2 = new JSONObject();
            Obj2.FirstName = "FirstName2";
            Obj2.LastName = "LastName2";
            a.Add(Obj1);
            a.Add(Obj2);
            var index = 0;
            string TestResult="";
            foreach (var item in a)
            {                            
                if (index == 1)
                {
                   TestResult = item.LastName;
                }
                index++;
            }
            Assert.AreEqual("LastName2",TestResult);
        }

        [TestMethod()]
        public void IndexOfTest()
        {
            NewList<int> a = new NewList<int>();
            a.Add(5);
            a.Add(10);
            a.Add(21);
            a.Add(12);
            Assert.AreEqual(2, a.IndexOf(21));
        }

        [TestMethod()]
        public void InsertTest()
        {
            NewList<int> a = new NewList<int>();
            a.Add(5);
            a.Add(10);
            a.Add(21);
            a.Add(12);
            a.Insert(2, 123);
            Assert.AreEqual(123, a[2]);
        }

        [TestMethod()]
        public void RemoveTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RemoveAtTest()
        {
            Assert.Fail();
        }
    }
}