
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace gratch_core_tests.Group_tests
{
    [TestClass]
    public class Remove
    {
        [TestMethod]
        public void Default()
        {
            const int index = 9;

            var group = DataFiller.GetGroup(20);
            var deletedPerson = group[index];

            group.Remove(group[index]);

            Assert.IsFalse(group.Contains(deletedPerson));
        }
    }
}
