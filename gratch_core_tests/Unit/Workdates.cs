
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace gratch_core_tests.Unit
{
    [TestClass]
    public class Workdates
    {

        [TestMethod]
        public void Default()
        {


            //Arrange
            var grp = DataFiller.GetGroup(20);
            bool isFirstDay = false;
            bool isLastDay = false;
            bool result;
            //Act
            isFirstDay = grp.Graph.Workdates[0].Day == 1;
            isLastDay = grp.Graph.Workdates[^1].Day == DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            //Assert
            result = isFirstDay & isLastDay;
            Assert.IsTrue(result);
        }
        [TestCleanup]
        public void CleanUp() => DataFiller.CleanUp();
    }
}
