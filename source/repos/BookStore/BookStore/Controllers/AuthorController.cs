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
            return View("CreateAuthor", author);
        }

        [HttpPost]
        public IActionResult Create(Author author)
        {
            if (!ModelState.IsValid)
                return View("CreateAuthor", author);

            _authorsRepository.Insert(author);

            return RedirectToAction("Index");
        }
    }
}
