using System;
using System.Collections.Generic;

namespace gratch_core.Models
{
    public interface IRepository<T1, T2> : IDisposable
        where T1 : IGroup
        where T2 : IPerson
    {
        void AddGroup(T1 group);
        void AddPerson(T1 group, T2 person);
        void DeleteAll();
        void DeleteGroup(T1 group);
        void DeletePerson(T1 group, T2 person);
        List<GroupModel> GetAllGroups();
        GroupModel GetGroup(string name);
        void UpdateGroup(T1 group);
        void UpdatePeople(T1 group);
    }
}