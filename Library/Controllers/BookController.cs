using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Library.Models;

namespace Library.Controllers
{
    public class BookController : Controller
    {

        [HttpGet("/books")]
        public ActionResult Index()
        {
            List<Book> allBooks = Book.GetAll();
            return View(allBooks);
        }

        [HttpGet("/books/{authorId}/new")]
        public ActionResult New(int authorId)
        {   
            Author newAuthor = Author.FindById(authorId);
            return View(newAuthor);
        }

        [HttpGet("/books/{bookId}/show")]
        public ActionResult ShowBook(int bookId)
        {
            Dictionary<string, object> model = new Dictionary<string, object>{};
            Book newBook = Book.FindById(bookId);
            int authorId = newBook.GetAuthorId();
            Author newAuthor = Author.FindById(authorId);
            Copy newCopy = new Copy(bookId, 0, 0);
            newCopy.Save();
            model.Add("book", newBook);
            model.Add("author", newAuthor);
            model.Add("copy", newCopy);

            return View(model);

        }
        [HttpPost("/books/{bookId}/show")]
        public ActionResult UpdateBook(int count, int bookId)
        {
            Copy newCopy = new Copy(bookId, count, count);
            newCopy.Save();
            Dictionary<string, object> modelUpdate = new Dictionary<string, object>{};
            Book newBook = Book.FindById(bookId);
            int authorId = newBook.GetAuthorId();
            Author newAuthor = Author.FindById(authorId);

            modelUpdate.Add("book", newBook);
            modelUpdate.Add("author", newAuthor);
            modelUpdate.Add("copy", newCopy);

            return View("ShowBook", modelUpdate);

        }
    }
}
