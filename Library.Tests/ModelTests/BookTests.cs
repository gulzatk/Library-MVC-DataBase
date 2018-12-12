using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Models;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Tests
{
    [TestClass]
    public class BookTest: IDisposable
    {
        public void Dispose()
        {
            Book.ClearAll();
        }

        public BookTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
        }

        [TestMethod]
        public void Create_TypeOfBook_Book()
        {
            Book newBook = new Book("HP", 1, 2000);
            Assert.AreEqual(newBook.GetType(), typeof(Book));
        }

        [TestMethod]
        public void GetName_ReturnName_Name()
        {
            string name = "Harry Potter";
            Book newBook = new Book(name, 1, 2000);

            string resultName = newBook.GetName();

            Assert.AreEqual(name, resultName);
        }

        [TestMethod]
        public void GetAll_ReturnListOfBooks_List()
        {
            Book newBookOne = new Book("HP", 1, 2000);
            Book newBookTwo = new Book("Programming for Dummies", 1, 2000);
            newBookOne.Save();
            newBookTwo.Save();

            List<Book> allBooks = Book.GetAll();
            List<Book> resultList = new List<Book>{newBookOne, newBookTwo};

            CollectionAssert.AreEqual(allBooks, resultList);
        }

        [TestMethod]
        public void FindById_ReturnBookById_Book()
        {
            Book newBookTwo = new Book("Programming for Dummies", 1, 2000);
            Book newBookOne = new Book("HP", 1, 2000);
            newBookOne.Save();
            newBookTwo.Save();
            
            int bookId = newBookTwo.GetId();
            Book resultBook = Book.FindById(bookId);

            Assert.AreEqual(newBookTwo, resultBook);
        }
    }
}


