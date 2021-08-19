using SQLite;

using SQLiteNetExtensions.Attributes;

using System;
using System.Collections.ObjectModel;

namespace gratch_core.Models
{
    public class PersonModel
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        [NotNull, ForeignKey(typeof(GroupModel))]
        public string GroupId { get; set; }
        [NotNull, Unique]
        public string Name { get; set; }
        [TextBlob("DutyDatesBlobbed")]
        public Collection<DateTime> DutyDates { get; set; }

        public string DutyDatesBlobbed { get; set; }
    }
}
