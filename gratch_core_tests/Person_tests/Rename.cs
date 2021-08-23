
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
            var name = group[0].Name;

            group[4].Rename(name);

            Assert.AreNotEqual(name, group[4]);
        }
        [TestMethod]
        public void Succesful()
        {
            var group = DataFiller.GetGroup(20);
            var name = "Gosha";

            group[0].Rename(name);

            Assert.AreEqual(name, group[0].Name);
        }
    }
}
