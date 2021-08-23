﻿using gratch_core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

namespace gratch_core_tests.Group_tests
{
    [TestClass]
    public class Constructor
    {
        [TestMethod]
        public void Default()
        {


            Random rng = new Random();
            var names = new List<string>();
            Group group;

            for (int i = 0; i < 20; i++)
            {
                names.Add("name" + rng.Next());
            }

            group = new Group("test", names);

            foreach (var person in group)
            {
                var index = group.IndexOf(person);
                Assert.IsTrue(person.Name == names[index]);
            }
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
