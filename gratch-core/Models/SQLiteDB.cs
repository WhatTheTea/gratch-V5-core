using System;
using System.IO;

namespace gratch_core.Models
{
    public static class SQLiteDB
    {
        private const string DBName = "gratch.db3";
        private static readonly string DBPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        private static string _fullPath;
        public static string FullPath { get => _fullPath; }
        public static SQLite.SQLiteConnection GetAsyncConnection()
        {
            _fullPath = Path.Combine(DBPath, DBName);
            return new SQLite.SQLiteConnection(FullPath, true);
        }
    }
}
