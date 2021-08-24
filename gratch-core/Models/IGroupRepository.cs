using System.Collections.Generic;

namespace gratch_core.Models
{
    public interface IGroupRepository
    {
        void DeleteAll();
        GroupModel GetGroup(int id);
        GroupModel GetGroup(string name);
        List<GroupModel> GetAllGroups();
        List<IGroup> LoadAllGroups();
        void InsertGroup(Group group);
        void UpdateGroup(Group group);
        void DeleteGroup(Group group);
    }
}