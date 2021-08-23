using gratch_core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}