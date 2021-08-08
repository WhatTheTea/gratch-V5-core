using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gratch_core
{
    public class Person
    {
        public string Name { get; internal set; }
        public List<DateTime> DutyDates { get; internal set; } // TODO: make it internal

        public Person(string name)
        {
            Name = name;
        }
    }
}
