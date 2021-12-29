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
                $"and access = {access} order by time;";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        chatinglogs.Add(new Chatinglog(
                            reader.GetInt32("ownerid"),
                            DateTime.Parse(reader.GetString("time")),
                            reader.GetString("content"),
                            reader.GetInt32("access")));
                    }
                }
                conn.Close();
            }
            return chatinglogs;
        }

        public int findPermission(int fir, int sec)
        {
            if (fir == 0 || sec == 0)
                return 0;
            int ret = -1;
  
            string SQLqurey = $"select permission from room where json_contains(data, '{fir}', '$.members') and json_contains(data, '{sec}', '$.members');";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        ret = reader.GetInt32("permission");
                    else
                        ret = -1;
                }
                conn.Close();
            }
            return ret;
        }

        public string insertPermission(int fir, int sec)
        {
            string SQLqurey = $"insert into room (data, name)value(json_object('owner', {fir}," +
                $"'members', json_array({fir}, {sec})), '{fir}&{sec}');";
            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                    if (command.ExecuteNonQuery() == 0)
                        Console.WriteLine("DB makeRoom func fail");
                }
                catch (Exception exception)
                {
                    Console.WriteLine("DB connecttion Fail");
                    Console.WriteLine(exception.ToString());
                }
                conn.Close();
                return $"{SQLqurey} OK";
            }
        }

        public string makeRoom(int ownerid, string roomname, List<int> members)
        {
            string memberstring = "";
            foreach(int member in members)
                memberstring += member.ToString() + ", ";
            memberstring = memberstring.Substring(0, memberstring.Length - 2);
            string SQLqurey = $"insert into room (data, name)value(json_object('owner', {ownerid}," +
                $"'members', json_array({memberstring})), '{roomname}');";
            Console.WriteLine(SQLqurey);
            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                    if (command.ExecuteNonQuery() == 0)
                        Console.WriteLine("DB makeRoom func fail");
                }
                catch (Exception exception)
                {
                    Console.WriteLine("DB connecttion Fail");
                    Console.WriteLine(exception.ToString());
                }
                conn.Close();
                return $"{SQLqurey} OK";
            }
        }

        public List<Room> findRooms(int myid)
        {
            List<Room> rooms = new List<Room>();
            string SQLqurey = $"select permission, name from room where json_contains(data, '{myid}', '$.members');";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rooms.Add(new Room(
                            reader.GetInt32("permission"),
                            reader.GetString("name")));
                    }
                }
                conn.Close();
            }
            return rooms;
        }
    }
}