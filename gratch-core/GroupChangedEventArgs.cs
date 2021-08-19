using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gratch_core
{
    internal enum GroupChangedEventType
    {
        GroupNameChanged,
        WeekendChanged
    }
    internal class GroupChangedEventArgs
    {
        internal GroupChangedEventType EventType { get; } = new GroupChangedEventType();
        internal GroupChangedEventArgs(GroupChangedEventType type)
        {
            EventType = type;
        }
    }
}
