using gratch_core;
using gratch_core.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace gratch_core_tests.Graph_tests
{
    [TestClass]
    public class Graph
    {
        
        [TestMethod]
        public void Default()
        {
            DataFiller.Repository.DeleteAll();

            var group = DataFiller.GetGroup(DateTime.Now.DaysInMonth());

            Assert.AreEqual(group[0], group.Graph[DateTime.Now.FirstDayOfMonth()]);
        }
    }
}
