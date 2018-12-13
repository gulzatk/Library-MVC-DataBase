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
            Copy newCopy = Copy.FindByBookId(bookId);
            model.Add("book", newBook);
            model.Add("author", newAuthor);
            model.Add("copy", newCopy);

            return View(model);

        }
        [HttpPost("/books/{bookId}/show")]
        public ActionResult UpdateBook(int count, int available, int bookId, int year, string bookName)
        {
            Dictionary<string, object> modelUpdate = new Dictionary<string, object>{};
            Copy newCopy = Copy.FindByBookId(bookId);
            newCopy.Edit(bookId, count, available);
            Book newBook = Book.FindById(bookId);
            newBook.Edit(bookName, year, bookId);

            int authorId = newBook.GetAuthorId();
            Author newAuthor = Author.FindById(authorId);
            modelUpdate.Add("book", newBook);
            modelUpdate.Add("author", newAuthor);
            modelUpdate.Add("copy", newCopy);

            return View("ShowBook", modelUpdate);

        }

        [HttpGet("/books/{bookId}/checkout")]
        public ActionResult Checkout(int bookId)
        {
            Book newBook = Book.FindById(bookId);
            return View(newBook);
        }

        [HttpPost("/books/{bookId}/show/checkout")]
        public ActionResult CheckoutShow(int bookId, string name, string password)
        {
            Patron newPatron = Patron.FindByName(name, password);
            int patronId = newPatron.GetId();
            Book newBook = Book.FindById(bookId);
            newPatron.Chekout(newBook);
            return RedirectToAction("Show", patronId);
        }

        [HttpGet("/patron/show/{patronId}")]
        public ActionResult Show(int patronId)
        {
            Patron newPatron = Patron.FindById(patronId);
            List<Book> checkedBooks = newPatron.GetBooks();
            Dictionary<string, object> model = new Dictionary<string, object>{};
            model.Add("books", checkedBooks);
            model.Add("patron", newPatron);
            return View(model);
        }


        [HttpGet("/books/{bookId}/update")]
        public ActionResult Update(int bookId)
        {
            Dictionary<string,object> model = new Dictionary<string,object>{};
            Copy newCopy = Copy.FindByBookId(bookId);
            Book newBook = Book.FindById(bookId);
            int authorId = newBook.GetAuthorId();
            Author newAuthor = Author.FindById(authorId);
            model.Add("book",newBook);
            model.Add("author",newAuthor);
            model.Add("copy", newCopy);
            return View(model);
        }


    }
}
