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
        protected static readonly List<IGroup> instances = new List<IGroup>();
        internal static IList<IGroup> AllInstances
        {
            get
            {
                return instances.Where(instance => instance.Count > 0).Distinct().ToList().AsReadOnly();
            }
        }

        #region events
        internal static SQLiteSubscriber subscriber = SQLiteSubscriber.GetSubscriber();

        public delegate void GroupEventHandler(object sender);
        public static event GroupEventHandler GroupChanged;
        public static event GroupEventHandler GroupAdded;
        public static event GroupEventHandler GroupRemoved;

        public delegate void PersonChangedEventHandler(object sender, IPerson person);
        public static event PersonChangedEventHandler PersonChanged;
        public static event PersonChangedEventHandler PersonRemoved;
        public static event PersonChangedEventHandler PersonAdded;

        private void Weekend_CollectionChanged(object sender, EventArgs e)
        {
            GroupChanged?.Invoke(this);
        }
        private void Person_PersonUpdated(object sender)
        {
            if (_people.Any(pers => pers.Name == (sender as IPerson).Name))
            {
                PersonChanged?.Invoke(this, sender as IPerson);
            }
        }
        #endregion
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (!Group.AllInstances.Any(grp => grp.Name == value))
                {
                    _name = value;
                    GroupChanged?.Invoke(this);
                }
            }
        }
        private readonly List<Person> _people = new List<Person>();
        private readonly Graph _graph;
        public Graph Graph { get => _graph; }
        private Group()
        {
            Group.instances.Add(this);
            _graph = new Graph(_people);

            Person.PersonChanged += Person_PersonUpdated;
            _graph.WeekendChanged += Weekend_CollectionChanged;
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
                return _people.Count != 0 ?
                _people[index] : null;
            }
            set
            {
                var name = value.Name;
                _people[index].Name = name;
            }
        }
        public int IndexOf(Person person) => _people.IndexOf(person);
        public void Insert(int index, Person person)
        {
            var _ = person;
            _people.Insert(index, _);
            PersonAdded?.Invoke(this, person); //!!!! at index
            Graph.AssignEveryone();
        }
        public void RemoveAt(int index) // если плохо с производительностью - сюды.
        {
            _people.RemoveAt(index);
            PersonRemoved?.Invoke(this, _people[index]);
            if (Count < 1)
            {
                GroupRemoved?.Invoke(this);
            }
            else
            {
                Graph.AssignEveryone();
            }
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
            //var newperson = person.Clone() as Person;
            _people.Add(person);

            if (Count == 1)
            {
                Group.instances.Add(this);
                GroupAdded?.Invoke(this);
            }
            PersonAdded?.Invoke(this, person);
        }
        public void Clear()
        {
            GroupRemoved?.Invoke(this);
            _people.Clear();
            //groups.Remove(this);
        }
        public bool Remove(Person person)
        {
            var _ = _people.Remove(person);
            PersonRemoved?.Invoke(this, person);
            if (Count < 1)
            {
                GroupRemoved?.Invoke(this);
            }
            else
            {
                Graph.AssignEveryone();
            }
            return _;
        }
        #endregion
        #region IEnumerator
        public IEnumerator<Person> GetEnumerator() => _people.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _people.GetEnumerator();
        #endregion
    }

}
