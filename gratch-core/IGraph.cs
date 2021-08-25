using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace gratch_core
{
    public interface IGraph
    {
        Person this[DateTime dutyDate] { get; }

        IList<Person> AssignedPeople { get; }
        Collection<DayOfWeek> Weekend { get; set; }
        IList<DateTime> Workdates { get; }

        void AssignEveryone(int startIndex = 0);
        void ClearAllAssignments();
        bool IsAssigned(DateTime date);
        bool IsHoliday(DateTime date);
        void MonthlyUpdate();
    }
}