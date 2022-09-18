using BookStore.Models;
using BookStore.Models.ViewModels;
using BookStore.Repository;
using BookStore.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBaseRepository<Book> _booksRepository;
        private readonly IBaseRepository<Author> _authorsRepository;
        private readonly IBaseRepository<Publisher> _publishersRepository;
        private readonly IBaseRepository<Order> _ordersRepository;

        private IWebHostEnvironment _hostingEnvironment { get; }
        public BookController(IBaseRepository<Book> booksRepository
            , IBaseRepository<Author> authorsRepository
            , IBaseRepository<Publisher> publishersRepository
            , IBaseRepository<Order> ordersRepository
            , IWebHostEnvironment hostingEnvironment)
        {
            _booksRepository = booksRepository;
            _authorsRepository = authorsRepository;
            _publishersRepository = publishersRepository;
            _ordersRepository = ordersRepository;
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
                model = new BookViewModel();
                model.Authors = _authorsRepository.SelectAll().ToList();
                model.Publishers = _publishersRepository.SelectAll().ToList();

                return View("CreateBook", model);
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
                CopiesAvailable = model.CopiesAvailable,
                YearOfPublishing = model.YearOfPublishing,
                ImagePath = imageFileName
            };

            _booksRepository.Insert(book);

            return RedirectToAction("View", new { id = book.Id });
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            Book book = _booksRepository.SelectById(id);
            book.Author = _authorsRepository.SelectById(book.AuthorId);
            book.Publisher = _publishersRepository.SelectById(book.PublisherId);

            bool IsTakenByUser = _ordersRepository.GetTable().Any(order => order.ReaderId == User.FindFirstValue(ClaimTypes.NameIdentifier)
                                                                             && order.BookId == id
                                                                             && order.IsBookReturned == false);
            ViewBag.IsTakenByUser = IsTakenByUser;

            if (book == null)
            {
                Response.StatusCode = 404;
                return RedirectToAction("index", "home");
            }

            return View("ViewBook", book);
        }

        [HttpGet]
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
            bookViewModel.CopiesAvailable = book.CopiesAvailable;
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
                model = new BookViewModel();

                model.Title = book.Title;
                model.Description = book.Description;
                model.AuthorId = book.AuthorId;
                model.Authors = _authorsRepository.SelectAll().ToList();
                model.Price = book.Price;
                model.PublisherId = book.PublisherId;
                model.Publishers = _publishersRepository.SelectAll().ToList();
                model.CopiesAvailable = book.CopiesAvailable;
                model.YearOfPublishing = book.YearOfPublishing;

                ViewBag.BookId = book.Id;
                ViewBag.ImagePath = book.ImagePath;

                return View("EditBook", model);
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
            book.CopiesAvailable = model.CopiesAvailable;
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

        public ActionResult Return(int id)
        {
            Order order = _ordersRepository.GetTable().Where(order => order.ReaderId == User.FindFirstValue(ClaimTypes.NameIdentifier)
                                                                   && order.BookId == id)
                                                            .First();
            if (order != null)
            {
                order.IsBookReturned = true;
                _ordersRepository.Update(order);
            }

            Book book = _booksRepository.SelectById(id);
            if (book != null)
            {
                book.Author = _authorsRepository.SelectById(book.AuthorId);
                book.Publisher = _publishersRepository.SelectById(book.PublisherId);
                book.CopiesAvailable++;

                _booksRepository.Update(book);
            }

            ViewBag.IsTakenByUser = false;

            return View("ViewBook", book);
        }

        public ActionResult Take(int id)
        {
            Book book = _booksRepository.SelectById(id);

            if (book != null)
            {
                book.Author = _authorsRepository.SelectById(book.AuthorId);
                book.Publisher = _publishersRepository.SelectById(book.PublisherId);

                if (book.CopiesAvailable > 0)
                {
                    book.CopiesAvailable--;

                    Order order = new Order()
                    {
                        BookId = id,
                        ReaderId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                        DateOfOrder = DateTime.Now,
                        IsBookReturned = false
                    };

                    _ordersRepository.Insert(order);
                    _booksRepository.Update(book);
                }
            }

            ViewBag.IsTakenByUser = true;

            return View("ViewBook", book);
        }

        public ActionResult TakenBooks()
        {
            List<Order> orders = _ordersRepository.GetTable().Where(order => order.IsBookReturned == false)
                                                        .Include(order => order.Book)
                                                        .Include(order => order.Book.Author)
                                                        .Include(order => order.Book.Publisher)
                                                        .Include(order => order.Reader)
                                                        .OrderBy(order => order.ReaderId)
                                                        .ToList();
            return View("TakenBooks", orders);
        }
    }
}
