using BookStore.Models;
using BookStore.Models.ViewModels;
using BookStore.Repository;
using BookStore.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBaseRepository<Book> _booksRepository;
        private readonly IBaseRepository<Author> _authorsRepository;
        private readonly IBaseRepository<Publisher> _publishersRepository;

        private IWebHostEnvironment _hostingEnvironment { get; }
        public BookController(IBaseRepository<Book> booksRepository
            , IBaseRepository<Author> authorsRepository
            , IBaseRepository<Publisher> publishersRepository
            , IWebHostEnvironment hostingEnvironment)
        {
            _booksRepository = booksRepository;
            _authorsRepository = authorsRepository;
            _publishersRepository = publishersRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Create()
        {
            BookViewModel bookViewModel = new BookViewModel();
            bookViewModel.Authors = _authorsRepository.SelectAll().ToList();
            bookViewModel.Publishers = _publishersRepository.SelectAll().ToList();

            return View("CreateBook", bookViewModel);
        }

        [HttpPost]
        public IActionResult Create(BookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                BookViewModel bookViewModel = new BookViewModel();
                bookViewModel.Authors = _authorsRepository.SelectAll().ToList();
                bookViewModel.Publishers = _publishersRepository.SelectAll().ToList();

                return View("CreateBook", bookViewModel);
            }

            string imageFileName;
            if (model.Image != null)
            {
                imageFileName = FileUpload.ProcessUploadedFile(model.Image, _hostingEnvironment, "images");
            }
            else
            {
                imageFileName = "no-image.jpg";
            }

            Book book = new Book
            {
                Title = model.Title,
                Description = model.Description,
                AuthorId = model.AuthorId,
                Price = model.Price,
                PublisherId = model.PublisherId,
                YearOfPublishing = model.YearOfPublishing,
                ImagePath = imageFileName
            };

            _booksRepository.Insert(book);

            return RedirectToAction("View", new { id = book.Id });
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult View(int id)
        {
            Book book = _booksRepository.SelectById(id);
            book.Author = _authorsRepository.SelectById(book.AuthorId);
            book.Publisher = _publishersRepository.SelectById(book.PublisherId);

            if (book == null)
            {
                Response.StatusCode = 404;
                return RedirectToAction("index", "home");
            }

            return View("ViewBook", book);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Edit(int id)
        {
            BookViewModel bookViewModel = new BookViewModel();
            Book book = _booksRepository.SelectById(id);

            bookViewModel.Title = book.Title;
            bookViewModel.Description = book.Description;
            bookViewModel.AuthorId = book.AuthorId;
            bookViewModel.Authors = _authorsRepository.SelectAll().ToList();
            bookViewModel.Price = book.Price;
            bookViewModel.PublisherId = book.PublisherId;
            bookViewModel.Publishers = _publishersRepository.SelectAll().ToList();
            bookViewModel.YearOfPublishing = book.YearOfPublishing;

            ViewBag.BookId = book.Id;
            ViewBag.ImagePath = book.ImagePath;

            return View("EditBook", bookViewModel);
        }

        [HttpPost]
        public ActionResult Edit(BookViewModel model, int id)
        {
            Book book = _booksRepository.SelectById(id);
            if (!ModelState.IsValid)
            {
                BookViewModel bookViewModel = new BookViewModel();

                bookViewModel.Title = book.Title;
                bookViewModel.Description = book.Description;
                bookViewModel.AuthorId = book.AuthorId;
                bookViewModel.Authors = _authorsRepository.SelectAll().ToList();
                bookViewModel.Price = book.Price;
                bookViewModel.PublisherId = book.PublisherId;
                bookViewModel.Publishers = _publishersRepository.SelectAll().ToList();
                bookViewModel.YearOfPublishing = book.YearOfPublishing;

                ViewBag.BookId = book.Id;
                ViewBag.ImagePath = book.ImagePath;

                return View("EditBook", bookViewModel);
            }

            if (model.Image != null)
            {
                book.ImagePath = FileUpload.ProcessUploadedFile(model.Image, _hostingEnvironment, "images");
            }
            else if (book.ImagePath.Length <= 0)
            {
                book.ImagePath = "no-image.jpg";
            }

            book.Title = model.Title;
            book.Description = model.Description;
            book.AuthorId = model.AuthorId;
            book.Price = model.Price;
            book.PublisherId = model.PublisherId;
            book.YearOfPublishing = model.YearOfPublishing;

            _booksRepository.Update(book);

            return RedirectToAction("View", new { id = book.Id });
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            _booksRepository.Delete(id);
            return RedirectToAction("index", "home");
        }
    }
}
