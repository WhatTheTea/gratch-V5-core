using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

namespace gratch_core.Models
{
    internal class GroupRepository
    {
        private readonly SQLiteAsyncConnection db;
        public GroupRepository()
        {
            db = SQLiteDB.GetAsyncConnection();
            db.CreateTableAsync<PersonModel>();
        }
        public Task<List<PersonModel>> GetPeople()
        {
            return db.Table<PersonModel>().ToListAsync();
        }
        public Task<PersonModel> GetPerson(int id)
        {
            return db.GetAsync<PersonModel>(id);
        }
        public Task<int> DeletePerson(int id)
        {
            return db.DeleteAsync<PersonModel>(id);
        }
        public Task<List<PersonModel>> DeletePersonByIndex(int index, string group)
        {
            
        }
        public Task<int> SavePerson(PersonModel person)
        {
            return db.InsertOrReplaceAsync(person);
        }
        public Task<int> SavePeople(List<PersonModel> people)
        {
            int rows = 0;
            foreach(var person in people)
            {
                rows += SavePerson(person).Result;
            }
            return Task.FromResult<int>(rows);
        }
        public Task<List<PersonModel>> ClearAll(string groupName)
        {
            
        }
    }
}
