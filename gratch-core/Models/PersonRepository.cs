using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

namespace gratch_core.Models
{
    public class PersonRepository
    {
        private readonly SQLiteAsyncConnection db;
        public PersonRepository()
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
        public Task<List<PersonModel>> DeletePersonByIndex(int index)
        {
            return db.QueryAsync<PersonModel>("DELETE FROM graph WHERE Index=?",index);
        }
        public Task<int> SavePerson(PersonModel person)
        {
            if (person.Index != 0)
            {
                db.UpdateAsync(person);
                return Task.FromResult(person.Id);
            }
            else return db.InsertAsync(person);
        }
    }
}
