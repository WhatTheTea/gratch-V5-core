using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace gratch_core
{
    public class Person : ICloneable
    {
        #region events
        internal static event PersonHandler PersonChanged;
        internal delegate void PersonHandler(object sender, PersonChangedEventArgs args);
        #endregion

        private string _name;
        public string Name { get => _name; set => Rename(value); }

        private string _groupName;
        public string GroupName
        {
            get => _groupName;
            internal set
            {
                _groupName = value;
            }
        }
        internal ObservableCollection<DateTime> DutyDates { get; set; }

        public Person(string name)
        {
            DutyDates.CollectionChanged += DutyDates_CollectionChanged;
            _name = name;

            PersonChanged.Invoke(this,
                new PersonChangedEventArgs(PersonChangedEventType.PersonAdded, GroupName));
        }

        private void DutyDates_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            PersonChanged.Invoke(this,
                new PersonChangedEventArgs(PersonChangedEventType.PersonChanged, GroupName));
        }

        public void Rename(string name)
        {
            foreach (var group in Group.AllInstances)
            {
                bool personExists = group.Where(person => person.Name == Name).Any();// Select group, where this person is
                bool renameExists = group.Where(reperson => reperson.Name == name).Any();
                if (personExists && !renameExists)
                {
#if DEBUG
                    Console.WriteLine(DateTime.Now + $" | Person | Renaming {Name} to {name}");
#endif
                    _name = name;

                    PersonChanged.Invoke(this,
                new PersonChangedEventArgs(PersonChangedEventType.PersonChanged, GroupName));
                }
                else
                {
#if DEBUG
                    Console.WriteLine(DateTime.Now + $" | Person | Rename failed. Name: {Name}, Rename: {name}");
#endif
                }
            }
        }
        //ICloneable
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
