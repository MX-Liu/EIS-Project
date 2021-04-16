using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Tests
{
    [TestClass()]
    public class UnitTestClassTests
    {
        [TestMethod()]
        public void GetTriangleTest()
        {
            string[] sideArr = { "5", "5", "5" };
            Assert.AreEqual("等边三角形", UnitTestClass.GetTriangle(sideArr));
        }

        [TestMethod()]
        public void fest_strTest()
        {
            string input = "100";
            int n = 4;
            Assert.AreEqual("0100", UnitTestClass.fest_str(input, n));
        }

        [TestMethod()]
        public void strToHexTest()
        {
            Assert.AreEqual("33E3D", UnitTestClass.strToHex("0212541"));
        }

        [TestMethod()]
        public void exchangeTest()
        {
            byte[] input = new byte[] { 0, 1, 8, 3 };
            byte[] buff = new byte[] { 0, 1, 8, 3 };
            byte[] output = UnitTestClass.exchange(input);
            int j = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (buff[i] == output[input.Length - i - 1])
                    j++;
            }
            Assert.AreEqual(4, j);
        }

        [TestMethod()]
        public void TimeToSecTest()
        {
            Assert.AreEqual("60", UnitTestClass.TimeToSec("00:01:00"));
        }

        [TestMethod()]
        public void strToBCDByteTest()
        {
            string input = "0212541";
            string output = "";

            byte[] buff = UnitTestClass.strToBCDByte(input);
            for (int i = 0; i < buff.Length; i++)
            {
                output += buff[i];
            }
            Assert.AreEqual("54897", output);
        }

        [TestMethod()]
        public void strToHexByteTest()
        {
            string input = "0212541";
            string output = "";

            byte[] buff = UnitTestClass.strToHexByte(input);
            for (int i = 0; i < buff.Length; i++)
            {
                output += buff[i];
            }
            Assert.AreEqual("36261", output);
        }

        [TestMethod()]
        public void trianglewavTest()
        {
            Assert.AreEqual(0, UnitTestClass.trianglewav(98,9));
        }

        [TestMethod()]
        public void SquarewavTest()
        {
            Assert.AreEqual(0, UnitTestClass.Squarewav(98, 9));
        }
    }

}