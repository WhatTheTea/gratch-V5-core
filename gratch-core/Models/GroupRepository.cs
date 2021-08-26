#define LOGGING
#undef LOGGING

using SQLite;

using SQLiteNetExtensions.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace gratch_core.Models
{
    public class GroupRepository : IRepository<Group, Person>
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

            Group.subscriber.Destroy();
            GetAllGroups().ForEach(model =>
           {
               list.Add(model.ToGroup());
           });
            Group.subscriber = SQLiteSubscriber.GetSubscriber();

            return list;
        }
        public GroupModel GetGroup(string name) //=> GetAllGroups().FirstOrDefault(grp => grp.Name == name);
        {
            var result = db.GetWithChildren<GroupModel>(db.Table<GroupModel>().First(g => g.Name == name).Id);
            result.People.ForEach(p => p.DutyDates = JsonSerializer.Deserialize<List<DateTime>>(p.DutyDatesBlob));
            return result;
        }
        public void AddGroup(Group group)
        {
            db.Insert(group.ToModel());
        }
        public void UpdatePeople(Group group)
        {
            db.RunInTransaction(() =>
            {
                group.ToModel().People.ForEach(p =>
                {
                    db.Execute("UPDATE PersonModel " +
                                "SET Name = ?, DutyDatesBlob = ? " +
                                "WHERE Id LIKE ? AND GroupId LIKE ?" +
                                "AND (Name NOT LIKE ? OR DutyDatesBlob NOT LIKE ?)", //6 аргументов
                                p.Name, p.DutyDatesBlob,
                                p.Id, p.GroupId,
                                p.Name, p.DutyDatesBlob);
                });
            });
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
            db.RunInTransaction(() =>
            {
                foreach(var p in group)
                {
                    db.Execute("UPDATE PersonModel " +
                    "SET Id = ? " + // 3 аргумента
                    "WHERE GroupId LIKE ? AND Name LIKE ?",
                    group.IndexOf(p) + 1,
                         model.Id, p.Name
                    );
                }
            }
            );

            UpdatePeople(group);
        }

        public void DeleteGroup(Group group)
        {
            var table = db.Table<GroupModel>();
            var deletedgroupId = (from g in table
                                  where g.Name == @group.Name
                                  select g.Id).First();
            db.Delete<GroupModel>(deletedgroupId);

            db.RunInTransaction(() => db.Execute("DELETE FROM PersonModel " +
                "WHERE GroupId LIKE ?", deletedgroupId));
            if (table.Count() == 0) DeleteAll();
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
