using gratch_core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace gratch_core_tests.Group_tests
{
    [TestClass]
    public class People
    {
        [TestMethod]
        public void ReadOnlyTest()
        {
            var group = DataFiller.GetGroup(5);

            Assert.ThrowsException<NotSupportedException>(() => group.People.Add(new Person("name")));
        }
    }
}
