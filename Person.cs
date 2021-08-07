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

        public bool IsOnWeekend(ref Group group)
        {
            return group.weekend.Contains(DutyDate.DayOfWeek);
        }
    }
}
