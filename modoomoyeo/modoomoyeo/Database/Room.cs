namespace modoomoyeo.Database
{
    public class Room
    {
        public Room(int _permission, string _name)
        {
            permission = _permission;
            name = _name;
        }
        public int permission { get; set; }
        public string name { get; set; }
    }
}
