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
    public class ClearDutyDates
    {
        [TestMethod]
        public void Default()
        {
            var group = DataFiller.GetGroup(20);

            group.Graph.ClearAllAssignments();

            foreach(var person in group.People)
            {
                Assert.IsTrue(person.DutyDates == null);
            }
        }
    }
}
