using gratch_core.Models;

using System;
using System.Collections.Generic;
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
            person.DutyDates = model.DutyDates;
            person.GroupName = model.Group;

            return person;
        }
    }
}
