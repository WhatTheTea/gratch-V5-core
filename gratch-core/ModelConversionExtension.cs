using gratch_core.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;

namespace gratch_core
{
    internal static class ModelConversionExtension
    {
        private static PersonModel ToModel(this Person person) => new()
        {
            Name = person.Name,
            DutyDates = person.DutyDates.ToList(),
            DutyDatesBlob = JsonSerializer.Serialize(person
                    .DutyDates.Select(dd => dd.ToString("yyyy-MM-dd")))
        };
        private static Person ToPerson(this PersonModel model) => new(model.Name)
        {
            DutyDates = JsonSerializer.Deserialize<Collection<DateTime>>(model.DutyDatesBlob)
        };
        private static List<PersonModel> ToModels(this IList<Person> people)
        {
            var list = new List<PersonModel>();
            foreach (var person in people)
            {
                list.Add(person.ToModel());
            }
            return list;
        }
        internal static Group ToGroup(this GroupModel model)
        {
            var grp = new Group(model.Name);
            grp.Graph.Weekend = new ObservableCollection<DayOfWeek>(
                JsonSerializer.Deserialize<List<DayOfWeek>>(model.WeekendBlobbed));
            foreach (var person in model.People)
            {
                grp.Add(person.ToPerson());
            }
            return grp;
        }
        internal static GroupModel ToModel(this IGroup group, bool renamed = false)
        {
            GroupRepository repository = new GroupRepository();
            var allGroupsInDB = repository.GetAllGroups();

            GroupModel groupModel = new GroupModel();
            if (allGroupsInDB.Exists(grp => grp.Name == group.Name)) //Если группа существует, значит она просто обновляется
            {
                groupModel.Id = allGroupsInDB.FindIndex(grp => grp.Name == group.Name) + 1;
            }
            else if (renamed) //Если не существует, то вставляется новая, где уже работает библиотека, но если группу переименовали, то:
            {
                throw new NotImplementedException();
            }

            groupModel.Name = group.Name;

            groupModel.People = group.ToModels();
            groupModel.People.ForEach(mod =>
            {
                mod.GroupModel = groupModel;
                mod.Id = groupModel.People.IndexOf(mod) + 1;
            });

            groupModel.Weekend = group.Graph.Weekend.ToList();
            groupModel.WeekendBlobbed = JsonSerializer.Serialize(groupModel.Weekend);

            return groupModel;
        }
    }
}
