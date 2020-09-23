using System;
using MySql.Data.MySqlClient;

namespace DBConnect
{
    public static class DBCon
    {
        private const string host = "mysql60.hostland.ru";
        private const string port = "3306";
        private const string username = "host1323541_itstep";
        private const string password = "269f43dc";
        private const string database = "host1323541_itstep27";

        private const string connString = "Server=" + host + ";Database=" + database + ";port=" + port + ";User Id=" + username + ";password=" + password;
        private static MySqlConnection db = new MySqlConnection(connString);
        private static MySqlCommand command = new MySqlCommand();

        public static void Open()
        {
            db.Open();
        }
        public static void Close()
        {
            db.Close();
        }

        public static MySqlDataReader SelectQuery(string sql)
        {
            command.Connection = db;
            command.CommandText = sql;
            var result = command.ExecuteReader();
            return result;
        }
        public static bool InsertQuery(string sql)
        {
            command.CommandText = sql;
            var is_complete = command.ExecuteNonQuery();
            if(is_complete > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
