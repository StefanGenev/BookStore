using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IBaseRepository<Author> _authorsRepository;

        public AuthorController(IBaseRepository<Author> authorsRepository)
        {
            _authorsRepository = authorsRepository;
        }

        public IActionResult Index()
        {
            List<Author> authors = _authorsRepository.GetTable().ToList();
            return View("ViewAuthors", authors);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Author author = new Author();
            return View("Author", author);
        }

        [HttpPost]
        public IActionResult Create(Author author)
        {
            if (!ModelState.IsValid)
                return View("Author", author);

            _authorsRepository.Insert(author);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Author record = _authorsRepository.SelectById(id);
            ViewBag.RecordId = id;

            return View("Author", record);
        }

        [HttpPost]
        public IActionResult Edit(Author record, int id)
        {
            if (!ModelState.IsValid)
                return View("Author", record);

            _authorsRepository.Update(record);

            return RedirectToAction("Index");
        }


        public ActionResult Delete(int id)
        {
            _authorsRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
