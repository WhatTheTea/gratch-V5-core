using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

//[assembly: InternalsVisibleTo("gratch_core_tests")]

namespace gratch_core
{
    public class Person
    {
        public string Name { get; internal set; }
        public List<DateTime> DutyDates { get; internal set; }

        public Person(string name)
        {
            Name = name;
        }

        public void Rename(string name)
        {
            foreach(var group in Group.Groups)
            {
                bool personExists = group.People.Where(person => person.Name == Name).Any();// Select group, where this person is
                bool renameExists = group.People.Where(reperson => reperson.Name == name).Any();
                if (personExists && !renameExists) 
                {
                    Name = name;
                }
            }
        }
    }
}
