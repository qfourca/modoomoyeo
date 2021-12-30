using MySql.Data.MySqlClient;

namespace modoomoyeo.Database
{
    public class ScheduleQurey : DBConnnection
    {
        public ScheduleQurey(string connectionString) : base(connectionString)
        {
        }

        public string insertSchedule(ScheduleData schedule)
        {
            string SQLqurey = $"insert into schedule" +
                $" values('{schedule.name}', {schedule.owner}, '{schedule.contents}'," +
                $"'{schedule.begin:yyyy/MM/dd}','{schedule.end:yyyy/MM/dd}'," +
                $" '{schedule.access}')";
            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                    Console.WriteLine("Schedule insert success" + (command.ExecuteNonQuery() == 1 ?
                        "success" : "fail"));
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

        public List<ScheduleData> scheduleDatas()
        {
            List<ScheduleData> scheduleDatas = new List<ScheduleData>();
            string SQLqurey = $"SELECT * from schedule;";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        scheduleDatas.Add(new ScheduleData(
                            reader.GetString("name"),
                            reader.GetInt32("owner"),
                            reader.GetString("contents"),
                            DateTime.Parse(reader.GetString("begintime")),
                            DateTime.Parse(reader.GetString("endtime")),
                            reader.GetInt32("access")
                         ));
                    }
                }
                conn.Close();
            }
            return scheduleDatas;
        }
        public List<ScheduleData> scheduleDatas(string owner)
        {
            List<ScheduleData> scheduleDatas = new List<ScheduleData>();
            string SQLqurey = $"SELECT * from schedule where owner = '{owner}';";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        scheduleDatas.Add(new ScheduleData(
                            reader.GetString("name"),
                            reader.GetInt32("owner"),
                            reader.GetString("contents"),
                            DateTime.Parse(reader.GetString("begintime")),
                            DateTime.Parse(reader.GetString("endtime")),
                            reader.GetInt32("access")
                         ));
                    }
                }
                conn.Close();
            }
            return scheduleDatas;
        }

        //public List<ScheduleData> scheduleDatas(string owner, string name)
        //{

        //}

        public bool signin(Userdata userdata)
        {
            bool ret;
            string SQLqurey = $"select Email from user where Email = '{userdata.Email}' AND " +
                $"password = '{convertPassword(userdata.Password)}' ;";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                using (var reader = command.ExecuteReader())
                {
                    ret = reader.Read();
                }
                conn.Close();
            }
            return ret;
        }

        /// <summary>
        /// 로그인 함수 로그인에 성공하면 true를 반환
        /// </summary>
        public string findData(string primary_key, string datatype)
        {
            string SQLqurey = $"select {datatype} from user where email = '{primary_key}'";
            string ret = null;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ret = reader.GetString(datatype);
                    }
                }
                conn.Close();
            }
            return ret;
        }
        //이메을을 가지고 특정을 찾는 함수
        private bool exist(string type, string name)
        {
            bool ret;
            string SQLqurey = $"select {type} from user where {type} = '{name}';";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                using (var reader = command.ExecuteReader())
                {
                    ret = reader.Read();
                }
                conn.Close();
            }
            return ret;
        }
        //특정 타입에 특정 이름을 가진 것이 테이블에 존재하는지 검사하는 함수
        private string convertPassword(string password)
        {
            var sha = new System.Security.Cryptography.HMACSHA512();
            sha.Key = System.Text.Encoding.UTF8.GetBytes(password.Length.ToString());
            var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return System.Convert.ToBase64String(hash);
        }
        //비밀번호를 넣으면 암호화된 비밀번호를 반환하는 함수
    }
}
