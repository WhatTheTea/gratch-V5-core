using gratch_core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace gratch_core_tests.Group_tests
{
    [TestClass]
    public class FindPersonByDutyDate
    {
        [TestMethod]
        public void Default()
        {


            var group = DataFiller.GetGroup(20);
            var expectedPerson = group[9];
            var now = DateTime.Now;

            var date = new DateTime(now.Year, now.Month, 10);

            Person actualPerson;

            actualPerson = group.Graph[date];

            Assert.AreEqual(expectedPerson, actualPerson);

        }
        [TestCleanup]
        public void CleanUp() => DataFiller.CleanUp();
    }
}
