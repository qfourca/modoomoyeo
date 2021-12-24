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
        private DBConnnection? context;
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
    }
}
