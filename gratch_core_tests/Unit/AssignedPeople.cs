
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq;

namespace gratch_core_tests.Unit
{
    [TestClass]
    public class AssignedPeople
    {

        [TestMethod]
        public void Default()
        {
            //Arrange
            var group = DataFiller.GetGroup(20);
            var assigned = group.Graph.AssignedPeople;

            bool result = true;
            //Act
            for (int i = 0; i < assigned.Count(); i++)
            {
                if (group[i] != assigned[i])
                {
                    result = false;
                    break;
                }
            }
            //Assert
            Assert.IsTrue(result);
        }
        [TestCleanup]
        public void CleanUp() => DataFiller.CleanUp();
    }
}
