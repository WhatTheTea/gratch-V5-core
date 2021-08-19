
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("gratch_core_tests")]

namespace gratch_core
{
    public class Graph
    {
        private List<Person> people;
        public IList<Person> AssignedPeople => (from p in people
                                                where p.DutyDates != null
                                                select p).ToList().AsReadOnly();
        public ObservableCollection<DayOfWeek> Weekend { get; set; } = new ObservableCollection<DayOfWeek>();
        public IList<DateTime> Workdates
        {
            get
            {
                var value = new List<DateTime>();

                for (DateTime dt = DateTime.Now.FirstDayOfMonth();
                    dt <= DateTime.Now.LastDayOfMonth();
                    dt = dt.AddDays(1)) //Перебор всех дней в месяце
                {
                    if (!Weekend.Contains(dt.DayOfWeek)) // day is not weekend
                    {
                        value.Add(dt);
                    }
                }

                return value.AsReadOnly();
            }
        }

        internal Graph(ref List<Person> people)
        {
            this.people = people;
            Weekend.CollectionChanged += Weekend_CollectionChanged;
        }

        private void Weekend_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AssignEveryone();
        }

        public void AssignEveryone(int startIndex = 0) //Главная механика
        {
            ClearAllAssignments();

            if (people.Count != 0)
            {
                for (int pIndex = startIndex, day = 1; day <= DateTime.Now.DaysInMonth(); day++, pIndex++)
                {
                    if (IsHoliday(day)) // if day is holiday - skip;
                    {
                        pIndex--; //but not skip person
                        continue;
                    }
                    if (pIndex >= people.Count) pIndex = 0;
                    if (people[pIndex].DutyDates == null)
                    {
                        people[pIndex].DutyDates = new();
                    }

                    var dutyDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);
                    people[pIndex].DutyDates.Add(dutyDate);
                }
            }
        }
        public void ClearAllAssignments()
        {
            foreach (var person in people) person.DutyDates = null;
        }
        internal void ClearAssignment(int index)
        {
            people[index].DutyDates = null;
        }
        
        public void MonthlyUpdate()
        {
            Person lastPerson = (from p in AssignedPeople
                                 where p.DutyDates.Last() == Workdates.Last()
                                 select p).Single();
            int lastIndex = people.IndexOf(lastPerson);

            ClearAllAssignments();

            AssignEveryone(lastIndex + 1);
        }
        public bool IsHoliday(DateTime date)
        {
            return Weekend.Contains(date.DayOfWeek);
        }

        public bool IsHoliday(int day)
        {
            return Weekend.Contains(new DateTime(DateTime.Now.Year,
                    DateTime.Now.Month, day).DayOfWeek);
        }
        public bool IsAssigned(DateTime date)
        {
            foreach (var person in people)
            {
                if (person.DutyDates != null)
                {
                    foreach (var dutydate in person.DutyDates)
                    {
                        if (dutydate == date) return true;
                    }
                }
            }
            return false;
        }
        public Person this[DateTime dutyDate] => people?.SingleOrDefault(person =>
                                                 person?.DutyDates?.Where(date =>
                                                 date == dutyDate).Any() == true);
    }
}
