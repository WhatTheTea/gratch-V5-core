using System.Collections.Generic;

namespace gratch_core.Models
{
    public interface IGroupRepository
    {
        void DeleteAll();
        void DeleteGroup(GroupModel group);
        void DeletePerson(PersonModel person);
        GroupModel GetGroup(int id);
        GroupModel GetGroup(string name);
        List<GroupModel> GetAllGroups();
        List<IGroup> LoadAllGroups();
        void InsertGroup(GroupModel group);
        void InsertPerson(PersonModel person);
        void UpdateGroup(GroupModel group);
        void UpdatePerson(PersonModel person);
    }
}