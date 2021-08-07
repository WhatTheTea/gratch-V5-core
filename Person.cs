using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gratch_core
{
    public class Person
    {
        public string Name { get; set; }
        public DateTime DutyDate { get; set; }

        public Person(string name)
        {
            Name = name;
        }

        public bool IsOnWeekend(ref Group context)
        {
            return context.Weekend.Contains(DutyDate.DayOfWeek);
        }
    }
}
