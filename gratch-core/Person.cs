using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace gratch_core
{
    public class Person : IPerson
    {
        #region events
        internal static event PersonHandler PersonChanged;
        internal delegate void PersonHandler(object sender);
        internal static bool SupressInvocation { get; set; } = false;
        private void DutyDates_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!SupressInvocation) PersonChanged?.Invoke(this);
        }
        #endregion

        private string _name;
        public string Name { get => _name; set => Rename(value); }
        private ObservableCollection<DateTime> _dutyDates = new();
        public Collection<DateTime> DutyDates { get => _dutyDates; set => _dutyDates = new ObservableCollection<DateTime>(value); }  //АвтоСвойство
        public Person(string name)
        {
            _name = name;

            _dutyDates.CollectionChanged += DutyDates_CollectionChanged;
        }


        public void Rename(string name, bool invokeMuted = false)
        {
            foreach (var group in IGroup.AllInstances)
            {
                bool personExists = group.Any(
                    person => person.Name == Name && person.DutyDates == DutyDates);// Select group, where this person is
                bool renameExists = group.Any(reperson => reperson.Name == name);
                if (personExists && !renameExists)
                {
#if DEBUG
                    Console.WriteLine(DateTime.Now + $" | Person | Renaming {Name} to {name} | InvokeMuted: {invokeMuted}");
#endif              
                    _name = name;
                    if (!invokeMuted) PersonChanged?.Invoke(this);
                }
                else if (renameExists)
                {
#if DEBUG
                    Console.WriteLine(DateTime.Now + $" | Person | Rename failed. Name: {Name}," +
                        $" Rename: {name} | InvokeMuted: {invokeMuted}");
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
