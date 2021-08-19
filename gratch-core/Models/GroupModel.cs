using System;
using System.Collections.Generic;

using SQLite;

using SQLiteNetExtensions.Attributes;

namespace gratch_core.Models
{
    public class GroupModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public string Name { get; set; }
        [OneToMany]
        public List<Person> People { get; set; }
        [TextBlob("WeekendBlobbed")]
        public List<DateTime> Weekend { get; set; }

        public string WeekendBlobbed { get; set; }
    }
}
