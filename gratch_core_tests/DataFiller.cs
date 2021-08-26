
using gratch_core;

using System;
using System.Collections.Generic;

namespace gratch_core_tests
{
    internal static class DataFiller
    {
        internal static gratch_core.Models.GroupRepository Repository = new();

        internal const int dinm_flag = -5;

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
            pIterator = 1;
            var people = new List<Person>();
            var group = new Group($"Grp{gIterator}");

            for (int i = 0; i < peoplecount; i++) group.Add(GetPerson());
            group.Graph.AssignEveryone();
            gIterator++;

            return group;
        }
        public static void ResetIterators()
        {
            gIterator = 1;
            pIterator = 1;
        }
        internal static void CleanUp()
        {
            DataFiller.Repository.DeleteAll();

            Group.subscriber?.Destroy();
            foreach (var grp in IGroup.AllInstances) grp.Clear();
            Group.subscriber = SQLiteSubscriber.GetSubscriber();

            DataFiller.ResetIterators();
        }
        internal static void ReturnTable()
        {
            foreach (var grp in DataFiller.Repository.GetAllGroups())
            {
                Console.WriteLine(DateTime.Now + " | Table |_id|Id|GrpID| Name | DutyDates");
                foreach (var p in grp.People)
                {
                    Console.WriteLine(DateTime.Now + $" | Table | {p._id} |  {p.Id} | {p.GroupId} |" +
                        $" {p.Name} | {p.DutyDatesBlob}");
                }
            }
        }
    }
}
