using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace KonyvtarAsztali
{
    internal class BookService
    {
        private static string DB_HOST = "localhost";
        private static string DB_USER = "root";
        private static string DB_PASSWORD = "";
        private static string DB_DBNAME = "books";

        private MySqlConnection connection;
        
        public BookService() 
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = DB_HOST;
            builder.UserID = DB_USER;
            builder.Password = DB_PASSWORD;
            builder.Database = DB_DBNAME;

            this.connection = new MySqlConnection(builder.ConnectionString);
            this.connection.Open();
        }

        public List<Book> GetBooks()
        {
            List<Book> list = new List<Book>();
            string sql = "SELECT * FROM books";
            MySqlCommand cmd = this.connection.CreateCommand();
            cmd.CommandText = sql;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    string title = reader.GetString("title");
                    string author = reader.GetString("author");
                    int publish_year = reader.GetInt32("publish_year");
                    int page_count = reader.GetInt32("page_count");
                    Book book = new Book(id, title, author, publish_year, page_count);
                    list.Add(book);
                }
            }

            return list;
        }

        //törlés az adatbázisból

        public bool DeleteBook(int id)
        {
            string sql = "DELETE FROM books WHERE id = @id";
            MySqlCommand command = this.connection.CreateCommand();
            command.CommandText = sql;
            command.Parameters.AddWithValue("@id", id);
            return command.ExecuteNonQuery() == 1;
        }
    }
}
