using MySql.Data.MySqlClient;

namespace modoomoyeo.Database
{
    public class MentorData
    {
        public MentorData(int _id, string _name, string _tech, string _phone, string _github)
        {
            id = _id;
            name = _name;
            tech = _tech;
            phone = _phone;
            github = _github;
        }
        //        private DBConnnection? context;
        public int id { get; set; }
        public string? name { get; set; }
        public string? tech { get; set; }
        public string? phone { get; set; }
        public string? github { get; set; }
    }
}
