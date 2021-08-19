using SQLite;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace gratch_core.Models
{
    [Table(SQLiteDB.GroupTableName)]
    public class PersonModel
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public int Index { get; set; }
        [NotNull, MaxLength(50)]
        public string Group { get; set; }
        [NotNull, MaxLength(50)]
        public string Name { get; set; }
        public Collection<DateTime> DutyDates { get; set; }
    }
}
