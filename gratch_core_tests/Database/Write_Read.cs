using gratch_core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Linq;

namespace gratch_core_tests.Database
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class Write_Read
    {
        [TestMethod]
        public void BasicReadWrite()
        {
            var rep = new gratch_core.Models.GroupRepository();
            var group = DataFiller.GetGroup(20);

            var expected = group[0];

            Group.listener.Destroy();
            var actual = DataFiller.Repository.GetGroup(group.Name).People[0];
            Group.listener = SQLiteListener.GetListener();

            Assert.IsTrue(expected.Name == actual.Name);
            Assert.IsTrue(expected.DutyDates.Count == actual.DutyDates.Count);
            for (int i = 0; i < expected.DutyDates.Count; i++)
            {
                Assert.AreEqual(expected.DutyDates[i], actual.DutyDates[i]);
            }
        }
        [TestMethod] //!Должен выполнятся после предыдущего
        public void ReadGroups()
        {
            BasicReadWrite();

            List<IGroup> groups = new();

            Group.listener.Destroy();
            foreach (var grp in Group.AllInstances) grp.Clear();
            Group.listener = SQLiteListener.GetListener();

            groups = DataFiller.Repository.LoadAllGroups();

            groups.ForEach(grp => Assert.IsTrue(grp.Any()));
            groups.ForEach(grp =>
            {
                foreach (var p in grp)
                {
                    Assert.IsTrue(p.Name != null);
                    Assert.IsTrue(p.DutyDates.Any());
                }
            });
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