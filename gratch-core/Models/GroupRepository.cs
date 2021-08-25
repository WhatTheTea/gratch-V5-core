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
    public class GroupRepository
    {
        private readonly SQLiteConnection db;
        public GroupRepository()
        {
            db = SQLiteDB.GetAsyncConnection();
            db.CreateTable<GroupModel>();
            db.CreateTable<PersonModel>();
        }
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

        public void InsertGroup(Group group)
        {
            db.Insert(group.ToModel());
        }
        public void UpdatePeople(Group group)
        {
            var mod = group.ToModel();
            db.BeginTransaction();
            foreach (var p in mod.People)
            {
                db.RunInTransaction(() =>
                {
                    db.Execute("UPDATE PersonModel " +
                            "SET Name = ?, DutyDatesBlob = ? " +
                            "WHERE Id LIKE ? AND GroupId LIKE ?" +
                            "AND (Name NOT LIKE ? OR DutyDatesBlob NOT LIKE ?)", //6 аргументов
                            p.Name, p.DutyDatesBlob,
                            p.Id, p.GroupId,
                            p.Name, p.DutyDatesBlob);
                });

            }
            db.Commit();
        }

        public void AddPerson(Group group, Person person)
        {
            var mod = group.ToModel();
            var added = mod.People.First(p => p.Name == person.Name); //новый человек
            db.InsertWithChildren(added);
        }
        public void UpdateGroup(Group group)
        {
            var mod = group.ToModel();
            db.Update(mod);
            UpdatePeople(group);
        }
        public void DeletePerson(Group group, Person person)
        {
            var allGroups = GetAllGroups();
            var model = group.ToModel();
            PersonModel deleted = allGroups[model.Id - 1].People.First(p => p.Name == person.Name); //несуществующий человек
            db.Delete(deleted);

            db.BeginTransaction(); 
            foreach (var p in group)
            {
                db.RunInTransaction(() =>
                db.Execute("UPDATE PersonModel " +
                "SET Id = ? " + // 3 аргумента
                "WHERE GroupId LIKE ? AND Name LIKE ?",
                group.IndexOf(p) + 1,
                     model.Id, p.Name)
                );
            }
            db.Commit();

            UpdatePeople(group);
        }

        public void DeleteGroup(Group group)
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
