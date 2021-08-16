using gratch_core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gratch_core_tests.Group_tests
{
    [TestClass]
    public class Remove
    {
        [TestMethod]
        public void Default()
        {
            const int index = 9;

            var group = DataFiller.GetGroup(20);
            var deletedPerson = group.People[index];

            group.Remove(index);

            Assert.IsFalse(group.People.Contains(deletedPerson));
        }
    }
}
