using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

using gratch_core;

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

            for(int i = 0; i < 20; i++)
            {
                names.Add("name" + rng.Next());
            }

            group = new Group(names);

            foreach(var person in group.People)
            {
                var index = group.People.IndexOf(person);
                Assert.IsTrue(person.Name == names[index]);
            }
        }
    }
}
