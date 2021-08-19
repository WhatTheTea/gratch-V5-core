using gratch_core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace gratch_core_tests.Graph_tests
{
    [TestClass]
    public class IsDutyDateAssigned
    {
        [TestMethod]
        public void Assigned()
        {
            var group = DataFiller.GetGroup(20);

            Assert.IsTrue(group.Graph.IsAssigned(DateTime.Now.FirstDayOfMonth()));
        }
        [TestMethod]
        public void NotAssigned_Cleared()
        {
            var group = DataFiller.GetGroup(20);

            group.Graph.ClearAllAssignments();

            Assert.IsFalse(group.Graph.IsAssigned(DateTime.Now.FirstDayOfMonth()));
        }
        [TestMethod]
        public void NotAssigned_Holiday()
        {
            var group = DataFiller.GetGroup(20);

            group.Graph.Weekend.Add(DateTime.Now.FirstDayOfMonth().DayOfWeek);

            Assert.IsFalse(group.Graph.IsAssigned(DateTime.Now.FirstDayOfMonth()));
        }
    }
}
