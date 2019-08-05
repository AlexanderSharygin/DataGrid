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
        public void AddKeysFromObjectTest()
        {
            AgregatedKeyList kl = new AgregatedKeyList();
            JSONObject js1 = new JSONObject();
            MyList<string> p_Keys = new MyList<string>();
            p_Keys.Add("Name");
            p_Keys.Add("Surname");
            p_Keys.Add("Range");
            js1.Fields.Keys = p_Keys;
            MyList<string> r_Keys = new MyList<string>();
            JSONObject js2 = new JSONObject();
            r_Keys.Add("Name1");
            r_Keys.Add("Surname1");
            r_Keys.Add("Range");
            r_Keys.Add("Range1");
            js2.Fields.Keys = r_Keys;
            kl.AddKeysFromObject(js1);
            kl.AddKeysFromObject(js2);
            Assert.AreEqual("Range1", kl[5]);


        }
        [TestMethod()]
        public void IndexatorTest()
        {
            AgregatedKeyList kl = new AgregatedKeyList();
           
            kl.Add("Name");
            kl.Add("Surname");
            kl.Add("Range");
            kl.Add("Name1");
            kl.Add("Surname1");
            kl.Add("Range");
            kl.Add("Range1");
            Assert.AreEqual("Range1", kl[5]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => kl[20]);

        }
        [TestMethod()]
        public void AddTest()
        {
            AgregatedKeyList kl = new AgregatedKeyList();

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