using System;
using System.Collections.Generic;

namespace gratch_core
{
    public interface IGraph
    {
        Person this[DateTime dutyDate] { get; }

        IList<Person> AssignedPeople { get; }
        IList<DayOfWeek> Weekend { get; set; }
        IList<DateTime> Workdates { get; }

        void AssignEveryone(int startIndex = 0);

        void ClearAllAssignments();

        bool IsAssigned(DateTime date);

        bool IsHoliday(DateTime date);

        void MonthlyUpdate();
    }
}