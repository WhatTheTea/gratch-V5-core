using SQLite;

using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensions.Extensions.TextBlob;

using System.Collections.Generic;
using System.Linq;

namespace gratch_core.Models
{
    public class GroupRepository : IGroupRepository
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
        public List<GroupModel> GetAllGroups()
        {
            return db.GetAllWithChildren<GroupModel>();
        }
        public List<IGroup> LoadAllGroups()
        {
            var list = new List<IGroup>();
            Group.listener.Destroy();
            foreach(var mod in GetAllGroups())
            {
                list.Add(mod.ToGroup());
            }
            Group.listener = SQLiteListener.GetListener();
            return list;
        }
        public GroupModel GetGroup(string name)
        {
            return GetAllGroups().FirstOrDefault(grp => grp.Name == name);
        }
        public GroupModel GetGroup(int id)
        {
            return db.GetWithChildren<GroupModel>(id);
        }
        public PersonModel GetPerson(string name, GroupModel group) => db.Table<PersonModel>()
            .FirstOrDefault(pers => pers.GroupModel == group);
        #endregion

        public void InsertGroup(GroupModel group)
        {
            db.Insert(group);
            db.UpdateWithChildren(group);
        }
        public void UpdateGroup(GroupModel group) => db.Update(group);
        public void DeleteGroup(GroupModel group) => db.Delete(group);
        public void InsertPerson(PersonModel person) => db.InsertWithChildren(person);
        public void DeletePerson(PersonModel person) => db.Delete(person);
        public void UpdatePerson(PersonModel person) => db.UpdateWithChildren(person);
        public void DeleteAll()
        {
            db.DropTable<GroupModel>();
            db.DropTable<PersonModel>();
            db.CreateTable<GroupModel>();
            db.CreateTable<PersonModel>();
        }
    }
}
