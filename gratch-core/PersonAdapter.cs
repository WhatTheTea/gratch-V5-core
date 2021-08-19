using gratch_core.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gratch_core
{
    public class PersonAdapter
    {
        public static PersonModel GetModel(Person person)
        {
            PersonModel model = new();
            model.Group = Group.AllInstances.Single(group => group.Contains(person)).Name;
            model.Name = person.Name;
            model.DutyDates = person.DutyDates;

            return model;
        }
        public static Person GetPerson(PersonModel model)
        {
            Person person = new(model.Name);
            person.DutyDates = model.DutyDates;
            person.GroupName = model.Group;
            return person;
        }
    }
}
