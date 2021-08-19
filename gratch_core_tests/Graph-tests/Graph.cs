﻿using gratch_core;

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
            var group = DataFiller.GetGroup(DateTime.Now.DaysInMonth());

            Assert.AreEqual(group[0], group.Graph[DateTime.Now.FirstDayOfMonth()]);
        }
    }
}
