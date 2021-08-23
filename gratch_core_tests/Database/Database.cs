using gratch_core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Linq;

namespace gratch_core_tests.Database
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class Database
    {
        [TestMethod]
        public void BasicReadWrite()
        {
            var rep = new gratch_core.Models.GroupRepository();
            var group = DataFiller.GetGroup(20);

            var expected = group[0];
            var actual = DataFiller.Repository.GetGroup(group.Name).People[0].ToPerson();

            Assert.IsTrue(expected.Name == actual.Name);
            for (int i = 0; i < expected.DutyDates.Count; i++)
            {
                Assert.AreEqual(expected.DutyDates[i], actual.DutyDates[i]);
            }
        }
        [TestMethod] //!Должен выполнятся после предыдущего
        public void ReadGroups()
        {
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
        [ClassCleanup]
        public static void CleanUp()
        {
            DataFiller.Repository.DeleteAll();
            Group.listener.Destroy();
            foreach (var grp in Group.AllInstances) grp.Clear();
            Group.listener = SQLiteListener.GetListener();
        }
    }
}