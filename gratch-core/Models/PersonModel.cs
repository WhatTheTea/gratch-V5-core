using SQLite;

using System;
using System.Collections.Generic;

namespace gratch_core.Models
{
    public class PersonModel
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        [NotNull, MaxLength(50)]
        public string Group { get; set; }
        [NotNull, MaxLength(50)]
        public string Name { get; set; }
        public List<DateTime> DutyDates { get; set; }
    }
}
