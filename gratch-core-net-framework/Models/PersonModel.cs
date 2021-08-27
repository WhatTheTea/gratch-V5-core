using SQLite;

using SQLiteNetExtensions.Attributes;

using System;
using System.Collections.Generic;

namespace gratch_core.Models
{
    public class PersonModel
    {
        [AutoIncrement, PrimaryKey]
#pragma warning disable IDE1006 // Стили именования
        public int _id { get; set; }
#pragma warning restore IDE1006 // Стили именования
        [NotNull]
        public int Id { get; set; }
        [NotNull, ForeignKey(typeof(GroupModel))]
        public int GroupId { get; set; }
        [NotNull]
        public string Name { get; set; }
        //[TextBlob("DutyDatesBlobbed")]
        [Ignore]
        public List<DateTime> DutyDates { get; set; }

        public string DutyDatesBlob { get; set; }
    }
}
