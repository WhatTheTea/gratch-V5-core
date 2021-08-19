using gratch_core.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gratch_core
{
    internal static class PersonAdapter
    {
        internal static PersonModel GetModel(Person person)
        {
            PersonModel model = new();
            model.Index = Group.AllInstances.Single(grp => grp.Name == person.GroupName).IndexOf(person);
            model.Group = person.GroupName;
            model.Name = person.Name;
            model.DutyDates = person.DutyDates;

            return model;
        }
        internal static Person GetPerson(PersonModel model)
        {
            Person person = new(model.Name);
            person.DutyDates = model.DutyDates as ObservableCollection<DateTime>;
            person.GroupName = model.Group;

            return person;
        }
        internal static List<PersonModel> GetModels(List<Person> people)
        {
            var list = new List<PersonModel>();
            foreach(var person in people)
            {
                list.Add(GetModel(person));
            }
            return list;
        }
    }
}
