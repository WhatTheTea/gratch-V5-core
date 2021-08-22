using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using gratch_core;

namespace gratch_core_tests.Database
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class Init
    {
        [TestMethod]
        public void Basic()
        {
            new gratch_core.Models.GroupRepository().DeleteAll();

            var rep = new gratch_core.Models.GroupRepository();
            var group = DataFiller.GetGroup(20);

            var expected = group[0];
            var actual = rep.GetPerson(group[0].Name, group.ToModel()).ToPerson();

            Assert.AreEqual(expected, actual);
        }
    }
}