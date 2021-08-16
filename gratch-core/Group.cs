using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

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
            groups.Add(this);
            graph = new Graph(ref people);
        }

        public Group(IEnumerable<string> names) : this()
        {
            foreach (var name in names)
            {
                Add(name);
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
                Add(new Person(name));
            }
            else
            {
                throw new ArgumentException("Person already exists");
            }
        }
        public void Add(Person person)
        {
            var samepeople = from p in People where p.Name == person.Name select p.Name;
            if (!samepeople.Any())
            {
                person.DutyDates = null;
                people.Add(person);
                Graph.AssignEveryone();
            }
            else
            {
                throw new ArgumentException("Person already exists");
            }
        }
        public void Remove(int index) // если плохо с производительностью - сюды.
        {
            people.RemoveAt(index);
            graph.AssignEveryone();
        }
        public Person GetPerson(DateTime dutydate) =>
            People?.SingleOrDefault(person => 
            person?.DutyDates?.Where(date => 
            date == dutydate).Any() == true);

    }
}
