using gratch_core.Models;

using System.Linq;

namespace gratch_core
{
    public class SQLiteListener
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
            repos.DeletePerson(FindAndConvertPerson(sender, person));
        }

        private void Group_PersonUpdated(object sender, object person)
        {
            repos.UpdatePerson(FindAndConvertPerson(sender, person));
        }

        private void Group_PersonAdded(object sender, object person)
        {
            repos.InsertPerson(FindAndConvertPerson(sender, person));
        }

        private void Group_GroupRemoved(object sender)
        {
            repos.DeleteGroup((sender as Group).ToModel());
        }

        private void Group_GroupChanged(object sender)
        {
            repos.UpdateGroup((sender as Group).ToModel());
        }

        private void Group_GroupAdded(object sender)
        {
            repos.InsertGroup((sender as Group).ToModel());
        }
    }
}
