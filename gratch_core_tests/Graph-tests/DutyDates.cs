using gratch_core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace gratch_core_tests {
    [TestClass]
    public class DutyDates
    {
        [TestMethod]
        public void Default()
        {
            const int num1 = 1;
            const int num2 = 40;

            int dinm = DateTime.Now.DaysInMonth();

            var group1 = DataFiller.GetGroup(num1);
            var group2 = DataFiller.GetGroup(num2);

            Assert.AreEqual(dinm, group1.Graph.DutyDates.Count);
            Assert.AreEqual(dinm, group2.Graph.DutyDates.Count);
        }
    }
}