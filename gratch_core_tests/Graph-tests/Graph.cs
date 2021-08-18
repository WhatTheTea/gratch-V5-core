using gratch_core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gratch_core_tests.Graph_tests
{
    [TestClass]
    public class Graph
    {
        [TestMethod]
        public void Default()
        {
            var group = DataFiller.GetGroup(DateTime.Now.DaysInMonth());

            Assert.AreEqual(group[0], group.Graph[DateTime.Now.FirstDayOfMonth()]);
        }
    }
}
