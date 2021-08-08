using Microsoft.VisualBasic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gratch_core
{
    public class Group
    {
        public List<Person> People { get; set; } = new List<Person>();
        public List<DayOfWeek> Weekend { get; set; } = new List<DayOfWeek>();

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

        public Group()
        {
        }
        public Group(IEnumerable<string> names)
        {
            foreach (var name in names)
            {
                People.Add(new Person(name));
            }
        }
        public void DutyDatesByDefault()
        {
            foreach (var person in People) person.DutyDates = null;
            AssignDutyDates();
        }
        public void AssignDutyDates() //Главная механика
        {
            if (People.Count != 0)
            {
                for (int pIndex = 0, day = 1; day <= DateTime.Now.DaysInMonth(); day++)
                {
                    if (IsHoliday(day)) continue;// if day is holiday - skip;
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

        private void UpdateDutyDates()
        {
            //TODO
        }

        public bool IsDutyDateAssigned(DateTime date)
        {
            foreach (var person in People)
            {
                foreach (var dutydate in person.DutyDates)
                {
                    if (dutydate == date) return true;
                }
            }
            return false;
        }
        public static bool IsPersonAssigned(Person person)
        {
            return person.DutyDates != null;
        }
    }
}
