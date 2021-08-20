using gratch_core.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
            DutyDates = new ObservableCollection<DateTime>(model.DutyDates),
            GroupName = new GroupRepository().GetGroup(model.GroupId).Result.Name,
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
            grp.Graph.Weekend = new ObservableCollection<DayOfWeek>(model.Weekend);
            foreach (var person in model.People)
            {
                grp.Add(person.ToPerson());
            }
            return grp;
        }
        internal static GroupModel ToModel(this Group group) => new GroupModel
        {
            Name = group.Name,
            People = group.ToModels(),
            Weekend = group.Graph.Weekend.ToList(),
        };
    }
}
