using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using gratch_core;

namespace gratch_core_tests.Group_tests
{
    [TestClass]
    public class IsDutyDateAssigned
    {
        [TestMethod]
        public void Assigned()
        {
            var group = DataFiller.GetGroup(20);

            Assert.IsTrue(group.IsAssigned(DateTime.Now.FirstDayOfMonth()));
        }
        [TestMethod]
        public void NotAssigned_Cleared()
        {
            var group = DataFiller.GetGroup(20);

            group.ClearAllAssignments();

            Assert.IsFalse(group.IsAssigned(DateTime.Now.FirstDayOfMonth()));
        }
        [TestMethod]
        public void NotAssigned_Holiday()
        {
            var group = DataFiller.GetGroup(20);

            group.Weekend.Add(DateTime.Now.FirstDayOfMonth().DayOfWeek);

            Assert.IsFalse(group.IsAssigned(DateTime.Now.FirstDayOfMonth()));
        }
    }
}
