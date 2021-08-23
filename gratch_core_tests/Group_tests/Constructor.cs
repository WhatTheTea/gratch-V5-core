using gratch_core;
using gratch_core.Models;

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
            DataFiller.Repository.DeleteAll();

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
    }
}
