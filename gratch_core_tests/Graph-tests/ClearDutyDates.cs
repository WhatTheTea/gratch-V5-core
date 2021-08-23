using gratch_core.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace gratch_core_tests.Graph_tests
{
    [TestClass]
    public class ClearDutyDates
    {
        
        [TestMethod]
        public void Default()
        {
            DataFiller.Repository.DeleteAll();

            var group = DataFiller.GetGroup(20);

            group.Graph.ClearAllAssignments();

            foreach (var person in group)
            {
                Assert.IsTrue(person.DutyDates == null);
            }
        }
    }
}
