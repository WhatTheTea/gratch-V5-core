using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using gratch_core;

namespace gratch_core_tests.Group_tests
{
    [TestClass]
    public class AssignedPeople
    {
        [TestMethod]
        public void Default()
        {
            //Arrange
            var group = DataFiller.GetGroup(20);
            var assigned = group.AssignedPeople;

            bool result = true;
            //Act
            for(int i = 0; i < assigned.Count(); i++)
            {
                if(group.People[i] != assigned[i])
                {
                    result = false;
                    break;
                }
            }
            //Assert
            Assert.IsTrue(result);
        }
    }
}
