﻿using gratch_core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;

namespace gratch_core_tests.Graph_tests
{
    [TestClass]
    public class AssignDutyDate
    {
        //private readonly int DaysInMonth = DateTime.Now.DaysInMonth();
        [TestMethod]
        public void Default()
        {
            //Arrange
            var group = DataFiller.GetGroup(DateTime.Now.DaysInMonth());
            //Act
            foreach (var person in group.People)
            {
                Assert.IsFalse(person.DutyDates == null);
                Assert.IsTrue(person.DutyDates.Count == 1);
            }
        }
        [TestMethod]
        public void FirstDayIsHoliday()
        {
            //Arrange
            Group testgroup = DataFiller.GetGroup(20);

            var dutydate = DateTime.Now.FirstDayOfMonth();
            //Act
            testgroup.Graph.Weekend.Add(dutydate.DayOfWeek);
            //Assert
            Assert.IsFalse(testgroup.Graph.IsAssigned(dutydate));
        }
        [TestMethod]
        public void OneWorkday()
        {
            //Arrange
            var group = DataFiller.GetGroup(DateTime.Now.DaysInMonth());
            //Act
            for (int i = 1; i <= 6; i++)
            {
                group.Graph.Weekend.Add((DayOfWeek)i);
            }
            //Assert
            foreach (var person in group.Graph.AssignedPeople)
            {
                Assert.AreEqual(DayOfWeek.Sunday, person.DutyDates.First().DayOfWeek);
                Assert.IsTrue(person.DutyDates.Count == 1);
            }
        }
    }
}
