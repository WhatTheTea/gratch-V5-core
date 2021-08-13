using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using gratch_core;

namespace gratch_core_tests
{
    internal static class DataFiller
    {
        private static int gIterator = 1;
        private static int pIterator = 1;
        private static Person GetPerson()
        {
            var result = new Person($"Grp: {gIterator} Person: {pIterator}");
            pIterator++;
            return result;
        }
        internal static Group GetGroup(int peoplecount)
        {
            var group = new Group();
            group.People = new List<Person>();
            for (int i = 0; i < peoplecount; i++) group.People.Add(GetPerson());
            group.AssignDutyDates();

            gIterator++;

            return group;
        }
    }
}
