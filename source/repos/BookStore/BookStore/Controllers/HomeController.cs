using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBaseRepository<Book> _booksRepository;

        public HomeController(IBaseRepository<Book> booksRepo)
        {
            _booksRepository = booksRepo;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            List<Book> books = _booksRepository.GetTable()
                .Include(book => book.Author)
                .Include(book => book.Publisher)
                .ToList();

            return View(books);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
