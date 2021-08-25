#define LOGGING
#undef LOGGING

using SQLite;

using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensions.Extensions.TextBlob;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace gratch_core.Models
{
    public class GroupRepository// : IGroupRepository
    {
        private readonly SQLiteConnection db;
        public GroupRepository()
        {
            db = SQLiteDB.GetAsyncConnection();
            db.CreateTable<GroupModel>();
            db.CreateTable<PersonModel>();
            TextBlobOperations.SetTextSerializer(Newtonsoft.Json.JsonSerializer.Create() as ITextBlobSerializer);
        }
        public enum UpdateType
        {
            PersonChanged,
            PersonAdded,
            PersonRemoved,
            GroupChanged
        }
        #region Getters
        public List<GroupModel> GetAllGroups()
        {
            var result = db.GetAllWithChildren<GroupModel>();
            result.ForEach(g => g.People.ForEach(p => p.DutyDates = JsonSerializer.Deserialize<List<DateTime>>(p.DutyDatesBlob)));
            return result;
        }
        public List<IGroup> LoadAllGroups()
        {
            var list = new List<IGroup>();

            Group.listener.Destroy();
            foreach (var mod in GetAllGroups())
            {
                list.Add(mod.ToGroup());
            }
            Group.listener = SQLiteListener.GetListener();

            return list;
        }
        public GroupModel GetGroup(string name) => GetAllGroups().FirstOrDefault(grp => grp.Name == name);
        public GroupModel GetGroup(int id)
        {
            return db.GetWithChildren<GroupModel>(id);
        }
        public PersonModel GetPerson(string name, GroupModel group) => GetAllGroups()
            .FirstOrDefault(grp => grp.Name == group.Name).People
            .FirstOrDefault(pers => pers.GroupModel == group);
        #endregion

        public void InsertGroup(Group group)
        {
            db.Insert(group.ToModel());
        }

        public void UpdateGroup(Group group, UpdateType type)
        {
            switch (type)
            {
                case UpdateType.GroupChanged:
                    Update(group);
                    break;
                case UpdateType.PersonAdded:
                    Update_AddPerson(group);
                    break;
                case UpdateType.PersonChanged:
                    Update_People(group);
                    break;
                case UpdateType.PersonRemoved:
                    Update_DeletePerson(group);
                    break;
            }
        }

        private void Update_People(Group group)
        {
            var mod = group.ToModel();
            mod.Id = Group.AllInstances.IndexOf(group) + 1;
            mod.People.ForEach(p =>
            {
                p.GroupId = mod.Id;
                p.GroupModel = mod;
                p.DutyDatesBlob = new string(JsonSerializer.Serialize(group[mod.People.IndexOf(p)]
                    .DutyDates.Select(dd => dd.ToString("yyyy-MM-dd"))));
                //p.DutyDatesBlob = new string(JsonSerializer.Serialize(group[mod.People.IndexOf(p)].DutyDates));
#if LOGGING
                Console.WriteLine(DateTime.Now + $" | Repository | {p.Name} DutyDatesBlob: {p.DutyDatesBlob}");
#endif
            });
#if LOGGING
            int RowsChanged = 0;
#endif
            db.BeginTransaction();
            foreach (var p in mod.People)
            {
                db.RunInTransaction(() =>
                {
#if LOGGING
                    RowsChanged +=
#endif
                    db.Execute("UPDATE PersonModel " +
                            "SET Name = ?, DutyDatesBlob = ? " +
                            "WHERE Id LIKE ? AND GroupId LIKE ?" +
                            "AND (Name NOT LIKE ? OR DutyDatesBlob NOT LIKE ?)", //6 аргументов
                            p.Name, p.DutyDatesBlob,
                            p.Id, p.GroupId,
                            p.Name, p.DutyDatesBlob);
                });

            }
#if LOGGING
            Console.WriteLine(DateTime.Now + $" | Repository | RowsChanged: {RowsChanged}");
#endif
            db.Commit();
        }

        private void Update_AddPerson(Group group)
        {
            var allGroups = GetAllGroups();
            var mod = group.ToModel();
            mod.Id = Group.AllInstances.IndexOf(group) + 1;
            mod.People.ForEach(p => p.GroupId = mod.Id);

            var person = mod.People.First(pers => !allGroups[mod.Id - 1]
                                            .People.Any(permod => pers.Name == permod.Name)); //новый человек
            person.GroupModel = mod;
            db.InsertWithChildren(person);
        }
        private void Update(Group group)
        {
            var mod = group.ToModel();
            mod.Id = Group.AllInstances.IndexOf(group);
            mod.People.ForEach(p => p.GroupId = mod.Id);
            db.Update(mod);
            Update_People(group);
        }
        private void Update_DeletePerson(Group group)
        {
            var allGroups = GetAllGroups();
            var mod = group.ToModel();
            mod.Id = Group.AllInstances.IndexOf(group) + 1;
            mod.People.ForEach(p => p.GroupId = mod.Id);

            var person = allGroups[mod.Id - 1].People.First(pers => !mod
                                            .People.Any(permod => pers.Name == permod.Name)); //несуществующий человек
            person.GroupModel = mod;
            db.Delete(person);

            db.BeginTransaction();
            foreach(var p in group)
            {
                db.RunInTransaction(() => 
                db.Execute("UPDATE PersonModel " +
                "SET Id = ? " + // 3 аргумента
                "WHERE GroupId LIKE ? AND Name LIKE ?",
                group.IndexOf(p) + 1,
                     mod.Id, p.Name)
                );
            }
            db.Commit();

            Update_People(group);
        }

        public void DeleteGroup(IGroup group)
        {
            var deletedgroupId = (from g in GetAllGroups()
                                 where g.Name == @group.Name
                                 select g.Id).First();
            db.Delete<GroupModel>(deletedgroupId);

            db.BeginTransaction();
            db.RunInTransaction(() => db.Execute("DELETE FROM PersonModel " +
                "WHERE GroupId LIKE ?", deletedgroupId));
            db.Commit();
            if (GetAllGroups().Count == 0) DeleteAll();
        }

        public void DeleteAll()
        {
            db.DropTable<GroupModel>();
            db.DropTable<PersonModel>();
            db.CreateTable<GroupModel>();
            db.CreateTable<PersonModel>();
        }
    }
}
