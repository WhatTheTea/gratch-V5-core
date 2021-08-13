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
    public class AssignDutyDate
    {
        [TestMethod]
        public void Default()
        {
            //Arrange
            Group testgroup = DataFiller.GetGroup(20);

            var dutydate = DateTime.Now.FirstDayOfMonth();
            //Act
            testgroup.Weekend.Add(dutydate.DayOfWeek);
            //Assert
            Assert.IsFalse(testgroup.IsDutyDateAssigned(dutydate));
        }
    }
}
