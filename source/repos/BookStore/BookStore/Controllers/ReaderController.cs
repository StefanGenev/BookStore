using BookStore.Models;
using BookStore.Models.ViewModels;
using BookStore.Repository;
using BookStore.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Controllers
{
    public class ReaderController : Controller
    {
        private readonly IBaseRepository<Reader> _readersRepository;

        private IWebHostEnvironment _hostingEnvironment { get; }
        public ReaderController(IBaseRepository<Reader> readersRepository
            , IWebHostEnvironment hostingEnvironment)
        {
            _readersRepository = readersRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            List<Reader> readers = _readersRepository.GetTable().ToList();
            return View("ViewReaders", readers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ReaderViewModel model = new ReaderViewModel();
            return View("CreateReader", model);
        }

        [HttpPost]
        public IActionResult Create(ReaderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = new ReaderViewModel();
                return View("CreateReader", model);
            }

            string imageFileName;
            if (model.Image != null)
            {
                imageFileName = FileUpload.ProcessUploadedFile(model.Image, _hostingEnvironment, "images");
            }
            else
            {
                imageFileName = "no-avatar.jpg";
            }

            Reader reader = new Reader
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Address = model.Address,
                Gender = model.Gender,
                ImagePath = imageFileName
            };

            _readersRepository.Insert(reader);

            return RedirectToAction("View", new { id = reader.Id });
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult View(int id)
        {
            Reader reader = _readersRepository.SelectById(id);

            if (reader == null)
            {
                Response.StatusCode = 404;
                return RedirectToAction("index", "Reader");
            }

            return View("ViewReader", reader);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Edit(int id)
        {
            ReaderViewModel readerViewModel = new ReaderViewModel();
            Reader reader = _readersRepository.SelectById(id);

            readerViewModel.FirstName = reader.FirstName;
            readerViewModel.MiddleName = reader.MiddleName;
            readerViewModel.LastName = reader.LastName;
            readerViewModel.Address = reader.Address;
            readerViewModel.Gender = reader.Gender;

            ViewBag.BookId = reader.Id;
            ViewBag.ImagePath = reader.ImagePath;

            return View("EditReader", readerViewModel);
        }

        [HttpPost]
        public ActionResult Edit(ReaderViewModel model, int id)
        {
            Reader reader = _readersRepository.SelectById(id);
            if (!ModelState.IsValid)
            {
                model = new ReaderViewModel();

                model.FirstName = reader.FirstName;
                model.MiddleName = reader.MiddleName;
                model.LastName = reader.LastName;
                model.Address = reader.Address;
                model.Gender = reader.Gender;

                ViewBag.BookId = id;
                ViewBag.ImagePath = reader.ImagePath;

                return View("EditReader", model);
            }

            if (model.Image != null)
            {
                reader.ImagePath = FileUpload.ProcessUploadedFile(model.Image, _hostingEnvironment, "images");
            }
            else if (reader.ImagePath.Length <= 0)
            {
                reader.ImagePath = "no-avatar.jpg";
            }

            reader.FirstName = model.FirstName;
            reader.MiddleName = model.MiddleName;
            reader.LastName = model.LastName;
            reader.Address = model.Address;
            reader.Gender = model.Gender;

            _readersRepository.Update(reader);

            return RedirectToAction("View", new { id = reader.Id });
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            _readersRepository.Delete(id);
            return RedirectToAction("Index", "Reader");
        }

    }
}
