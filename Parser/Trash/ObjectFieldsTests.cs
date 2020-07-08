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
    public class ObjectFieldsTests
    {
        [TestMethod()]
        public void IndexatorTest()
        {
            ObjectFields a = new ObjectFields();
            a.Add("LastName", "Shvine");
            a.Add("FirstName", "Alex");
            a.Add("Range", "Unter Offecurer");
            a.Add("Gruppen", "Doicen Seldaten");
            a.Add("Range", "Ober Leitenant");
            Assert.AreEqual("Shvine", a["LastName"]);
            Assert.ThrowsException<KeyNotFoundException>(() => a["shvine"]);
            
        }

        [TestMethod()]
        public void AddFieldTest()
        {
            ObjectFields a = new ObjectFields();
            a.Add("LastName", "Shvine");
            a.Add("FirstName", "Alex");
            a.Add("Range", "Unter Offecurer");
            a.Add("Gruppen", "Doicen Seldaten");
            a.Add("Range", "Ober Leitenant");
            string Range = a["Range"];
            Assert.AreEqual(Range, "Ober Leitenant");
        }
    }
}