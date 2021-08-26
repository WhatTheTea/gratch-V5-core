
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq;

namespace gratch_core_tests.Unit
{
    [TestClass]
    public class ClearDutyDates
    {

        [TestMethod]
        public void Default()
        {


            var group = DataFiller.GetGroup(20);

            group.Graph.ClearAllAssignments();

            foreach (var person in group)
            {
                Assert.IsTrue(!person.DutyDates.Any());
            }
        }
        [TestCleanup]
        public void CleanUp() => DataFiller.CleanUp();
    }
}
