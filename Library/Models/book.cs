using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Library;

namespace Library.Models
{
    public class Book
    {
        private int _id;
        private string _name;
        private int _author_id;
        private int _year;

        public Book(string name, int author_id, int year, int id=0)
        {
            _name = name;
            _author_id = author_id;
            _year = year;
            _id = id;
        }

        public string GetName()
        {
            return _name;
        }

        public int GetId()
        {
            return _id;
        }

        public int GetAuthorId()
        {
            return _author_id;
        }

        public int GetYear()
        {
            return _year;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO books (name, author_id, year) VALUES (@name, @author_id, @year);";
            cmd.Parameters.AddWithValue("@name", this._name);
            cmd.Parameters.AddWithValue("@author_id", this._author_id);
            cmd.Parameters.AddWithValue("@year", this._year);
            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Book> GetAll()
        {
            List<Book> allBooks = new List<Book>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM books;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int bookId = rdr.GetInt32(0);
                string bookName = rdr.GetString(1);
                int author_id = rdr.GetInt32(2);
                int bookYear = rdr.GetInt32(3);
                Book newBook = new Book(bookName, author_id, bookYear, bookId);
                allBooks.Add(newBook);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allBooks;
        }

        public static List<Book> FindByAuthor(int authorId)
        {
            List<Book> allBooks = new List<Book>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM books WHERE author_id = @author_id;";
            cmd.Parameters.AddWithValue("@author_id", authorId);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                int author_id = rdr.GetInt32(2);
                int year = rdr.GetInt32(3);
                Book newBook = new Book(name, author_id, year, id);
                allBooks.Add(newBook);
            }
            
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allBooks;
        }
        public static Book FindById(int bookId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM books WHERE id = @bookId;";
            cmd.Parameters.AddWithValue("@bookId", bookId);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            int id = 0;
            string name = "";
            int author_id = 0;
            int year = 0;
            while(rdr.Read())
            {
                id = rdr.GetInt32(0);
                name = rdr.GetString(1);
                author_id = rdr.GetInt32(2);
                year = rdr.GetInt32(3);
            }
            Book newBook = new Book(name, author_id, year, id);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newBook;
        }

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM books;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
        }

        public override bool Equals(System.Object otherBook)
        {
            if (!(otherBook is Book))
            {
                return false;
            }
            else
            {
                Book newBook = (Book) otherBook;
                bool areIdsEqual = (this.GetId() == newBook.GetId());
                bool areNameEqual = (this.GetName() == newBook.GetName());
                bool areauthor_idEqual = (this.GetAuthorId() == newBook.GetAuthorId());
                bool areYearEqual = (this.GetYear() == newBook.GetYear());
                return (areIdsEqual && areNameEqual && areauthor_idEqual && areYearEqual);
            }
        }
        public override int GetHashCode()
        {
            return this.GetName().GetHashCode();
        }

    }
}
