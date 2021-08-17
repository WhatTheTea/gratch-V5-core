using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("gratch_core_tests")]

namespace gratch_core
{
    public class Group : IList<Person>
    {
        #region Instances
        private static readonly List<Group> instances = new List<Group>();
        internal static IList<Group> AllInstances { 
            get 
            {
                var realInstances = instances.Where(instance => instance != null);
                if (instances.Count != realInstances.Count())
                {
                    instances.Clear();
                    instances.AddRange(realInstances);
                }
                return instances.AsReadOnly(); 
            } 
        }
        #endregion

        private readonly List<Person> _people = new();
        public Person this[int index]
        {
            get
            {
                return _people[index];
            }
            set
            {
                Add(value); //!!!!!!!!!
            }
        }

        private Graph graph;
        public Graph Graph { get => graph; }
        public Group()
        {
            instances.Add(this);
            graph = new Graph(ref _people);
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
            string buffer = _people[withIndex].Name;
            _people[withIndex].Name = _people[itIndex].Name;
            _people[itIndex].Name = buffer;
        }
        public void Add(string name)
        {
            var samepeople = from p in _people where p.Name == name select p.Name;
            if (!samepeople.Any())
            {
                Add(new Person(name));
            }
            else
            {
                throw new ArgumentException("Person already exists");
            }
        }
        public Person FindByDutyDate(DateTime dutydate) =>
            _people?.SingleOrDefault(person =>
            person?.DutyDates?.Where(date =>
            date == dutydate).Any() == true);
        #region IList
        public int IndexOf(Person person) => _people.IndexOf(person);
        public void Insert(int index, Person person)
        {
            _people.Insert(index, person);
            Graph.AssignEveryone();
        }
        public void RemoveAt(int index) // если плохо с производительностью - сюды.
        {
            _people.RemoveAt(index);
            Graph.AssignEveryone();
        }
        #endregion
        #region ICollection
        public int Count => _people.Count;
        public bool IsReadOnly => false;
        public void CopyTo(Person[] people, int index) => _people.CopyTo(people, index);
        public bool Contains(Person person) => _people.Contains(person);
        public void Add(Person person)
        {
            var samepeople = from p in _people where p.Name == person.Name select p.Name;
            if (!samepeople.Any())
            {
                if (this == null) //InstanceReused
                {
                    instances.Add(this);
                }
                person.DutyDates = null;
                _people.Add(person);
                Graph.AssignEveryone();
            }
            else
            {
                throw new ArgumentException("Person already exists");
            }
        }
        public void Clear()
        {
            _people.Clear();
            //groups.Remove(this);
        }
        public bool Remove(Person person)
        {
            var _ = _people.Remove(person);
            Graph.AssignEveryone();
            return _;
        }
        #endregion
        #region IEnumerator
        public IEnumerator<Person> GetEnumerator() => _people.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _people.GetEnumerator();
        #endregion
    }

}
