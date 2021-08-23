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
        };
        internal static Person ToPerson(this PersonModel model) => new(model.Name)
        {
            DutyDates = new ObservableCollection<DateTime>(
                JsonSerializer.Deserialize<List<DateTime>>(model.DutyDatesBlobbed))
        };
        internal static List<PersonModel> ToModels(this IList<Person> people)
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
            var grp = new Group();
            grp.Name = model.Name;
            grp.Graph.Weekend = new ObservableCollection<DayOfWeek>(
                JsonSerializer.Deserialize<List<DayOfWeek>>(model.WeekendBlobbed));
            foreach (var person in model.People)
            {
                grp.Add(person.ToPerson());
            }
            return grp;
        }
        internal static GroupModel ToModel(this Group group)
        {
            var model = new GroupModel
            {
                Id = Group.AllInstances.IndexOf(group) != -1 ? Group.AllInstances.IndexOf(group) + 1
                                                                  : throw new ArgumentNullException("Id", "groupId is not valid"),
                Name = group.Name,
                Weekend = group.Graph.Weekend.ToList(),
            };

            model.WeekendBlobbed = JsonSerializer.Serialize(model.Weekend);

            var people = group.ToModels();
            for (int i = 0; i < people.Count; i++)
            {
                people[i].Id = i + 1;
                people[i].GroupModel = model;
                people[i].GroupId = model.Id;
            }
            model.People = people;

            return model;
        }
    }
}
