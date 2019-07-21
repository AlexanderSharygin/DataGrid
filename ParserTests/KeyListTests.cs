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
    public class KeyListTests
    {
        [TestMethod()]
        public void FillKeyListTest()
        {
            KeyList kl = new KeyList();
            MyList<string> p_Keys = new MyList<string>();
            p_Keys.Add("Name");
            p_Keys.Add("Surname");
            p_Keys.Add("Range");
            MyList<string> r_Keys = new MyList<string>();
            r_Keys.Add("Name1");
            r_Keys.Add("Surname1");
            r_Keys.Add("Range");
            r_Keys.Add("Range1");
            kl.FillKeyList(p_Keys);
            kl.FillKeyList(r_Keys);
            Assert.AreEqual("Range1", kl[5]);


        }
        [TestMethod()]
        public void IndexatorTest()
        {
            KeyList kl = new KeyList();
           
            kl.Add("Name");
            kl.Add("Surname");
            kl.Add("Range");
            kl.Add("Name1");
            kl.Add("Surname1");
            kl.Add("Range");
            kl.Add("Range1");
             Assert.AreEqual("Range1", kl[5]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => kl[6]);

        }
        [TestMethod()]
        public void AddTest()
        {
            KeyList kl = new KeyList();

            kl.Add("Name");
            kl.Add("Surname");
            kl.Add("Range");
            kl.Add("Name1");
            kl.Add("Surname1");
            kl.Add("Range");
            kl.Add("Range1");
          
            Assert.AreEqual("Range1", kl[5]);
            Assert.AreEqual(6, kl.Count);
            

        }
    }
}