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
        private static readonly List<Group> groups = new List<Group>();
        internal static IList<Group> Groups { get => groups.AsReadOnly(); }

        private readonly List<Person> people = new List<Person>();
        public IList<Person> People { get => people.AsReadOnly(); }

        private Graph graph;
        public Graph Graph { get => graph; }
        public Group()
        {
            //Person.PersonImported += Person_PersonImported;
            groups.Add(this);
            graph = new Graph(ref people);
        }

        public Group(IEnumerable<string> names) : this()
        {
            foreach (var name in names)
            {
                people.Add(new Person(name));
            }
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
            if (!samepeople.Any())
            {
                people.Add(new Person(name));
            }
            else
            {
                throw new ArgumentException("Person already exists");
            }
        }
        public void Add(Person person)
        {
            var freeDutyDate = graph.AssignedPeople.Count == 0
                ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
                : graph.AssignedPeople[^1].DutyDates[^1].AddDays(1);
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
            } else
            {
                throw new ArgumentException("Person already exists");
            }
        }
        public void Remove(int index) // если плохо с производительностью - сюды.
        {
            people.RemoveAt(index);
            graph.AssignEveryone();
        }
        public Person GetPerson(DateTime dutydate)
        {
            foreach (var person in graph.AssignedPeople)
            {
                foreach (var date in person.DutyDates)
                {
                    if (dutydate == date)
                    {
                        return person;
                    }
                }
            }
            return null;
        }
        
    }
}
