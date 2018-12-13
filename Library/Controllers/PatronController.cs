using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Library.Models;

namespace Library.Controllers
{
    public class PatronController : Controller
    {
        [HttpGet("/patron")]
        public ActionResult Index()
        {
            return View();
        }

         [HttpGet("/patron/new")]
        public ActionResult New()
        {
            return View();
        }

         [HttpPost("/patron/create")]
        public ActionResult Create(string name, string password)
        {
            Patron newPatron = new Patron(name, password);
            newPatron.Save();
            return View(newPatron);
        } 

        [HttpGet("/patron/login")]
        public ActionResult Login()
        {
            return View();
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

        [HttpGet("/patron/show/{patronId}/return/{bookId}")]
        public ActionResult Return(int bookId, int patronId)
        {
            Patron newPatron = Patron.FindById(patronId);
            Book newBook = Book.FindById(bookId);
            newPatron.Return(newBook);
            return RedirectToAction("Show", patronId);
        }

        [HttpGet("/patron/loginmessage")]
        public ActionResult LoginMessage(string name, string password)
        {
            Patron newPatron = Patron.FindByName(name, password);
            return View(newPatron);
        }
    }
}