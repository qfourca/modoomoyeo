using MySql.Data.MySqlClient;

namespace modoomoyeo.Database
{
    public class ScheduleData
    {
        public ScheduleData(string Name, string Owner, string Contents, DateTime End, int Access)
        {
            name = Name;
            owner = Owner;
            contents = Contents;
            begin = DateTime.Now;
            end = End;
            access = Access;
        }
        public ScheduleData(string Name, string Owner, string Contents, DateTime Begin, DateTime End, int Access)
        {
            name = Name;
            owner = Owner;
            contents = Contents;
            begin = Begin;
            end = End;
            access = Access;
        }
        private DBConnnection? context;
        public string? name { get; set; }
        public string? owner { get; set; }
        public string? contents { get; set; }
        public DateTime begin { get; set; }
        public DateTime end { get; set; }
        public int access { get; set; }
    }
}