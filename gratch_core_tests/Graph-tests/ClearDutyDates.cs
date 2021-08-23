
using gratch_core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq;

namespace gratch_core_tests.Graph_tests
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
        public void CleanUp()
        {
            DataFiller.Repository.DeleteAll();
            Group.listener.Destroy();
            foreach (var grp in Group.AllInstances) grp.Clear();
            Group.listener = SQLiteListener.GetListener();
        }
    }
}
