using System;
using System.Collections.ObjectModel;

namespace gratch_core
{
    public interface IPerson : System.ICloneable
    {
        string Name { get; set; }
        Collection<DateTime> DutyDates { get; set; }
    }
}