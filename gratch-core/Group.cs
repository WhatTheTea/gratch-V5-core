using Microsoft.VisualBasic;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gratch_core
{
    public class Group
    {
        public List<Person> People { get; set; } = new List<Person>();
        public ObservableCollection<DayOfWeek> Weekend { get; set; } = new ObservableCollection<DayOfWeek>();
        private void Weekend_CollectionChanged(object sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AssignDutyDates();
        }

        public List<DateTime> Workdates
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

                return value;
            }
        }
        public List<Person> AssignedPeople => (from p in People
                                                     where p.DutyDates != null
                                                     select p).ToList();

        public Group()
        {
            Person.PersonImported += Person_PersonImported;
            Weekend.CollectionChanged += Weekend_CollectionChanged;
        }

        public Group(IEnumerable<string> names) : this()
        {
            foreach (var name in names)
            {
                People.Add(new Person(name));
            }
        }
        private void Person_PersonImported(Person person)
        {
            ValidateDutyDate(person);
        }
        private void ValidateDutyDate(Person person)
        {
            if (People.Contains(person))
            {

                if (People.Count > 1)
                {
                    var pIndex = People.IndexOf(person);
                    var isValid = person.DutyDates.First() > People[pIndex - 1].DutyDates.First();
                    if (!isValid)
                    {
                        throw new FormatException("DutyDate is not valid");
                    }
                }
                else if (People.First().DutyDates.First() != DateTime.Now.FirstDayOfMonth()
                  && !IsHoliday(DateTime.Now.FirstDayOfMonth()) // if first dutydate is not first day of month
                  )                                                  // and first day of month is not holiday
                {
                    throw new FormatException("First person is not on first day");
                }
            }
        }
        public void ClearDutyDates()
        {
            foreach (var person in People) person.DutyDates = null;
        }
        public void AssignDutyDates(int startIndex = 0) //Главная механика
        {
            ClearDutyDates();
            if (People.Count != 0)
            {
                for (int pIndex = startIndex, day = 1; day <= DateTime.Now.DaysInMonth(); day++, pIndex++)
                {
                    if (IsHoliday(day)) // if day is holiday - skip;
                    {
                        pIndex--;
                        continue;
                    }
                    if (pIndex >= People.Count) pIndex = 0;
                    if (People[pIndex].DutyDates == null)
                    {
                        People[pIndex].DutyDates = new();
                    }

                    var dutyDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);
                    People[pIndex].DutyDates.Add(dutyDate);
                }
            }
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

        public void UpdateDutyDates()
        {
            Person lastPerson = (from p in AssignedPeople
                                 where p.DutyDates.Last() == Workdates.Last()
                                 select p).Single();
            int lastIndex = People.IndexOf(lastPerson);

            ClearDutyDates();

            AssignDutyDates(lastIndex + 1);
        }

        public bool IsDutyDateAssigned(DateTime date)
        {
            foreach (var person in People)
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
        public Person FindPersonByDutyDate(DateTime dutydate)
        {
            foreach (var person in AssignedPeople)
            {
                foreach (var date in person.DutyDates)
                {
                    if (dutydate == date) return person;
                }
            }
            return null;
        }
    }
}
