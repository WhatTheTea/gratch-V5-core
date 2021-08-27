﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace gratch_core_tests.Unit
{
    [TestClass]
    public class PersonTests
    {
        [TestMethod]
        public void Rename_UnSuccessful_PersonIsNotRenamed()
        {
            var group = DataFiller.GetGroup(20);
            var name = group[0].Name;

            group[4].Rename(name);

            Assert.AreNotEqual(name, group[4]);
        }
        [TestMethod]
        public void Rename_Succesful_PersonRenamed()
        {
            var group = DataFiller.GetGroup(20);
            var name = "Gosha";

            group[0].Rename(name);

            Assert.AreEqual(name, group[0].Name);
        }
        [TestCleanup]
        public void CleanUp()
        {
            DataFiller.CleanUp();
        }
    }
}