using System;
using System.IO;
using SQLite;

namespace EverNoteApp.ViewModel
{
    public class DataBaseHelper
    {
        public static string dbfile = Path.Combine(Environment.CurrentDirectory, "dbData.db");

        public static int Insert<T>(T item)
        {
            int records = 0;
            using (SQLiteConnection con = new SQLiteConnection(dbfile))
            {
                con.CreateTable<T>();
                records = con.Insert(item);
            }
            return records;
        }
        public static bool Delete<T>(T item)
        {
            int records = 0;
            using (SQLiteConnection con = new SQLiteConnection(dbfile))
            {
                records = con.Delete(item);
            }
            return records > 0 ? true : false;
        }
        public static bool Update<T>(T item)
        {
            int records = 0;
            using (SQLiteConnection con = new SQLiteConnection(dbfile))
            {
                records = con.Update(item);
            }
            return records > 0 ? true : false;
        }

        public static bool tableExists<T>()
        {
            using (SQLiteConnection con = new SQLiteConnection(dbfile))
            {
                string cmdText = "SELECT * FROM sqlite_master WHERE type = 'table' AND name = ?";
                var cmd = con.CreateCommand(cmdText, typeof(T).Name);
                //ExecuteScalar returns first col of first row if exists.
                return cmd.ExecuteScalar<String>()!=null;
                //return true;
            }
        }


    }
}
