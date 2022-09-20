using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Controllers
{
    public class PublisherController : Controller
    {
        private readonly IBaseRepository<Publisher> _publisherRepository;

        public PublisherController(IBaseRepository<Publisher> publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        public ActionResult Index(string searchString)
        {
            List<Publisher> publishers = new List<Publisher>();
            if (searchString != null)
            {
                publishers = _publisherRepository.GetTable()
                                                 .Where( publisher => publisher.Name.ToLower().IndexOf( searchString.ToLower() ) >= 0 )
                                                 .ToList();
            }
            else
                publishers = _publisherRepository.GetTable().ToList();

            return View("ViewPublishers", publishers);
        }

        [HttpGet]
        public ActionResult Create()
        {
            Publisher publisher = new Publisher();
            return View("Publisher", publisher);
        }

        [HttpPost]
        public ActionResult Create(Publisher publisher)
        {
            if (!ModelState.IsValid)
                return View("Publisher", publisher);

            _publisherRepository.Insert(publisher);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Publisher record = _publisherRepository.SelectById(id);
            ViewBag.RecordId = id;

            return View("Publisher", record);
        }

        [HttpPost]
        public IActionResult Edit(Publisher record, int id)
        {
            if (!ModelState.IsValid)
                return View("Publisher", record);

            _publisherRepository.Update(record);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            _publisherRepository.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
