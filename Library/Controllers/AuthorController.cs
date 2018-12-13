using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Library.Models;

namespace Library.Controllers
{
    public class AuthorController : Controller
    {
        [HttpGet("/authors")]
        public ActionResult Index()
        {
            List<Author> allAuthors = Author.GetAll();
            return View(allAuthors);
        }

        [HttpGet("/authors/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/authors")]
        public ActionResult Show(string authorName)
        {
            Author newAuthor = new Author(authorName);
            newAuthor.Save();
            return RedirectToAction("Index");
        } 

        [HttpGet("/authors/{authorId}/show")]
        public ActionResult ShowAuthor(int authorId)
        {
            Dictionary<string,object> model = new Dictionary<string,object>{};
            Author newAuthor = Author.FindById(authorId);
            List<Book> authorBooks = Book.FindByAuthor(authorId);
            model.Add("author", newAuthor);
            model.Add("books", authorBooks);

            return View(model);
        }

        [HttpPost("/authors/{authorId}/show")]
        public ActionResult CreateBook(int authorId, string bookName, int year)
        {
            Book newBook = new Book(bookName, authorId, year);
            newBook.Save();
            int bookId = newBook.GetId();
            Copy newCopy = new Copy(bookId, 1, 1);
            newCopy.Save();

            return RedirectToAction("ShowAuthor", authorId);
        }
    }
}
