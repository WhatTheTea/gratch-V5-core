using System;
using System.IO;

namespace gratch_core.Models
{
    public static class SQLiteDB
    {
        private const string DBName = "gratch.db3";
        private static readonly string DBPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static SQLite.SQLiteConnection GetAsyncConnection()
        {
            return new SQLite.SQLiteConnection(Path.Combine(DBPath, DBName));
        }
    }
}
