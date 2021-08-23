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
            DataFiller.Repository.DeleteAll();
            var group = DataFiller.GetGroup(20);

            Assert.IsTrue(group.Graph.IsAssigned(DateTime.Now.FirstDayOfMonth()));
        }
        [TestMethod]
        public void NotAssigned_Cleared()
        {
            DataFiller.Repository.DeleteAll();

            var group = DataFiller.GetGroup(20);

            group.Graph.ClearAllAssignments();

            Assert.IsFalse(group.Graph.IsAssigned(DateTime.Now.FirstDayOfMonth()));
        }
        [TestMethod]
        public void NotAssigned_Holiday()
        {
            DataFiller.Repository.DeleteAll();

            var group = DataFiller.GetGroup(20);

            group.Graph.Weekend.Add(DateTime.Now.FirstDayOfMonth().DayOfWeek);

            Assert.IsFalse(group.Graph.IsAssigned(DateTime.Now.FirstDayOfMonth()));
        }
    }
}
