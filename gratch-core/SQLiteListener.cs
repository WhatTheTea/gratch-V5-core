using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gratch_core
{
    public class SQLiteListener 
    {
        private readonly Models.PersonRepository repos = new();
        SQLiteListener()
        {
            Group.PersonAdded += Group_PersonAdded;
            Group.PersonChanged += Group_PersonChanged;
            Group.PersonRemoved += Group_PersonRemoved;
            Group.GroupNameChanged += Group_GroupNameChanged;
        }

        private async void Group_GroupNameChanged(object sender, EventArgs e)
        {
            await repos.SavePeople(PersonAdapter.GetModels((sender as Group).ToList()));
        }

        private async void Group_PersonRemoved(object sender, Person person)
        {
            await repos.DeletePersonByIndex((sender as Group).IndexOf(person));
        }

        private async void Group_PersonChanged(object sender, Person person)
        {
            await repos.SavePerson(PersonAdapter.GetModel(person));
        }

        private async void Group_PersonAdded(object sender, Person person)
        {
            await repos.SavePerson(PersonAdapter.GetModel(person));
        }
    }
}
