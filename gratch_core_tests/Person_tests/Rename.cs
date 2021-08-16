
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace gratch_core_tests.Person_tests
{
    [TestClass]
    public class Rename
    {
        [TestMethod]
        public void Conflict()
        {
            var group = DataFiller.GetGroup(20);
            var name = group.People[0].Name;

            group.People[4].Rename(name);

            Assert.AreNotEqual(name, group.People[4]);
        }
        [TestMethod]
        public void Succesful()
        {
            var group = DataFiller.GetGroup(20);
            var name = "Gosha";

            group.People[0].Rename(name);

            Assert.AreEqual(name, group.People[0].Name);
        }
    }
}
