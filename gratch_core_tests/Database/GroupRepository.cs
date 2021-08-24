using gratch_core;
using gratch_core.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gratch_core_tests.Database
{
    [TestClass]
    public class GroupRepository
    {
        [TestMethod]
        public void RunCheck()
        {
            var group = DataFiller.GetGroup(20);
            foreach(var grp in DataFiller.Repository.GetAllGroups())
            {
                Console.WriteLine(DateTime.Now + " | GroupRepository.RunCheck | _id | Id | GrpID | Name | DutyDates");
                foreach (var p in grp.People)
                {
                    Console.WriteLine(DateTime.Now + $" | GroupRepository.RunCheck | {p._id} | {p.Id} | {p.GroupId} |" +
                        $" {p.Name} | {p.DutyDatesBlob}");
                }
            }
        }
        [TestCleanup]
        public void CleanUp() => DataFiller.CleanUp();
    }
}
