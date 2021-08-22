using SQLite;

using SQLiteNetExtensions.Attributes;

using System;
using System.Collections.Generic;

namespace gratch_core.Models
{
    public class PersonModel
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        [NotNull, ForeignKey(typeof(GroupModel))]
        public int GroupId { get; set; }
        [NotNull]
        public string Name { get; set; }
        [TextBlob("DutyDatesBlobbed")]
        public List<DateTime> DutyDates { get; set; }

        public string DutyDatesBlobbed { get; set; }
        [ManyToOne]
        public GroupModel GroupModel { get; set; }
    }
}
