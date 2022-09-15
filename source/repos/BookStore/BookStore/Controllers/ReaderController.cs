using BookStore.Models;
using BookStore.Models.ViewModels;
using BookStore.Repository;
using BookStore.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class ReaderController : Controller
    {
        private readonly IBaseRepository<Reader> _readersRepository;
        private IWebHostEnvironment _hostingEnvironment { get; }
        private readonly UserManager<Reader> _userManager;
        private readonly SignInManager<Reader> _signInManager;

        public ReaderController(IBaseRepository<Reader> readersRepository
            , IWebHostEnvironment hostingEnvironment
            , UserManager<Reader> userManager
            , SignInManager<Reader> signInManager)
        {
            _readersRepository = readersRepository;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            string imageFileName;
            if (model.Image != null)
            {
                imageFileName = FileUpload.ProcessUploadedFile(model.Image, _hostingEnvironment, "images");
            }
            else
            {
                imageFileName = "no-avatar.jpg";
            }

            if (ModelState.IsValid)
            {
                Reader reader = new Reader()
                {
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    Address = model.Address,
                    Gender = model.Gender,
                    PhoneNumber = model.Phone,
                    ImagePath = imageFileName
                };

                var result = await _userManager.CreateAsync(reader, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(reader, isPersistent: false);
                    return RedirectToAction("index", "home");
                }

                foreach (IdentityError error in result.Errors)
                {
                    switch (error.Code)
                    {
                        case "PasswordTooShort":
                            ModelState.AddModelError("Password", "Паролата трябва да съдържа поне 6 символа");
                            break;

                        case "PasswordRequiresNonAlphanumeric":
                            ModelState.AddModelError("Password", "Паролата трябва да съдържа поне един символ, които не е цифра или буква");
                            break;

                        case "PasswordRequiresLower":
                            ModelState.AddModelError("Password", "Паролата трябва да съдържа поне една малка буква");
                            break;

                        case "PasswordRequiresUpper":
                            ModelState.AddModelError("Password", "Паролата трябва да съдържа поне една главна буква");
                            break;

                        default:
                            break;
                    }
                    
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberUser, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }

                    return RedirectToAction("index", "home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login atempt");
            }

            return View(model);
        }

        public IActionResult Index()
        {
            List<Reader> readers = _readersRepository.GetTable().ToList();
            return View("ViewReaders", readers);
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(String email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Вече има потребител с имейл {email}");
            }
        }

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    ReaderViewModel model = new ReaderViewModel();
        //    return View("CreateReader", model);
        //}

        //[HttpPost]
        //public IActionResult Create(ReaderViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        model = new ReaderViewModel();
        //        return View("CreateReader", model);
        //    }

        //    string imageFileName;
        //    if (model.Image != null)
        //    {
        //        imageFileName = FileUpload.ProcessUploadedFile(model.Image, _hostingEnvironment, "images");
        //    }
        //    else
        //    {
        //        imageFileName = "no-avatar.jpg";
        //    }

        //    Reader reader = new Reader
        //    {
        //        FirstName = model.FirstName,
        //        MiddleName = model.MiddleName,
        //        LastName = model.LastName,
        //        Address = model.Address,
        //        Gender = model.Gender,
        //        PhoneNumber = model.Phone,
        //        ImagePath = imageFileName
        //    };

        //    _readersRepository.Insert(reader);

        //    return RedirectToAction("View", new { id = reader.Id });
        //}

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
            readerViewModel.Phone = reader.PhoneNumber;

            ViewBag.ReaderId = reader.Id;
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
                model.Phone = reader.PhoneNumber;

                ViewBag.ReaderId = id;
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
            reader.PhoneNumber = model.Phone;

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
