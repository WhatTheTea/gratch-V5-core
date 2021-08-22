using System;
using System.Collections.ObjectModel;

namespace gratch_core
{
    public interface IPerson : System.ICloneable
    {
        string Name { get; set; }
        ObservableCollection<DateTime> DutyDates { get; set; }
    }
}