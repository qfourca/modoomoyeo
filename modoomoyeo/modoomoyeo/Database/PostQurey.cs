namespace modoomoyeo.Database
{
    using MySql.Data.MySqlClient;


    public class PostQurey : DBConnnection
    {
        public PostQurey(string connectionString) : base(connectionString)
        {
        }

        public int insertPost(PostData post)
        {
            string SQLqurey = $"insert into posts (title, ownerid, contents, time, access)" +
                $" values('{post.title}', {post.ownerid}, '{post.contents}'," +
                $"'{post.time:yyyy/MM/dd HH/mm/ss}', {post.access});";
            Console.WriteLine(SQLqurey);
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
            }
            return findPostId(post.title, post.ownerid);

        }

        public List<PostData> findPosts(int access)
        {
            List<PostData> postDatas = new List<PostData>();
            string SQLqurey = $"select * from posts where access = {access};";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        postDatas.Add(new PostData(
                            reader.GetInt32("id"),
                            null,
                            reader.GetString("title"),
                            reader.GetInt32("ownerid"),
                            reader.GetString("contents"),
                            DateTime.Parse(reader.GetString("time")),
                            reader.GetInt32("access")
                            ));
                    }
                }
                conn.Close();
            }
            return postDatas;
        }


        public int findPostId(string title, int ownerid)
        {
            int ret = 0;
            string SQLqurey = $"select id from posts where title = '{title}' AND ownerid = {ownerid};";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(SQLqurey, conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret = reader.GetInt32("id");
                    }
                }
                conn.Close();
            }
            return ret;
        }

    }
    


}
