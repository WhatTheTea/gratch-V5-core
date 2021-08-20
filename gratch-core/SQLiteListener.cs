
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
        }
        public static SQLiteListener GetListener()
        {
            if (listener == null)
            {
                listener = new SQLiteListener();
            }
            return listener;
        }

        private async void Group_GroupRemoved(object sender)
        {
            await repos.DeleteGroup((sender as Group).ToModel());
        }

        private async void Group_GroupChanged(object sender)
        {
            await repos.UpdateGroup((sender as Group).ToModel());
        }

        private async void Group_GroupAdded(object sender)
        {
            await repos.InsertGroup((sender as Group).ToModel());
        }
    }
}
