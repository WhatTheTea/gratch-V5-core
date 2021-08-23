using gratch_core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Linq;

namespace gratch_core_tests.Database
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class Init
    {
        [TestMethod]
        public void BasicReadWrite()
        {
            new gratch_core.Models.GroupRepository().DeleteAll();

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
            BasicReadWrite();
            List<Group> groups = new();

            var models = new gratch_core.Models.GroupRepository().GetGroups();
            foreach (var grp in Group.AllInstances) grp.Clear();
            foreach (var mod in models) groups.Add(mod.ToGroup());

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
    }
}