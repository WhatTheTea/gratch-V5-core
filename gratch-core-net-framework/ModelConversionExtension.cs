using gratch_core.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace gratch_core
{
    internal static class ModelConversionExtension
    {
        internal static PersonModel ToModel(this Person person, Group group)
        {
            PersonModel result = new PersonModel()
            {
                Name = person.Name,
                DutyDates = person.DutyDates.ToList(),
                GroupId = Group.AllInstances.IndexOf(group) + 1,
                Id = group.IndexOf(person) + 1
            };
            result.DutyDatesBlob = JsonSerializer.Serialize(
                person.DutyDates.Select(dd => dd.ToString("yyyy-MM-dd")));
            return result;
        }

        internal static Person ToPerson(this PersonModel model) => new Person(model.Name)
        {
            DutyDates = new System.Collections.ObjectModel.Collection<DateTime>(model.DutyDates)
        };
        internal static List<PersonModel> ToModels(this IList<Person> people, Group group)
        {
            var list = new List<PersonModel>();
            foreach (var person in people)
            {
                list.Add(person.ToModel(group));
            }
            return list;
        }
        internal static Group ToGroup(this GroupModel model)
        {
            var grp = new Group(model.Name);
            grp.Graph.Weekend = JsonSerializer.Deserialize<List<DayOfWeek>>(model.WeekendBlobbed);
            model.People.ForEach(p => grp.Add(p.ToPerson()));
            return grp;
        }
        internal static GroupModel ToModel(this Group group)
        {
            var model = new GroupModel()
            {
                Name = group.Name,
                People = group.ToModels(group),
                Weekend = group.Graph.Weekend.ToList(),
                Id = Group.AllInstances.IndexOf(group) + 1
            };
            model.WeekendBlobbed = JsonSerializer.Serialize(model.Weekend);
            return model;
        }
    }
}
