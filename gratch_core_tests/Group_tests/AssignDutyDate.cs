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
    public class AssignDutyDate
    {
        [TestMethod]
        public void Default()
        {
            //Arrange
            var group = DataFiller.GetGroup(DateTime.Now.DaysInMonth());
            //AssignDutyDates() in GetGroup();
            bool isEveryoneAssigned = false;
            bool isEveryoneSingle = false;
            //Act
            foreach (var person in group.People)
            {
                if (person.DutyDates == null)
                {
                    isEveryoneAssigned = false;
                    break;
                }
                else isEveryoneAssigned = true;

                if (person.DutyDates.Count != 1)
                {
                    isEveryoneSingle = false;
                    break;
                }
                else isEveryoneSingle = true;
            }
            //Assert
            Assert.IsTrue(isEveryoneAssigned);
            Assert.IsTrue(isEveryoneSingle);
        }
        [TestMethod]
        public void FirstDayIsHoliday()
        {
            //Arrange
            Group testgroup = DataFiller.GetGroup(20);

            var dutydate = DateTime.Now.FirstDayOfMonth();
            //Act
            testgroup.Weekend.Add(dutydate.DayOfWeek);
            //Assert
            Assert.IsFalse(testgroup.IsDutyDateAssigned(dutydate));
        }
        [TestMethod]
        public void OneWorkday()
        {
            //Arrange
            var group = DataFiller.GetGroup(DateTime.Now.DaysInMonth());
            //Act
            for (int i = 1; i <= 6; i++)
            {
                group.Weekend.Add((DayOfWeek)i);
            }
            //Assert
            foreach (var person in group.AssignedPeople)
            {
                Assert.AreEqual(DayOfWeek.Sunday, person.DutyDates.First().DayOfWeek);
                Assert.IsTrue(person.DutyDates.Count == 1);
            }
        }
        [TestMethod]
        public void AllHolidays()
        {
            //Arrange
            var group = DataFiller.GetGroup(DateTime.Now.DaysInMonth());
            //Act
            for (int i = 1; i <= 7; i++)
            {
                if (i == 7)
                {
                    group.Weekend.Add(DayOfWeek.Sunday);
                    break;
                }
                group.Weekend.Add((DayOfWeek)i);
            }
            //Assert
            Assert.IsTrue(group.AssignedPeople.Count == 0);
        }
    }
}
