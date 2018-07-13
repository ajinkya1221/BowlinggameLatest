using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObjectComparer.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var st1 = new Student
            {
                Id = 100,
                Name = "Ajinkya",
                Marks = new[] { 1, 2, 3 }
            };

            var st2 = new Student
            {
                Id = 100,
                Name = "Ajinkya",
                Marks = new[] { 1, 2, 3 }
            };
            Assert.IsTrue(Comparer.Comparer.AreSimilar(st1, st2));
        }
    }
}
