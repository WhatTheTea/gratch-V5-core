﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using gratch_core;

namespace gratch_core_tests.Group_tests
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
            isFirstDay = grp.Workdates.First().Day == 1;
            isLastDay = grp.Workdates.Last().Day == DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            //Assert
            result = isFirstDay & isLastDay;
            Assert.IsTrue(result);
        }
        
    }
}