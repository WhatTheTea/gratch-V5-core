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
            db = SQLiteDB.GetConnection();
            db.CreateTable<GroupModel>();
            db.CreateTable<PersonModel>();

            db.ExecuteScalar<string>("PRAGMA journal_mode = wal");
            db.ExecuteScalar<string>("PRAGMA synchronous = OFF");
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

            Group.subscriber.Dispose();
            GetAllGroups().ForEach(model =>
           {
               list.Add(model.ToGroup());
           });
            Group.subscriber = SQLiteSubscriber.GetSubscriber();

            return list;
        }
        public GroupModel GetGroup(string name) //=> GetAllGroups().FirstOrDefault(grp => grp.Name == name);
        {
            GroupModel result;
            try
            {
                result = db.GetWithChildren<GroupModel>(db.Table<GroupModel>()?.FirstOrDefault(g => g.Name == name)?.Id);
                result.People.ForEach(p => p.DutyDates = JsonSerializer.Deserialize<List<DateTime>>(p.DutyDatesBlob));
            }
            catch (InvalidOperationException)
            {
                result = null;
            }
            return result;
        }
        public void AddGroup(Group group)
        {
            db.Insert(group.ToModel());
        }
        public void UpdatePeople(Group group)
        {
            UpdatePeople(group.ToModel());
        }
        private void UpdatePeople(GroupModel group)
        {
            db.RunInTransaction(() =>
            {
                group.People.ForEach(p =>
                {
                    db.Execute("UPDATE PersonModel " +
                                "SET Name = ?, DutyDatesBlob = ? " +
                                "WHERE Id == ? AND GroupId == ?" +
                                "AND (Name != ? OR DutyDatesBlob != ?)", //6 аргументов
                                p.Name, p.DutyDatesBlob,
                                p.Id, p.GroupId,
                                p.Name, p.DutyDatesBlob);
                });
            });
        }

        public void AddPerson(Group group, Person person)
        {
            var added = person.ToModel(group); //новый человек
            db.Insert(added);
        }
        public void UpdateGroup(Group group)
        {
            var mod = group.ToModel();
            db.Update(mod);
            UpdatePeople(mod);
        }
        public void DeletePerson(Group group, Person person)
        {
            var model = group.ToModel();
            PersonModel deleted = db.GetWithChildren<GroupModel>(model.Id).People.First(p => p.Name == person.Name); //несуществующий человек
            db.Delete(deleted);
            db.RunInTransaction(() =>
            {
                foreach (var p in group)
                {
                    db.Execute("UPDATE PersonModel " +
                    "SET Id = ? " + // 3 аргумента
                    "WHERE GroupId == ? AND Name == ?",
                    group.IndexOf(p) + 1,
                         model.Id, p.Name
                    );
                }
            }
            );

            UpdatePeople(model);
        }

        public void DeleteGroup(Group group)
        {
            var deletedGroupId = GetGroup(group.Name).Id;
            db.Delete<GroupModel>(deletedGroupId);

            db.Execute("DELETE FROM PersonModel " +
                "WHERE GroupId == ?", deletedGroupId);
            if (db.Table<GroupModel>().Count() == 0) DeleteAll();
        }

        public void DeleteAll()
        {
            db.DropTable<GroupModel>();
            db.DropTable<PersonModel>();
            db.CreateTable<GroupModel>();
            db.CreateTable<PersonModel>();
        }

        #region IDisposable
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    db.Close();
                }
                disposed = true;

                db.Dispose();
            }
        }

        ~GroupRepository()
        {
            Dispose(false);
        }
        #endregion
    }
}
