using System;
using System.Collections.Generic;
using System.Linq;

namespace gratch_core
{
    public class SQLiteListener 
    {
        private static SQLiteListener listener;
        private readonly Models.GroupRepository repos = new();
        private SQLiteListener()
        {
            Group.GroupChanged += Group_GroupChanged;
            Person.PersonChanged += Person_PersonChanged;
            Group.PersonDeleted += Person_PersonChanged;
        }
        public static SQLiteListener GetListener()
        {
            if(listener == null)
            {
                listener = new SQLiteListener();
            }
            return listener;
        }
        private void Person_PersonChanged(object sender, PersonChangedEventArgs args)
        {
            switch (args.EventType)
            {
                case PersonChangedEventType.PersonAdded:
                    PersonAdded(sender as Person);
                    break;
                case PersonChangedEventType.PersonChanged:
                    PersonChanged(sender as Person);
                    break;
                case PersonChangedEventType.PersonRemoved:
                    PersonRemoved(Group.AllInstances.Single(grp => grp.Name == args.GroupName),
                        sender as Person);
                    break;
                case PersonChangedEventType.AllPeopleRemoved:
                    ClearPeople(Group.AllInstances.Single(grp => grp.Name == args.GroupName));
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        private void Group_GroupChanged(object sender, GroupChangedEventArgs args)
        {
            switch (args.EventType)
            {
                case GroupChangedEventType.GroupNameChanged:
                    GroupNameChanged(sender as Group);
                    break;
                case GroupChangedEventType.WeekendChanged:
                    throw new NotImplementedException();

                default:
                    throw new NotSupportedException();
            }
        }
        private async void WeekendChanged(Group grp)
        {

        }
        private async void GroupNameChanged(Group sender)
        {
            await repos.SavePeople(ModelConversionExtension.ToModels(sender.ToList()));
        }

        private async void PersonRemoved(Group sender, Person person)
        {
            await repos.DeletePersonByIndex(sender.IndexOf(person),sender.Name);
        }

        private async void PersonChanged(Person person)
        {
            await repos.SavePerson(ModelConversionExtension.ToModel(person));
        }

        private async void PersonAdded(Person person)
        {
            await repos.SavePerson(ModelConversionExtension.ToModel(person));
        }
        private async void ClearPeople(Group grp)
        {
            await repos.ClearAll(grp.Name);
        }
    }
}
