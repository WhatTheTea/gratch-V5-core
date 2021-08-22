using SQLite;

using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensions.Extensions.TextBlob;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gratch_core.Models
{
    internal class GroupRepository
    {
        private readonly SQLiteConnection db;
        public GroupRepository()
        {
            db = SQLiteDB.GetAsyncConnection();
            db.CreateTable<GroupModel>();
            db.CreateTable<PersonModel>();
            TextBlobOperations.SetTextSerializer(Newtonsoft.Json.JsonSerializer.Create() as ITextBlobSerializer);
        }
        #region Getters
        public List<GroupModel> GetGroups()
        {
            return db.GetAllWithChildren<GroupModel>();
        }
        public GroupModel GetGroup(string name)
        {
            return GetGroups().SingleOrDefault(grp => grp.Name == name);
        }
        public GroupModel GetGroup(int id)
        {
            return db.GetWithChildren<GroupModel>(id);
        }
        public List<DayOfWeek> GetGroupWeekend(string groupname)
        {
            return GetGroup(groupname).Weekend;
        }
        public PersonModel GetPerson(string name, GroupModel group)
        {
            return db.Table<PersonModel>().SingleOrDefault(pers => pers.GroupModel == group);
        }
        #endregion

        public void InsertGroup(GroupModel group) => db.InsertOrReplaceWithChildren(group);
        public void UpdateGroup(GroupModel group)
        {
            var realGroup = GetGroup(group.Name);

            int groupCount = group.People.Count;
            int realCount = realGroup.People.Count;
            if (groupCount > realCount) // Is person added
            {
                db.InsertWithChildren(group.People.Last());
            } else if (groupCount == realCount)
            {
                db.UpdateWithChildren(group);
            } else
            {
                db.Delete(realGroup.People.Single(pers => !group.People.Contains(pers)));
            }
            
        }
        public void DeleteGroup(GroupModel group) => db.Delete(group);
        //public void InsertPerson(PersonModel person) => db.InsertOrReplace(person);
        public void DeleteAll()
        {
            db.DeleteAll<GroupModel>();
            db.DeleteAll<PersonModel>();
        }
    }
}
