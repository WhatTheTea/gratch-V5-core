
using gratch_core;

namespace gratch_core_tests
{
    internal static class DataFiller
    {
        private static int gIterator = 1;
        private static int pIterator = 1;
        private static Person GetPerson(bool iterate = true)
        {
            var result = new Person($"Grp: {gIterator} Person: {pIterator}");
            if (iterate) pIterator++;
            return result;
        }
        internal static Group GetGroup(int peoplecount)
        {
            var group = new Group();

            for (int i = 0; i < peoplecount; i++) group.Add(GetPerson());

            gIterator++;
            
            return group;
        }
    }
}
