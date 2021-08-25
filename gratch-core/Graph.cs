
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("gratch_core_tests")]

namespace gratch_core
{
    public class Graph : IGraph
    {
        #region events
        internal event Person.PersonHandler PersonChanged;
        private void Weekend_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AssignEveryone();
        }
        #endregion
        private List<Person> _people;
        public IList<Person> AssignedPeople =>
            _people.Where(p =>
            p.DutyDates.Any()).ToList().AsReadOnly();
        private ObservableCollection<DayOfWeek> _weekend = new();
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
            this._people = people;
            Weekend.CollectionChanged += Weekend_CollectionChanged;
        }

        public void AssignEveryone(int startIndex = 0) //Главная механика
        {
            if (_people[startIndex].DutyDates.Any()) ClearAllAssignments();

            if (_people.Count != 0)
            {
                for (int pIndex = startIndex, day = 1; day <= DateTime.Now.DaysInMonth(); day++, pIndex++)
                {
                    if (IsHoliday(day)) // if day is holiday - skip;
                    {
                        pIndex--; //but do not skip person
                        continue;
                    }
                    if (pIndex >= _people.Count) pIndex = 0;

                    var dutyDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);
                    _people[pIndex].DutyDates.Add(dutyDate);
                    PersonChanged.Invoke(_people[pIndex]);
                }
            }
        }
        public void ClearAllAssignments()
        {
            foreach (var person in _people)
            {
                person.DutyDates.Clear();
            }
        }
        internal void ClearAssignment(int index) => _people[index].DutyDates.Clear();
        public void MonthlyUpdate()
        {
            Person lastPerson = (from p in AssignedPeople
                                 where p.DutyDates.Last() == Workdates.Last()
                                 select p).First();
            int lastIndex = _people.IndexOf(lastPerson);

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
            foreach (var person in _people)
            {
                if (person.DutyDates.Any() == true)
                {
                    foreach (var dutydate in person.DutyDates)
                    {
                        if (dutydate == date) return true;
                    }
                }
            }
            return false;
        }
        public Person this[DateTime dutyDate] => _people.FirstOrDefault(person =>
                                                 person.DutyDates.Any(date =>
                                                 date == dutyDate) == true);
    }
}
