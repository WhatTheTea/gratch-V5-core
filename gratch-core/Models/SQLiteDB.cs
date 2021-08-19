using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gratch_core.Models
{
    public static class SQLiteDB
    {
        const string DBName = "gratch.db3";
        readonly static string DBPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public static SQLite.SQLiteAsyncConnection GetAsyncConnection()
        {
            return new SQLite.SQLiteAsyncConnection(Path.Combine(DBPath, DBName));
        }
    }
}
