using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ACMF.UnitTests
{
    [TestClass]
    public class ModLoadOrderTest
    {
        [TestMethod]
        public void TestLoadOrderDeterministic()
        {
            Tuple<string, List<string>> one = new Tuple<string, List<string>>("568", new List<string>());
            Tuple<string, List<string>> two = new Tuple<string, List<string>>("1", new List<string>());
            Tuple<string, List<string>> three = new Tuple<string, List<string>>("74", new List<string>());
            Tuple<string, List<string>> four = new Tuple<string, List<string>>("9", new List<string>());

            Queue<string> output = ModLoader.ModLoader.GenerateModLoadOrder(new List<Tuple<string, List<string>>>()
            {
                one,
                two,
                three,
                four
            });

            Assert.AreEqual("568", output.Dequeue());
            Assert.AreEqual("1", output.Dequeue());
            Assert.AreEqual("74", output.Dequeue());
            Assert.AreEqual("9", output.Dequeue());
        }

        [TestMethod]
        public void TestLoadOrderReliantOnOther()
        {
            Tuple<string, List<string>> modOne = new Tuple<string, List<string>>("modOne", new List<string>());
            Tuple<string, List<string>> modTwo = new Tuple<string, List<string>>("modTwo", new List<string>()
            {
                "modOne",
                "modFour"
            });
            Tuple<string, List<string>> modThree = new Tuple<string, List<string>>("modThree", new List<string>()
            {
                "modTwo",
            });
            Tuple<string, List<string>> modFour = new Tuple<string, List<string>>("modFour", new List<string>());

            Queue<string> output = ModLoader.ModLoader.GenerateModLoadOrder(new List<Tuple<string, List<string>>>()
            {
                modOne,
                modTwo,
                modThree,
                modFour
            });

            Assert.AreEqual("modOne", output.Dequeue());
            Assert.AreEqual("modFour", output.Dequeue());
            Assert.AreEqual("modTwo", output.Dequeue());
            Assert.AreEqual("modThree", output.Dequeue());
        }

        [TestMethod]
        public void TestLoadOrderFailed()
        {
            Tuple<string, List<string>> one = new Tuple<string, List<string>>("def", new List<string>());
            Tuple<string, List<string>> two = new Tuple<string, List<string>>("abc", new List<string>());
            Tuple<string, List<string>> three = new Tuple<string, List<string>>("ghi", new List<string>()
            {
                "someUnknownMod"
            });

            Queue<string> output = ModLoader.ModLoader.GenerateModLoadOrder(new List<Tuple<string, List<string>>>()
            {
                one,
                two,
                three
            });

            Assert.AreEqual(false, output.Contains("someUnknownMod"));
            Assert.AreEqual(false, output.Contains("ghi"));
            Assert.AreEqual("def", output.Dequeue());
            Assert.AreEqual("abc", output.Dequeue());
        }
    }
}
