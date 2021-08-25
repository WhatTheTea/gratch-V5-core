#define LOGGING
#undef LOGGING

using gratch_core.Models;

namespace gratch_core
{
    internal class SQLiteSubscriber
    {
        private static SQLiteSubscriber _subscriber;
        private readonly GroupRepository repos = new();
        private SQLiteSubscriber()
        {
            Group.GroupAdded += Group_GroupAdded;
            Group.GroupChanged += Group_GroupChanged;
            Group.GroupRemoved += Group_GroupRemoved;

            Group.PersonAdded += Group_PersonAdded;
            Group.PersonChanged += Group_PersonChanged;
            Group.PersonRemoved += Group_PersonRemoved;
        }
        public void Destroy()
        {
            _subscriber = null;

            Group.GroupAdded -= Group_GroupAdded;
            Group.GroupChanged -= Group_GroupChanged;
            Group.GroupRemoved -= Group_GroupRemoved;

            Group.PersonAdded -= Group_PersonAdded;
            Group.PersonChanged -= Group_PersonChanged;
            Group.PersonRemoved -= Group_PersonRemoved;
        }
        public static SQLiteSubscriber GetSubscriber()
        {
            if (_subscriber == null)
            {
                _subscriber = new SQLiteSubscriber();
            }
            return _subscriber;
        }
        private void Group_PersonRemoved(object sender, IPerson person)
        {
            repos.DeletePerson(sender as Group, person as Person);
#if LOGGING
            Console.WriteLine(DateTime.Now + $" | SQLiteListener | Person {(person as Person).Name} Removed from {(sender as Group).Name}");
#endif
        }

        private void Group_PersonChanged(object sender, object person)
        {
            repos.UpdatePeople(sender as Group);
#if LOGGING
            Console.WriteLine(DateTime.Now + $" | SQLiteListener | Person {(person as Person).Name} updated in {(sender as Group).Name}");
#endif
        }

        private void Group_PersonAdded(object sender, IPerson person)
        {
            repos.AddPerson(sender as Group, person as Person);
#if LOGGING
            Console.WriteLine(DateTime.Now + $" | SQLiteListener | Person {(person as Person).Name} added from {(sender as Group).Name}");
#endif
        }

        private void Group_GroupRemoved(object sender)
        {
            repos.DeleteGroup((sender as Group));
#if LOGGING
            Console.WriteLine(DateTime.Now + $" | SQLiteListener | Group {(sender as Group).Name} removed from table");
#endif
        }

        private void Group_GroupChanged(object sender)
        {
            repos.UpdateGroup(sender as Group);
#if LOGGING
            Console.WriteLine(DateTime.Now + $" | SQLiteListener | Group {(sender as Group).Name} changed");
#endif
        }

        private void Group_GroupAdded(object sender)
        {
            repos.AddGroup((sender as Group));
#if LOGGING
            Console.WriteLine(DateTime.Now + $" | SQLiteListener | Group {(sender as Group).Name}  table");
#endif
        }
    }
}
