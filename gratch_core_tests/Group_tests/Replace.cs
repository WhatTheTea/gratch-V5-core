
using gratch_core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace gratch_core_tests.Group_tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class Replace
    {
        [TestMethod]
        public void Default()
        {


            var group = DataFiller.GetGroup(2);

            var person1 = group[0].Name;
            var person2 = group[1].Name;

            group.Replace(0, 1);

            Assert.AreEqual(person1, group[1].Name);
            Assert.AreEqual(person2, group[0].Name);
        }
        [TestCleanup]
        public void CleanUp() => DataFiller.CleanUp();
    }
}
