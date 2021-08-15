using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("gratch_core_tests")]

namespace gratch_core
{
    public class Person
    {
        public delegate void PersonHandler(Person person);
        public static event PersonHandler PersonImported;

        public string Name { get; internal set; }
        public List<DateTime> DutyDates { get; internal set; }

        public Person(string name)
        {
            Name = name;
        }
        internal Person(string name, params string[] datetimes_str)
        {
            foreach (var str in datetimes_str)
            {
                DutyDates.Add(DateTime.Parse(str));
            }
            PersonImported?.Invoke(this);
        }
        internal Person(string name, params DateTime[] datetimes)
        {
            DutyDates = datetimes.ToList();
            PersonImported?.Invoke(this);
        }

    }
}
