using gratch_core;
using gratch_core.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;

namespace gratch_core_tests.Graph_tests
{
    [TestClass]
    public class Weekend
    {
        private readonly int DaysInMonth = DateTime.Now.DaysInMonth();
        
        [TestMethod]
        public void Default()
        {
            DataFiller.Repository.DeleteAll();

            //Arrange
            var grp = DataFiller.GetGroup(20);
            DayOfWeek holiday = DayOfWeek.Sunday;
            bool isHolidayFound = false;
            //Act
            grp.Graph.Weekend.Add(holiday);

            foreach (var date in grp.Graph.Workdates)
            {
                if (date.DayOfWeek == holiday)
                {
                    isHolidayFound = true;
                    break;
                }
            }
            //Assert
            Assert.IsFalse(isHolidayFound);
        }
        [TestMethod]
        public void EverydayIsHoliday()
        {
            DataFiller.Repository.DeleteAll();

            var group = DataFiller.GetGroup(DaysInMonth);

            for (int i = 0; i < 7; i++)
            {
                group.Graph.Weekend.Add((DayOfWeek)i);
            }

            Assert.IsFalse(group.Graph.Workdates.Any());
        }
    }
}
