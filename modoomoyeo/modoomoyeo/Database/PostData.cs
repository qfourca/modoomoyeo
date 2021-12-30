namespace modoomoyeo.Database
{
    using MySql.Data.MySqlClient;
    public class PostData
    {
        public PostData(int _id, string? _name,string _title, int _ownerid, string _contents, DateTime _time, int _access)
        {
            id = _id;
            name = _name;
            title = _title;
            ownerid = _ownerid;
            contents = _contents;
            time = _time;
            access =  _access;
        }


        public int id { get; set; }
        public string? name { get; set; }
        public string? title { get; set; }
        public int ownerid { get; set; }
        public string? contents { get; set; }
        public DateTime time { get; set; }
        public int access { get; set; }
    }
}
