
namespace gratch_core
{
    public class SQLiteListener
    {
        private static SQLiteListener listener;
        private readonly Models.GroupRepository repos = new();
        private SQLiteListener()
        {
            Group.GroupAdded += Group_GroupAdded;
            Group.GroupChanged += Group_GroupChanged;
            Group.GroupRemoved += Group_GroupRemoved;
            //Group.PersonChanged += Group_PersonChanged;
        }

        /*private void Group_PersonChanged(object sender, object person)
        {
            repos.InsertPerson((person as Person).ToModel());
        }*/

        public static SQLiteListener GetListener()
        {
            if (listener == null)
            {
                listener = new SQLiteListener();
            }
            return listener;
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
