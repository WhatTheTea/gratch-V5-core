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
    public class Add
    {
        [TestMethod]
        public void ByNameSuccessful()
        {
            var group = DataFiller.GetGroup(4);
            var name = "test";

            group.Add(name);

            Assert.IsTrue(group.People.Where(person => person.Name == name).Any());
        }
        [TestMethod]
        public void ByNameUnsuccesful()
        {
            var group = DataFiller.GetGroup(4);
        }
    }
}
