using BookStore.Models;
using BookStore.Models.NewFolder;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBaseRepository<Book> _booksRepository;
        private readonly IBaseRepository<Author> _authorsRepository;
        private readonly IBaseRepository<Publisher> _publishersRepository;
        public BookController(IBaseRepository<Book> booksRepository
            , IBaseRepository<Author> authorsRepository
            , IBaseRepository<Publisher> publishersRepository)
        {
            _booksRepository = booksRepository;
            _authorsRepository = authorsRepository;
            _publishersRepository = publishersRepository;
        }

        public IActionResult Create()
        {
            CreateBookViewModel createBookViewModel = new CreateBookViewModel();
            createBookViewModel.Authors = _authorsRepository.SelectAll().ToList();
            createBookViewModel.Publishers = _publishersRepository.SelectAll().ToList();

            return View("CreateBook", createBookViewModel);
        }
    }
}
