using SQLite;

using System;
using System.Collections.Generic;

namespace gratch_core.Models
{
    [Table("graph")]
    public class PersonModel
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public int Index { get; set; }
        [NotNull, MaxLength(50)]
        public string Group { get; set; }
        [NotNull, MaxLength(50)]
        public string Name { get; set; }
        public List<DateTime> DutyDates { get; set; }
    }
}
