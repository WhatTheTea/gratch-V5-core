using gratch_core;
using gratch_core.Models;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;

namespace gratch_core_tests.Unit
{
    [TestFixture]
    public class GroupTests
    {
        
        [TestCase(1, 1)]
        [TestCase(1, 40)]
        [TestCase(2, 1)]
        [TestCase(2, 40)]
        [TestCase(3, 1)]
        [TestCase(3, 40)]
        public void IGroup_AllInstances_IsWorkingProperly(int gCount, int pCount)
        {
            var gList = new List<Group>();

            for (int g = 0; g < gCount; g++)
            {
                gList.Add(DataFiller.GetGroup(pCount));
            }

            gList.ForEach(g => Assert.AreEqual(g.Name, new GroupRepository().GetAllGroups()[gList.IndexOf(g)].Name));
        }
        [Test]
        public void IGroup_AllInstances_Deletion_AllInstancesDontHaveEmptyGroups()
        {
            var group1 = DataFiller.GetGroup(10);
            var group2 = DataFiller.GetGroup(20);

            var name1 = group1.Name;
            var name2 = group2.Name;

            var repos = new GroupRepository();

            group1.Clear();
            group2.Clear();

            Assert.IsFalse(repos.GetAllGroups().Any(g => g.Name == name1));
            Assert.IsFalse(repos.GetAllGroups().Any(g => g.Name == name2));
        }
        [Test]
        public void Add_ByNameDaysInMonth_Exists()
        {
            Group group;
            Person person;
            string name;

            group = DataFiller.GetGroup(DateTime.Now.DaysInMonth());
            name = "test";
            group.Add(name);
            person = group.Where(person => person.Name == name).First();

            Assert.IsTrue(group.Where(person => person.Name == name).Any());
            Assert.IsFalse(person.DutyDates.Any());
        }
        [Test]
        public void Add_ByName_ThrowsException()
        {
            var group = DataFiller.GetGroup(4);
            var name = group[0].Name;

            Assert.Catch<ArgumentException>(() => group.Add(name));
        }
        [Test]
        public void Constructor_ByListOfNames_IsPersonCreated()
        {
            Random rng = new Random();
            var names = new List<string>();
            Group group;

            group = new Group("test", names);
            for (int i = 0; i < 20; i++)
            {
                names.Add("name" + rng.Next());
            }
            foreach (var person in group)
            {
                var index = group.IndexOf(person);
                Assert.IsTrue(person.Name == names[index]);
            }
        }
        [Test]
        public void Insert_Insert5WithDutyDate_PersonIsAfter5()
        {
            const int people = 9;
            var group = DataFiller.GetGroup(people);
            var person = new Person("test");
            person.DutyDates = new System.Collections.ObjectModel.Collection<DateTime> { DateTime.MinValue };

            group.Insert(5, new Person("test"));

            Assert.AreNotEqual(person, group[5]);
            Assert.AreEqual(person.Name, group[5].Name);
            Assert.AreEqual(people + 1, group.Count);
        }
        [Test]
        public void Remove_ByPerson_GroupDontHavePerson()
        {
            const int index = 9;

            var group = DataFiller.GetGroup(20);
            var deletedPerson = group[index];

            group.Remove(group[index]);

            Assert.IsFalse(group.Contains(deletedPerson));
        }
        [Test]
        public void Remove_ByIndex_GroupDontHavePerson()
        {
            const int index = 9;

            var group = DataFiller.GetGroup(20);
            var deletedPerson = group[index];

            group.RemoveAt(index);

            Assert.IsFalse(group.Contains(deletedPerson));
        }
        [Test]
        public void Replace_ByIndex_P1IsP2()
        {
            var group = DataFiller.GetGroup(2);

            var person1 = group[0].Name;
            var person2 = group[1].Name;

            group.Replace(0, 1);

            Assert.AreEqual(person1, group[1].Name);
            Assert.AreEqual(person2, group[0].Name);
        }
        [TearDown]
        public void TestCleanUp()
        {
            DataFiller.CleanUp();
        }
    }
}
