using MySql.Data.MySqlClient;
using System;

namespace modoomoyeo.Database
{
    public class ChatingQurey : DBConnnection
    {
        public ChatingQurey(string connectionString) : base(connectionString)
        {
        }

        public string insertLog(Chatinglog chatinglog)
        {

            string SQLqurey = $"insert into chatlog (ownerid, time, content, access)values(" +
                $"'{chatinglog.ownerid}','{chatinglog.time:yyyy/MM/dd HH/mm/ss}','{chatinglog.contents}', '{chatinglog.access_code}')";
            Console.WriteLine(SQLqurey);
            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                    if (command.ExecuteNonQuery() == 0)
                        Console.WriteLine("DB chating login fail");
                }
                catch (Exception exception)
                {
                    Console.WriteLine("DB connecttion Fail");
                    Console.WriteLine(exception.ToString());
                }
                conn.Close();
                return "OK";
            }
        }

        public List<Chatinglog> findLog(DateTime start, DateTime end, int access)
        {
            List<Chatinglog> chatinglogs = new List<Chatinglog>();
            string SQLqurey = $"select * from chatlog where time between '{start:yyyy/MM/dd HH/mm/ss}' and '{end:yyyy/MM/dd HH/mm/ss}' " +
                $"and access = {access};";
            Console.WriteLine(SQLqurey);
            Console.WriteLine(SQLqurey);
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                using (var reader = command.ExecuteReader())
                {
                    chatinglogs.Add(new Chatinglog(
                        reader.GetInt32("ownerid"),
                        DateTime.Parse(reader.GetString("time")),
                        reader.GetString("content"),
                        reader.GetInt32("id")));
                }
                conn.Close();
            }
            return chatinglogs;
        }
    }
}
