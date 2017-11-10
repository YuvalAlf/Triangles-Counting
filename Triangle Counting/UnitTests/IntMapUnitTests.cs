using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.FSharp.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;

namespace UnitTests
{
    [TestClass]
    public class IntMapUnitTests
    {
        [TestMethod]
        public void AddTest()
        {
            var map = IntMap<string>.Empty.AddOrUpdate(1, "asd").AddOrUpdate(2, "sda");
            Assert.AreEqual("asd", map.TryFind(1).Value);
            Assert.AreEqual("sda", map.TryFind(2).Value);
            Assert.AreEqual(map.TryFind(3), FSharpOption<string>.None);
            Assert.AreEqual(map.TryFind(4), FSharpOption<string>.None);
            Assert.AreEqual(map.TryFind(5), FSharpOption<string>.None);
            Assert.AreEqual(map.TryFind(8), FSharpOption<string>.None);
        }
        [TestMethod]
        public void CountTest()
        {
            var map = IntMap<string>.Empty.AddOrUpdate(1, "asd").AddOrUpdate(1, "bgf").AddOrUpdate(3, "asd");
            Assert.AreEqual(2, map.Count());
        }
        [TestMethod]
        public void UpdateTest()
        {
            var map = IntMap<string>.Empty.AddOrUpdate(1, "asd").AddOrUpdate(1, "sda");
            Assert.AreEqual("sda", map.TryFind(1).Value);
        }
        [TestMethod]
        public void RemoveTest()
        {
            var map = IntMap<double>.Empty.AddOrUpdate(1, 2).AddOrUpdate(2, 4).AddOrUpdate(3, 0);
            map = map.Remove(1).Remove(3).Remove(4).Remove(2);
            Assert.AreEqual(0, map.Count());
        }
        [TestMethod]
        public void EnumerationTest()
        {
            var map = IntMap<string>.Empty.AddOrUpdate(1, "a").AddOrUpdate(2, "b").AddOrUpdate(3, "c");
            var fields = map.EnumerateFields().ToArray();
            Assert.IsTrue(fields.Contains(Tuple.Create(1, "a")));
            Assert.IsTrue(fields.Contains(Tuple.Create(2, "b")));
            Assert.IsTrue(fields.Contains(Tuple.Create(3, "c")));
            Assert.AreEqual(3, fields.Length);
        }
        [TestMethod]
        public void BuildTreeTest()
        {
            var array = new [] {Tuple.Create(1, 3), Tuple.Create(2, 4), Tuple.Create(5, 8) , Tuple.Create(7, 9) };
            var map = IntMap<int>.OfSortedArray(array);
            Assert.AreEqual(4, map.Count());
            map = map.AddOrUpdate(3, 5).AddOrUpdate(9, 8).AddOrUpdate(10, 11).AddOrUpdate(15, 18);
            Assert.IsTrue(map.ContainsKey(9));
            Assert.IsTrue(map.ContainsKey(2));
            Assert.IsTrue(map.ContainsKey(15));
            Assert.IsTrue(map.ContainsKey(7));
        }
    }
}
