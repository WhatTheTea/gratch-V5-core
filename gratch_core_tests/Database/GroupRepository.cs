
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace gratch_core_tests.Database
{
    [TestClass]
    public class GroupRepository
    {
        [TestMethod]
        public void RunCheck()
        {
            var group = DataFiller.GetGroup(20);
            
        }
        [TestCleanup]
        public void CleanUp() => DataFiller.CleanUp();
    }
}
