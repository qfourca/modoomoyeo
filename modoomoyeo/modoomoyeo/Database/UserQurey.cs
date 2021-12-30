using MySql.Data.MySqlClient;

namespace modoomoyeo.Database
{
    public class UserQurey : DBConnnection
    {
        public UserQurey(string connectionString) : base(connectionString)
        {
        }

        public List<Userdata> GetData()
        {
            List<Userdata> List = new List<Userdata>();
            String SQLqurey = "SELECT * from User";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        List.Add(new Userdata(
                            reader.GetString("Email"),
                            reader.GetString("name"),
                            reader.GetString("password")
                        ));
                    }
                }
                conn.Close();
                return List;
            }
        }


        public string signup(Userdata userdata)
        {
            if (exist("Email", userdata.Email))
            {
                return "Email Already Exist";
            }
            else
            {
                string test = "{" + '"' + "name" + '"' + " : " + '"' + "test" + '"' + '}';
                Console.Write(test);
                string SQLqurey = $"insert into user (Email, password, name, info)values(" +
                    $"'{userdata.Email}','{convertPassword(userdata.Password)}','{userdata.Name}', '{test}')";
                using (MySqlConnection conn = GetConnection())
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                        Console.WriteLine("Sign up " + (command.ExecuteNonQuery() == 1 ?
                            "success" : "fail"));
                    }
                    catch(Exception exception)
                    {
                        Console.WriteLine("DB connecttion Fail");
                        Console.WriteLine(exception.ToString());
                    }
                    conn.Close();
                    return "OK";
                }
            }
        }
        //회원가입 함수 회원가입에 성공하면 문자열 OK를 반환
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
        //로그인 함수 로그인에 성공하면 true를 반환
        public string findData(string email, string datatype)
        {
            string SQLqurey = $"select {datatype} from user where email = '{email}'";
            string ret = null;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                using (var reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        ret = reader.GetString(datatype);
                    }
                }
                conn.Close();
            }
            return ret;
        }
        public string idToName(int id)
        {
            string SQLqurey = $"select name from user where id = '{id}'";
            string ret = null;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ret = reader.GetString("name");
                    }
                }
                conn.Close();
            }
            return ret;
        }
        public int nameToId(string name)
        {
            string SQLqurey = $"select id from user where name = '{name}'";
            int ret = 0;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ret = reader.GetInt32("id");
                    }
                }
                conn.Close();
            }
            return ret;
        }
        //이메일을 가지고 특정을 찾는 함수
        public List<Userdata> findUserAll()
        {
            List<Userdata> ret = new List<Userdata>();
            string SQLqurey = "select id, name from user;";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.Add(new Userdata(
                            reader.GetInt32("id"),
                            null,
                            null,
                            reader.GetString("name")));
                    }
                conn.Close();
                }
            }
            return ret;
        }
        //모든 유저를 반환
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

        public List<MentorData> findMentors()
        {
            List<MentorData> ret = new List<MentorData>();
            string SQLqurey = $"select * from Mentor;";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                using (var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        int id = reader.GetInt32("id");
                        ret.Add(new MentorData(
                                            id,
                                            idToName(id),
                                            reader.GetString("tech"),
                                            reader.GetString("phone"),
                                            reader.GetString("github")));
                    }
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

/*
try
{
    conn.Open();
    MySqlCommand command = new MySqlCommand(SQLqurey, conn);
    if(command.ExecuteNonQuery() == 1)
}
*/