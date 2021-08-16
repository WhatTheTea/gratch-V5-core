using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using gratch_core;

namespace gratch_core_tests.Graph_tests
{
    [TestClass]
    public class UpdateDutyDates
    {
        [TestMethod]
        public void LessThanDaysInMonth()
        {
            //Arrange
            const int persCount = 20;

            var group = DataFiller.GetGroup(persCount);
            var expectedIndex = DateTime.Now.DaysInMonth() - persCount;
            var expectedPerson = group.People[expectedIndex];

            Person actualPerson;
            //Act
            group.Graph.AssignEveryone();
            group.Graph.MonthlyUpdate();
            //Assert
            actualPerson = group.GetPerson(group.Graph.Workdates.First());
        }
        [TestMethod]
        public void MoreThanDaysInMoth()
        {
            //Arrange
            const int persCount = 40;

            var group = DataFiller.GetGroup(persCount);
            var expectedPerson = group.People[DateTime.Now.DaysInMonth()];
            Person actualPerson;
            //Act
            group.Graph.AssignEveryone();
            group.Graph.MonthlyUpdate();
            //Assert
            actualPerson = group.GetPerson(group.Graph.Workdates.First());
            Assert.AreEqual(expectedPerson, actualPerson);
        }
    }
}
