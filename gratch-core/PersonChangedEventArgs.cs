using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gratch_core
{
    internal class PersonChangedEventArgs
    {
        public PersonChangedEventType EventType { get; }
        public string GroupName { get; }
        internal PersonChangedEventArgs(PersonChangedEventType type, string groupName)
        {
            EventType = type;
            GroupName = groupName;
        }
    }
    internal enum PersonChangedEventType
    {
        PersonChanged,
        PersonAdded,
        PersonRemoved,
        AllPeopleRemoved
    }
}
