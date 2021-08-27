
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
        internal event EventHandler WeekendChanged;
        private void Weekend_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AssignEveryone();
            WeekendChanged.Invoke(this, new EventArgs());
        }
        #endregion
        private List<Person> _people;
        public IList<Person> AssignedPeople => _people.Where(p => p.DutyDates.Any()).ToList().AsReadOnly();
        private ObservableCollection<DayOfWeek> _weekend = new();
        public IList<DayOfWeek> Weekend
        {
            get => _weekend.ToList().AsReadOnly();
            set
            {
                Group.subscriber?.Destroy();
                _weekend.Clear();
                for (int i = 0; i < value.Count; i++)
                {
                    if (value[i] == value.Last()) Group.subscriber = SQLiteSubscriber.GetSubscriber();
                    _weekend.Add(value[i]);
                }
            }
        }
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

        internal Graph(List<Person> people)
        {
            this._people = people;
            _weekend.CollectionChanged += Weekend_CollectionChanged;
        }

        public void AssignEveryone(int startIndex = 0) //Главная механика
        {
            DateTime now = DateTime.Now;
            int pCount = _people.Count;
            int dinm = now.DaysInMonth();

            if (pCount > 0)
            {
                if (_people[startIndex].DutyDates.Any()) ClearAllAssignments();
                for (int pIndex = startIndex, day = 1; day <= dinm; day++, pIndex++)
                {
                    if (!IsHoliday(day)) // if day is holiday - skip;
                    {
                        if (pCount > 1 && day != dinm)
                        {
                            Person.SupressInvocation = true;
                        }
                        else Person.SupressInvocation = false;

                        if (pIndex >= pCount) pIndex = 0;
                        var dutyDate = new DateTime(now.Year, now.Month, day);

                        _people[pIndex].DutyDates.Add(dutyDate);
                    }
                    else
                    {
                        pIndex--; //but do not skip person
                        continue;
                    }
                }
            }
        }
        public void ClearAllAssignments()
        {
            _people.ForEach(p => p.DutyDates.Clear());
        }
        internal void ClearAssignment(int index) => _people[index].DutyDates.Clear();
        public void MonthlyUpdate()
        {
            Person lastPerson = AssignedPeople.First(p => p.DutyDates.Last() == Workdates.Last());
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
