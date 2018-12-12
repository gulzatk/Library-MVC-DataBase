using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Library;

namespace Library.Models
{
    public class Copy
    {
        private int _id;
        private int _book_id;
        private int _count;
        private int _available;

        public Copy(int book_id, int count, int available, int id=0)
        {
            _book_id = book_id;
            _count = count;
            _available = available;
            _id = id;
        }

        public int GetId()
        {
            return _id;
        }

        public int GetCount()
        {
            return _count;
        }

        public void SetCount( int newCount)
        {
            _count = newCount;
        }

        public int GetAvailable()
        {
            return _available;
        }

        public int GetBookId()
        {
            return _book_id;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO copies (book_id, count, available) VALUES (@book_id, @count, @available);";
            cmd.Parameters.AddWithValue("@book_id", this._book_id);
            cmd.Parameters.AddWithValue("@count", this._count);
            cmd.Parameters.AddWithValue("@available", this._available);
            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Copy> GetAll()
        {
            List<Copy> allCopies = new List<Copy>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM copies;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int copyId = rdr.GetInt32(0);
                int book_id = rdr.GetInt32(1);
                int count = rdr.GetInt32(2);
                int available= rdr.GetInt32(3);
                Copy newCopy = new Copy(book_id, count, available, copyId);
                allCopies.Add(newCopy);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCopies;
        }

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM copies;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
        }

        public void Edit(int bookId, int newCount, int newAvailable)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE copies SET (count, available) VALUES (@newCount, @newAvailable) WHERE book_id = @bookId;";
            cmd.Parameters.AddWithValue("@bookId", bookId);
            cmd.Parameters.AddWithValue("@newAvailable", newAvailable);
            cmd.Parameters.AddWithValue("@newCount", newCount);
            cmd.ExecuteNonQuery();
            _book_id = bookId;
            _count = newCount;
            _available = newAvailable;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public override bool Equals(System.Object otherCopy)
        {
            if (!(otherCopy is Copy))
            {
                return false;
            }
            else
            {
                Copy newCopy = (Copy) otherCopy;
                bool areIdsEqual = (this.GetId() == newCopy.GetId());
                bool areBook_idEqual = (this.GetBookId() == newCopy.GetBookId());
                bool areCountEqual = (this.GetCount() == newCopy.GetCount());
                bool areAvailableEqual = (this.GetAvailable() == newCopy.GetAvailable());
                return (areIdsEqual && areBook_idEqual && areCountEqual && areAvailableEqual);
            }
        }
    }
}
