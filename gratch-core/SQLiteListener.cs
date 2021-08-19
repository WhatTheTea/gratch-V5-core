using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gratch_core
{
    public class SQLiteListener 
    {
        private Models.PersonRepository repos = new();
        SQLiteListener()
        {
            Group.PersonAdded += Group_PersonAdded;
            Group.PersonChanged += Group_PersonChanged;
            Group.PersonRemoved += Group_PersonRemoved;
        }

        private void Group_PersonRemoved(Group sender, Person person)
        {
            throw new NotImplementedException();
        }

        private async void Group_PersonChanged(Group sender, Person person)
        {
            

            //await repos.SavePerson(//model);
        }

        private void Group_PersonAdded(Group sender, Person person)
        {
            throw new NotImplementedException();
        }
    }
}
