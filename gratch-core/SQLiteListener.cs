using gratch_core.Models;

using System;
using System.Linq;

namespace gratch_core
{
    internal class SQLiteListener
    {
        private static SQLiteListener listener;
        private readonly GroupRepository repos = new();
        private SQLiteListener()
        {
            Group.GroupAdded += Group_GroupAdded;
            Group.GroupChanged += Group_GroupChanged;
            Group.GroupRemoved += Group_GroupRemoved;

            Group.PersonAdded += Group_PersonAdded;
            Group.PersonUpdated += Group_PersonUpdated;
            Group.PersonRemoved += Group_PersonRemoved;
        }
        public void Destroy()
        {
            listener = null;

            Group.GroupAdded -= Group_GroupAdded;
            Group.GroupChanged -= Group_GroupChanged;
            Group.GroupRemoved -= Group_GroupRemoved;

            Group.PersonAdded -= Group_PersonAdded;
            Group.PersonUpdated -= Group_PersonUpdated;
            Group.PersonRemoved -= Group_PersonRemoved;
        }
        public static SQLiteListener GetListener()
        {
            if (listener == null)
            {
                listener = new SQLiteListener();
            }
            return listener;
        }
        private static PersonModel FindAndConvertPerson(object group,
                                                        object person) => (group as Group).ToModel().People.First(p => p.Name == (person as Person).Name);
        private void Group_PersonRemoved(object sender, object person)
        {
            repos.UpdateGroup(sender as Group, GroupRepository.UpdateType.PersonRemoved);
#if DEBUG
            Console.WriteLine(DateTime.Now + $" | SQLiteListener | Person {(person as Person).Name} Removed from {(sender as Group).Name}");
#endif
        }

        private void Group_PersonUpdated(object sender, object person)
        {
            repos.UpdateGroup(sender as Group, GroupRepository.UpdateType.PersonChanged);
#if DEBUG
            Console.WriteLine(DateTime.Now + $" | SQLiteListener | Person {(person as Person).Name} updated in {(sender as Group).Name}");
#endif
        }

        private void Group_PersonAdded(object sender, object person)
        {
            repos.UpdateGroup(sender as Group, GroupRepository.UpdateType.PersonAdded);
#if DEBUG
            Console.WriteLine(DateTime.Now + $" | SQLiteListener | Person {(person as Person).Name} added from {(sender as Group).Name}");
#endif
        }

        private void Group_GroupRemoved(object sender)
        {
            repos.DeleteGroup((sender as Group));
#if DEBUG
            Console.WriteLine(DateTime.Now + $" | SQLiteListener | Group {(sender as Group).Name} removed from table");
#endif
        }

        private void Group_GroupChanged(object sender)
        {
            repos.UpdateGroup((sender as Group),GroupRepository.UpdateType.GroupChanged);
#if DEBUG
            Console.WriteLine(DateTime.Now + $" | SQLiteListener | Group {(sender as Group).Name} changed");
#endif
        }

        private void Group_GroupAdded(object sender)
        {
            repos.InsertGroup((sender as Group));
#if DEBUG
            Console.WriteLine(DateTime.Now + $" | SQLiteListener | Group {(sender as Group).Name}  table");
#endif
        }
    }
}
