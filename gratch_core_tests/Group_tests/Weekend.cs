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
    public class Weekend
    {
        [TestMethod]
        public void Default()
        {
            //Arrange
            var grp = DataFiller.GetGroup(20);
            DayOfWeek holiday = DayOfWeek.Sunday;
            bool isHolidayFound = false;
            //Act
            grp.Weekend.Add(holiday);

            foreach (var date in grp.Workdates)
            {
                if (date.DayOfWeek == holiday)
                {
                    isHolidayFound = true;
                    break;
                }
            }
            //Assert
            Assert.IsFalse(isHolidayFound);
        }
    }
}
