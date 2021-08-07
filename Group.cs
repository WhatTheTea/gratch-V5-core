using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gratch_core
{
    public class Group
    {
        private List<Person> people = new List<Person>();
        public List<Person> People { get => people; set => people = value; }

        private List<DayOfWeek> weekend = new List<DayOfWeek>();
        public List<DayOfWeek> Weekend { get => weekend; set => weekend = value; }

        public List<DateTime> Workdates
        {
            get
            {
                List<DateTime> value = new();

                DateTime today = DateTime.Today;
                var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
                DateTime firstDOM = DateTime.Parse($@"01.{today.Month}.{today.Year}"); //Первый день //Последний
                DateTime lastDOM = DateTime.Parse($@"{daysInMonth}.{today.Month}.{today.Year}");

                for (DateTime dt = firstDOM; dt <= lastDOM; dt = dt.AddDays(1))
                {
                    if (!Weekend.Contains(dt.DayOfWeek)) // day is not weekend
                    {
                        value.Add(dt);
                    }
                }

                return value;
            }
        }

        public Group()
        {
            People = people;
            Weekend = weekend;
        }
        public Group(List<string> names) : this()
        {
            foreach (var name in names)
            {
                people.Add(new Person(name));
            }
        }

        private void AssignDutyDates()
        {

        }
        private void UpdateDutyDates()
        {

        }
    }
}
