using gratch_core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace gratch_core_tests.Group_tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class AllInstances
    {

        [TestMethod]
        public void Default()
        {
            DataFiller.Repository.DeleteAll();

            var group1 = DataFiller.GetGroup(10);
            var group2 = DataFiller.GetGroup(20);
            var group3 = DataFiller.GetGroup(30);

            Assert.AreEqual(group1, Group.AllInstances[^3]);
            Assert.AreEqual(group2, Group.AllInstances[^2]);
            Assert.AreEqual(group3, Group.AllInstances[^1]);
        }
        [TestMethod]
        public void Deletion()
        {
            DataFiller.Repository.DeleteAll();

            var group1 = DataFiller.GetGroup(10);
            var group2 = DataFiller.GetGroup(20);

            group1.Clear();
            group2.Clear();

            Assert.IsFalse(Group.AllInstances.Contains(group1));
            Assert.IsFalse(Group.AllInstances.Contains(group2));
        }
    }
}
