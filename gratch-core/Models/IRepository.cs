using System.Collections.Generic;

namespace gratch_core.Models
{
    public interface IRepository<G, P>
        where G : IGroup
        where P : IPerson
    {
        void AddGroup(G group);
        void AddPerson(G group, P person);
        void DeleteAll();
        void DeleteGroup(G group);
        void DeletePerson(G group, P person);
        List<GroupModel> GetAllGroups();
        GroupModel GetGroup(string name);
        void UpdateGroup(G group);
        void UpdatePeople(G group);
    }
}