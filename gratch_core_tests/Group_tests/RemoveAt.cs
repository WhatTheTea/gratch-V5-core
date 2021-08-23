
using gratch_core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace gratch_core_tests.Group_tests
{
    [TestClass]
    public class RemoveAt
    {
        [TestMethod]
        public void Default()
        {

            const int index = 9;

            var group = DataFiller.GetGroup(20);
            var deletedPerson = group[index];

            group.RemoveAt(index);

            Assert.IsFalse(group.Contains(deletedPerson));
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
