using MySql.Data.MySqlClient;

namespace modoomoyeo.Database
{
    public class Chatinglog
    {
        public Chatinglog(int Ownerid, string Contents, int Access_code)
        {
            ownerid = Ownerid;
            contents = Contents;
            time = System.DateTime.Now;
            access_code = Access_code;
        }
        public Chatinglog(int Ownerid, DateTime Time, string Contents, int Access_code)
        {
            ownerid = Ownerid;
            time = Time;
            contents = Contents;
            access_code = Access_code;
        }
        private DBConnnection? context;
        public int ownerid { get; set; }
        public DateTime? time { get; set; }
        public string? contents { get; set; }
        public int access_code { get; set; }
    }
}