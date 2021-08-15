using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("gratch_core_tests")]

namespace gratch_core
{
    public class Group
    {
        private List<Person> people { get; set; } = new List<Person>();
        public IList<Person> People { get => people.AsReadOnly(); }
        public ObservableCollection<DayOfWeek> Weekend { get; set; } = new ObservableCollection<DayOfWeek>();
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
        public IList<Person> AssignedPeople => (from p in People
                                                     where p.DutyDates != null
                                                     select p).ToList().AsReadOnly();
        private void Weekend_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AssignEveryone();
        }
        public Group()
        {
            //Person.PersonImported += Person_PersonImported;
            Weekend.CollectionChanged += Weekend_CollectionChanged;
        }

        public Group(IEnumerable<string> names) : this()
        {
            foreach (var name in names)
            {
                people.Add(new Person(name));
            }
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
        public void MonthlyUpdate()
        {
            Person lastPerson = (from p in AssignedPeople
                                 where p.DutyDates.Last() == Workdates.Last()
                                 select p).Single();
            int lastIndex = people.IndexOf(lastPerson);

            ClearAllAssignments();

            AssignEveryone(lastIndex + 1);
        }
        public void ClearAllAssignments()
        {
            foreach (var person in people) person.DutyDates = null;
        }

        internal void Assign(int index)
        {
            foreach(var dutydate in people[index - 1].DutyDates)
            {
                people[index].DutyDates.Add(dutydate.AddDays(1));
            }
        }
        internal void ClearAssignment(int index)
        {
            people[index].DutyDates.Clear();
        }
        public void Replace(int itIndex, int withIndex)
        {
            string buffer = people[withIndex].Name;
            people[withIndex].Name = people[itIndex].Name;
            people[itIndex].Name = buffer;
        }
        public void Add(string name)
        {
            var samepeople = from p in People where p.Name == name select p.Name;
            if (!samepeople.Any()) people.Add(new Person(name));
            else throw new ArgumentException("Person already exists");
        }
        public void Add(Person person)
        {
            var freeDutyDate = AssignedPeople.Count == 0
                ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
                : AssignedPeople[^1].DutyDates[^1].AddDays(1);
            var samepeople = from p in People where p.Name == person.Name select p.Name;
            if (!samepeople.Any())
            {
                if (freeDutyDate.Month == DateTime.Now.Month)
                {
                    person.DutyDates = new();
                    person.DutyDates.Add(freeDutyDate);
                    people.Add(person);
                }
                else
                {
                    people.Add(person);
                }
            } else throw new ArgumentException("Person already exists");
        }
        public void Remove(int index)
        {

        }
        public Person FindPerson(DateTime dutydate)
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
    }
}
