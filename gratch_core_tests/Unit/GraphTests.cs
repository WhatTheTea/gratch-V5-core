using gratch_core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;

namespace gratch_core_tests.Unit
{
    [TestClass]
    public class GraphTests
    {
        [TestMethod]
        public void AssignEveryone_PeopleСountIsDINM_DutydatesNotNullOrZero()
        {
            //Arrange
            int daysinmonth = DateTime.Now.DaysInMonth();
            var group = DataFiller.GetGroup(daysinmonth);
            //Act
            foreach (var person in group)
            {
                Assert.IsFalse(person.DutyDates == null);
                Assert.IsTrue(person.DutyDates.Count == 1);
            }
        }
        [TestMethod]
        public void AssignEveryone_FirstDayIsHoliday_DutydatesDontHaveHoliday()
        {
            //Arrange
            Group testgroup = DataFiller.GetGroup(20);

            var dutydate = DateTime.Now.FirstDayOfMonth();
            //Act
            testgroup.Graph.Weekend = new List<DayOfWeek> { dutydate.DayOfWeek };
            //Assert
            Assert.IsFalse(testgroup.Graph.IsAssigned(dutydate));
        }
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(0, 1)]
        [DataRow(0, 1, 2)]
        [DataRow(0, 1, 2, 3)]
        [DataRow(0, 1, 2, 3, 4)]
        [DataRow(0, 1, 2, 3, 4, 5)]
        [DataRow(0, 1, 2, 3, 4, 5, 6)]
        public void AssignEveryone_Holidays_DutydatesDontHaveHolidays(params int[] intdays)
        {
            //Arrange
            var group = DataFiller.GetGroup(DateTime.Now.DaysInMonth());
            var holidays = new List<DayOfWeek>(intdays.Cast<DayOfWeek>());
            //Act
            group.Graph.Weekend = holidays;
            //Assert
            foreach (var person in group.Graph.AssignedPeople)
            {
                person?.DutyDates?.ToList()?.ForEach(dd =>
               {
                   Assert.IsFalse(holidays.Any(hd => hd == dd.DayOfWeek));
               });
            }
        }
        [DataTestMethod]
        [DataRow(1)]
        [DataRow(DataFiller.dinm_flag)]
        public void Brackets_GetPersonByDutyDate_ReturnsPerson(int DoF)
        {
            DoF = DoF == DataFiller.dinm_flag ? DateTime.Now.DaysInMonth() : DoF;

            Group group;
            Person expected;
            Person actual;

            group = DataFiller.GetGroup(DateTime.Now.DaysInMonth());
            expected = group[DoF - 1];
            actual = group.Graph[new DateTime(DateTime.Now.Year, DateTime.Now.Month, DoF)];

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ClearAllAssignments_ClearAndCheck_PersonDontHaveDD()
        {
            var group = DataFiller.GetGroup(20);

            group.Graph.ClearAllAssignments();

            foreach (var person in group)
            {
                Assert.IsFalse(person.DutyDates.Any());
            }
        }
        [TestMethod]
        public void AssignedPeople_Check_ReturnPeople()
        {
            Group group;
            IList<Person> assigned;

            group = DataFiller.GetGroup(20);
            assigned = group.Graph.AssignedPeople;

            for (int i = 0; i < assigned.Count; i++)
            {
                Assert.IsTrue(group[i] == assigned[i]);
            }
        }
        [DataTestMethod]
        [DataRow(20)] // LessThanDINM
        [DataRow(40)] // MoreThanDINM
        public void UpdateDutyDates_VariousPersCount_LastPersonIsFirst(int pCount)
        {
            //Arrange
            var dinm = DateTime.Now.DaysInMonth();

            Group group;
            Person expectedPerson;
            Person actualPerson;
            int pIndex;
            //Act
            pIndex = pCount > dinm ? dinm : dinm - pCount;

            group = DataFiller.GetGroup(pCount);
            expectedPerson = group[pIndex];
            group.Graph.MonthlyUpdate();
            //Assert
            actualPerson = group.Graph[group.Graph.Workdates.First()];
            Assert.AreEqual(expectedPerson, actualPerson);
        }
        [TestCleanup]
        public void CleanUp()
        {
            DataFiller.CleanUp();
        }
    }
}
