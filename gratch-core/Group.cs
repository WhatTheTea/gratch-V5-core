using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("gratch_core_tests")]

namespace gratch_core
{
    public class Group : IGroup
    {
        #region events
        internal static SQLiteListener listener = SQLiteListener.GetListener();

        public delegate void GroupEventHandler(object sender);
        public static event GroupEventHandler GroupChanged;
        public static event GroupEventHandler GroupAdded;
        public static event GroupEventHandler GroupRemoved;

        public delegate void PersonChangedEventHandler(object sender, object person);
        public static event PersonChangedEventHandler PersonUpdated;
        public static event PersonChangedEventHandler PersonRemoved;
        public static event PersonChangedEventHandler PersonAdded;

        private void Weekend_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            GroupChanged?.Invoke(this);
        }
        private void Person_PersonUpdated(object sender)
        {
            if (_people.Any(pers => pers.Name == (sender as Person).Name))
            {
                PersonUpdated?.Invoke(this, sender);
            }
        }
        #endregion
        #region Instances
        private static readonly List<Group> instances = new List<Group>();
        internal static IList<Group> AllInstances =>
            instances.Where(instance => instance.Count > 0).ToList().AsReadOnly();
        #endregion
        private string _name;
        private readonly List<Person> _people = new();
        private Graph graph;
        public string Name
        {
            get => _name;
            set
            {
                if (!AllInstances.Any(grp => grp.Name == value))
                {
                    _name = value;
                    GroupChanged?.Invoke(this);
                }
            }
        }
        public Graph Graph { get => graph; }
        public Group()
        {
            instances.Add(this);
            graph = new Graph(ref _people);

            Person.PersonChanged += Person_PersonUpdated;
            graph.Weekend.CollectionChanged += Weekend_CollectionChanged;
        }
        public Group(string GroupName) : this() => _name = GroupName;
        public Group(string GroupName, IEnumerable<string> names) : this()
        {
            _name = GroupName;
            foreach (var name in names) Add(name);
        }
        public void Replace(int pIndex, int withIndex)
        {   //Записываем имена в буфер
            string pBuffer = _people[pIndex].Name;
            string withBuffer = _people[withIndex].Name;
            //Обходим ограничение на одно и то самое имя в группе
            _people[pIndex].Rename(string.Empty, true);
            _people[withIndex].Rename(null, true);
            //Присваиваем из буфера
            _people[withIndex].Name = pBuffer;
            _people[pIndex].Name = withBuffer;
        }
        public void Add(string name)
        {
            if (this.Contains(name))
            {
                throw new ArgumentException("Person already exists");
            }
            else
            {
                Add(new Person(name));
                Graph.AssignEveryone();
            }
        }
        #region IList
        public Person this[int index]
        {
            get
            {
                return _people[index];
            }
            set
            {
                var name = (value.Clone() as Person).Name;
                _people[index].Name = name;
            }
        }
        public int IndexOf(Person person) => _people.IndexOf(person);
        public void Insert(int index, Person person)
        {
            var _ = person.Clone() as Person;
            _people.Insert(index, _);
            PersonAdded?.Invoke(this, person); //!!!! at index
            Graph.AssignEveryone();
        }
        public void RemoveAt(int index) // если плохо с производительностью - сюды.
        {
            PersonRemoved?.Invoke(this, _people[index]);
            _people.RemoveAt(index);
            if (Count <= 0)
            {
                GroupRemoved?.Invoke(this);
            }

            Graph.AssignEveryone();
        }
        #endregion
        #region ICollection
        public int Count => _people.Count;
        public bool IsReadOnly => false;
        public void CopyTo(Person[] people, int index) => _people.CopyTo(people, index);
        public bool Contains(Person person) => _people.Contains(person);
        public bool Contains(string name) => (from p in _people where p.Name == name select p.Name).Any();
        public void Add(Person person) // not safe for dutydates
        {
            var newperson = person.Clone() as Person;
            _people.Add(newperson);

            if (Count == 1)
            {
                instances.Add(this);
                GroupAdded?.Invoke(this);
            }
            PersonAdded?.Invoke(this, newperson);
        }
        public void Clear()
        {
            GroupRemoved?.Invoke(this);
            _people.Clear();
            //groups.Remove(this);
        }
        public bool Remove(Person person)
        {
            if (Count > 0)
            {
                PersonRemoved?.Invoke(this, person);
            }
            else
            {
                GroupRemoved?.Invoke(this);
            }

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
