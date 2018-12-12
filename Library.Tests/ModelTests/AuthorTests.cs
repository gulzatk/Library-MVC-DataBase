using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Models;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Tests
{
    [TestClass]
    public class AuthorTest: IDisposable
    {
        public void Dispose()
        {
            Author.ClearAll();
        }

        public AuthorTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
        }

        [TestMethod]
        public void Create_TypeOfAuthor_Author()
        {
            Author newAuthor = new Author("JK Rowling");
            Assert.AreEqual(newAuthor.GetType(), typeof(Author));
        }

        [TestMethod]
        public void GetName_ReturnName_Name()
        {
            string name = "JK";
            Author newAuthor = new Author(name);

            string resultName = newAuthor.GetName();

            Assert.AreEqual(name, resultName);
        }

        [TestMethod]
        public void GetAll_ReturnListOfAuthors_List()
        {
            string nameOne = "JK";
            string nameTwo = "Bob";
            Author newAuthorOne = new Author(nameOne);
            Author newAuthorTwo = new Author(nameTwo);
            newAuthorOne.Save();
            newAuthorTwo.Save();

            List<Author> allAuthors = Author.GetAll();
            List<Author> resultList = new List<Author>{newAuthorOne, newAuthorTwo};

            CollectionAssert.AreEqual(allAuthors, resultList);
        }
    }
}


