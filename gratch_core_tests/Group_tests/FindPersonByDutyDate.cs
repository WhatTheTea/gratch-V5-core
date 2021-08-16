using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using gratch_core;

namespace gratch_core_tests.Group_tests
{
    [TestClass]
    public class FindPersonByDutyDate
    {
        [TestMethod]
        public void Default()
        {
            var group = DataFiller.GetGroup(20);
            var expectedPerson = group.People[9];
            var now = DateTime.Now;

            var date = new DateTime(now.Year, now.Month, 10);

            Person actualPerson;

            actualPerson = group.GetPerson(date);

            Assert.AreEqual(expectedPerson, actualPerson);

        }
    }
}
