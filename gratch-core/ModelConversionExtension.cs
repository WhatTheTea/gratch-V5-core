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
            DutyDatesBlob = JsonSerializer.Serialize(person.DutyDates)
        };
        internal static Person ToPerson(this PersonModel model) => new(model.Name)
        {
            DutyDates = new Collection<DateTime>(
                JsonSerializer.Deserialize<List<DateTime>>(model.DutyDatesBlob))
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
            var grp = new Group
            {
                Name = model.Name
            };
            grp.Graph.Weekend = new ObservableCollection<DayOfWeek>(
                JsonSerializer.Deserialize<List<DayOfWeek>>(model.WeekendBlobbed));
            foreach (var person in model.People)
            {
                grp.Add(person.ToPerson());
            }
            return grp;
        }
        /*internal static GroupModel ToModel(this Group group)
        {
            //Последняя группа для получения GroupID при вставке
            var lastGrp = new GroupRepository()?.GetAllGroups()?.LastOrDefault();
            var model = new GroupModel
            {
                Id = lastGrp == null ? 1 : Group.AllInstances.IndexOf(group) + 1, //если последней группы нету, значит это первая
                Name = group.Name,
                Weekend = group.Graph.Weekend.ToList(),
            };

            model.WeekendBlobbed = JsonSerializer.Serialize(model.Weekend);

            var people = group.ToModels();
            people.ForEach(p =>
            {
                if (people.IndexOf(p) == 0)// Если он первый, то присвоить 1, иначе
                {
                    p.Id = 1;
                }
                else
                {
                    int globalCount = 0;
                    new GroupRepository().GetAllGroups().ForEach(grp =>
                    {
                        if (grp.Name != group.Name)
                        {
                            globalCount += grp.People.Count;
                        }
                    });
                    p.Id = globalCount + people.IndexOf(p) + 1;
                }
                p.GroupId = model.Id;
                p.GroupModel = model;
                p.DutyDatesBlobbed = JsonSerializer.Serialize(p.DutyDates);
            });
            model.People = people;

            return model;
        }*/
        internal static GroupModel ToModel(this Group group, bool renamed = false)
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
