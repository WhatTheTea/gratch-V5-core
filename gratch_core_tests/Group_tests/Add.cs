using gratch_core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;

namespace gratch_core_tests.Group_tests
{
    [TestClass]
    public class Add
    {

        [TestMethod]
        public void ByNameSuccessful()
        {


            var group = DataFiller.GetGroup(4);
            var name = "test";

            Person person;

            group.Add(name);

            Assert.IsTrue(group.Where(person => person.Name == name).Any());
            person = group.Where(person => person.Name == name).Single();
            Assert.IsFalse(person.DutyDates == null);
        }
        [TestMethod]
        public void ByNameDaysInMonth()
        {


            var group = DataFiller.GetGroup(DateTime.Now.DaysInMonth());
            var name = "test";

            Person person;

            group.Add(name);

            Assert.IsTrue(group.Where(person => person.Name == name).Any());
            person = group.Where(person => person.Name == name).First();
            Assert.IsTrue(!person.DutyDates.Any());
        }
        [TestMethod]
        public void ByNameUnsuccesful()
        {
            var group = DataFiller.GetGroup(4);
            var name = group[0].Name;

            Assert.ThrowsException<ArgumentException>(() => group.Add(name));
        }
        [TestCleanup]
        public void CleanUp() => DataFiller.CleanUp();
    }
}
