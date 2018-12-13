using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Library;

namespace Library.Models
{
    public class Patron
    {
        private int _id;
        private string _name;
        private string _password;

        public Patron (string name, string password, int id=0)
        {
            _name = name;
            _password = password;
            _id = id;
        }

//Getters
        public string GetName()
        {
            return _name;
        }

        public string GetPassword()
        {
            return _password;
        }

        public int GetId()
        {
            return _id;
        }

        public void SetId(int newId)
        {
            _id = newId;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO patrons (name, password) VALUES (@name, @password);";
            cmd.Parameters.AddWithValue("@name", this._name);
            cmd.Parameters.AddWithValue("@password", this._password);
            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Patron> GetAll()
        {
            List<Patron> allPatrons = new List<Patron>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM patrons;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int patronId = rdr.GetInt32(0);
                string patronName = rdr.GetString(1);
                string password = rdr.GetString(2);
                Patron newPatron = new Patron(patronName, password);
                newPatron.SetId(patronId);
                allPatrons.Add(newPatron);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allPatrons;
        }
        public void Chekout(Book newBook)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO patrons_books (patron_id, book_id) VALUE (@patronId, @bookId);";
            cmd.Parameters.AddWithValue("@patronId", this._id);
            cmd.Parameters.AddWithValue("@bookId", newBook.GetId());
            cmd.ExecuteNonQuery();
            
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public void Return(Book newBook)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM patrons_books WHERE book_id = @bookId AND patron_id = @patronId;";
            cmd.Parameters.AddWithValue("@patronId", this._id);
            cmd.Parameters.AddWithValue("@bookId", newBook.GetId());
            cmd.ExecuteNonQuery();
            
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Book> GetBooks()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT books.* FROM patrons
                JOIN patrons_books ON (patrons.id = patrons_books.patron_id)
                JOIN books ON (patrons_books.book_id = books.id)
                WHERE patrons.id = @PatronId;";

            cmd.Parameters.AddWithValue("@PatronId", this._id);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Book> books = new List<Book>{};
            while(rdr.Read())
            {
            int bookId = rdr.GetInt32(0);

            Book newBook = Book.FindById(bookId);
            books.Add(newBook);
            }
            conn.Close();
            if (conn != null)
            {
            conn.Dispose();
            }
            return books;
        }

        public static Patron FindById(int patronId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM patrons WHERE id = @patronId;";
            cmd.Parameters.AddWithValue("@patronId", patronId);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            int id = 0;
            string name = "";
            string password = "";
            while(rdr.Read())
            {
                id = rdr.GetInt32(0);
                name = rdr.GetString(1);
                password = rdr.GetString(2);
            }
            Patron newPatron = new Patron(name, password, id);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newPatron;
        }

        public static Patron FindByName(string patronName, string inputPassword)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM patrons WHERE name = @name AND password = @inputPassword;";
            cmd.Parameters.AddWithValue("@name", patronName);
            cmd.Parameters.AddWithValue("@inputPassword", inputPassword);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            int id = 0;
            string name = "none";
            string password = "none";
            while(rdr.Read())
            {
                id = rdr.GetInt32(0);
                name = patronName;
                password = rdr.GetString(2);
            }
            Patron newPatron = new Patron(name, password, id);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newPatron;
        }

        

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM patrons;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
        }

        public override bool Equals(System.Object otherPatron)
        {
            if (!(otherPatron is Patron))
            {
                return false;
            }
            else
            {
                Patron newPatron = (Patron) otherPatron;
                bool areIdsEqual = (this.GetId() == newPatron.GetId());
                bool areNameEqual = (this.GetName() == newPatron.GetName());
                bool arePasswordEqual = (this.GetPassword() == newPatron.GetPassword());
                return (areIdsEqual && areNameEqual && arePasswordEqual);
            }
        }
        public override int GetHashCode()
        {
            return this.GetName().GetHashCode();
        }


    }
}
