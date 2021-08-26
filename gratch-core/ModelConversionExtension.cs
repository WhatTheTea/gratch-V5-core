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
        internal static PersonModel ToModel(this Person person) => new()
        {
            Name = person.Name,
            DutyDates = person.DutyDates.ToList()
        };
        internal static Person ToPerson(this PersonModel model) => new(model.Name)
        {
            DutyDates = new(model.DutyDates)
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
            var grp = new Group(model.Name);
            grp.Graph.Weekend = JsonSerializer.Deserialize<List<DayOfWeek>>(model.WeekendBlobbed);
            foreach (var person in model.People)
            {
                grp.Add(person.ToPerson());
            }
            return grp;
        }
        internal static GroupModel ToModel(this IGroup group)
        {
            var model = new GroupModel()
            {
                Name = group.Name,
                People = group.ToModels(),
                Weekend = group.Graph.Weekend.ToList(),
                Id = IGroup.AllInstances.IndexOf(group) + 1
            };
            model.WeekendBlobbed = JsonSerializer.Serialize(model.Weekend);
            model.People.ForEach(p =>
            {
                p.Id = model.People.IndexOf(p) + 1;
                p.GroupId = model.Id;
                p.GroupModel = model;
                p.DutyDatesBlob = new string(JsonSerializer.Serialize(group[model.People.IndexOf(p)]
                    .DutyDates.Select(dd => dd.ToString("yyyy-MM-dd"))));
#if LOGGING
                Console.WriteLine(DateTime.Now + $" | MCE | {p.Name} DutyDatesBlob: {p.DutyDatesBlob}");
#endif
            });
            return model;
        }
    }
}
