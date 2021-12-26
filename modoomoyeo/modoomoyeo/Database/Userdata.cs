using MySql.Data.MySqlClient;

namespace modoomoyeo.Database
{
    public class Userdata
    {
        public Userdata(string email, string password, string name)
        {
            Email = email;
            Password = password;
            Name = name;
        }
        public Userdata(int ID, string email, string password, string name)
        {
            id = ID;
            Email = email;
            Password = password;
            Name = name;
        }
//        private DBConnnection? context;
        public int id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
    }
}
