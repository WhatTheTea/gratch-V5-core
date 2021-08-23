﻿using gratch_core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace gratch_core_tests.Group_tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class Insert
    {

        [TestMethod]
        public void Default()
        {


            const int people = 9;
            var group = DataFiller.GetGroup(people);
            var person = new Person("test");
            person.DutyDates = new();
            person.DutyDates.Add(DateTime.MinValue);

            group.Insert(5, person);

            Assert.AreNotEqual(person, group[5]);
            Assert.AreEqual(person.Name, group[5].Name);
            Assert.AreEqual(people + 1, group.Count);
        }
        [TestCleanup]
        public void CleanUp()
        {
            DataFiller.Repository.DeleteAll();
            Group.listener.Destroy();
            foreach (var grp in Group.AllInstances) grp.Clear();
            Group.listener = SQLiteListener.GetListener();
        }
    }
}
