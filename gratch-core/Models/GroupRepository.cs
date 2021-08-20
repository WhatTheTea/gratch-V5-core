using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

using SQLiteNetExtensionsAsync;
using SQLiteNetExtensionsAsync.Extensions;

namespace gratch_core.Models
{
    internal class GroupRepository
    {
        private readonly SQLiteAsyncConnection db;
        public GroupRepository()
        {
            db = SQLiteDB.GetAsyncConnection();
            db.CreateTableAsync<GroupModel>();
            db.CreateTableAsync<PersonModel>();
        }
        #region Getters
        public Task<List<GroupModel>> GetGroups()
        {
            return db.GetAllWithChildrenAsync<GroupModel>();
        }
        public Task<GroupModel> GetGroup(string name)
        {
            return Task.Run(() => GetGroups().Result.Single(grp => grp.Name == name));
        }
        public Task<GroupModel> GetGroup(int id)
        {
            return Task.Run(() => db.GetWithChildrenAsync<GroupModel>(id));
        }
        public Task<List<DayOfWeek>> GetGroupWeekend(string groupname)
        {
            return Task.Run(() => GetGroup(groupname).Result.Weekend);
        }
        public Task<PersonModel> GetPerson(string name, string groupname)
        {
            return Task.Run(() => GetGroup(groupname).Result.People.Single(pers => pers.Name == name));
        }
        #endregion

        public Task InsertGroup(GroupModel group)
        {
            return Task.Run(() => db.InsertOrReplaceWithChildrenAsync(group));
        }
        public Task UpdateGroup(GroupModel group)
        {
            return Task.Run(() => db.UpdateWithChildrenAsync(group));
        }
        public Task<int> DeleteGroup(GroupModel group)
        {
            return Task.Run(() => db.DeleteAsync(group));
        }
    }
}
