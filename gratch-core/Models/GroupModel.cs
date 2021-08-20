﻿using SQLite;

using SQLiteNetExtensions.Attributes;

using System;
using System.Collections.Generic;

namespace gratch_core.Models
{
    public class GroupModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public string Name { get; set; }
        [OneToMany]
        public List<PersonModel> People { get; set; }
        [TextBlob("WeekendBlobbed")]
        public List<DayOfWeek> Weekend { get; set; }

        public string WeekendBlobbed { get; set; }
    }
}