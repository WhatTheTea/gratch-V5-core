using gratch_core;
using gratch_core.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;

namespace gratch_core_tests.Graph_tests
{
    [TestClass]
    public class UpdateDutyDates
    {
        
        [TestMethod]
        public void LessThanDaysInMonth()
        {
            DataFiller.Repository.DeleteAll();

            //Arrange
            const int persCount = 20;

            var group = DataFiller.GetGroup(persCount);
            var expectedIndex = DateTime.Now.DaysInMonth() - persCount;
            var expectedPerson = group[expectedIndex];

            Person actualPerson;
            //Act
            group.Graph.AssignEveryone();
            group.Graph.MonthlyUpdate();
            //Assert
            actualPerson = group.Graph[group.Graph.Workdates.First()];
        }
        [TestMethod]
        public void MoreThanDaysInMoth()
        {
            DataFiller.Repository.DeleteAll();

            //Arrange
            const int persCount = 40;

            var group = DataFiller.GetGroup(persCount);
            var expectedPerson = group[DateTime.Now.DaysInMonth()];
            Person actualPerson;
            //Act
            group.Graph.AssignEveryone();
            group.Graph.MonthlyUpdate();
            //Assert
            actualPerson = group.Graph[group.Graph.Workdates.First()];
            Assert.AreEqual(expectedPerson, actualPerson);
        }
    }
}
