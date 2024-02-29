using Newtonsoft.Json;
using QuestLibraryApplication.Abstraction;
using QuestLibraryApplication.Models.BookModel;
using QuestLibraryApplication.Models.OrderModel;
using QuestLibraryApplication.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuestLibraryApplication.Controllers
{
    [RoutePrefix("Book")]
    public class BookController : Controller
    {
        private string url = @"https://localhost:44399";
        IHttpUtility _httpUtility;
        // GET: Book
        [HttpGet]
        [Route("")]
        [Route("MultipleBookDetails")]
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "Book Records";
            url = $"{url}/api/GetBookData";
            _httpUtility = new HttpUtilityClass(url);

            var response = await _httpUtility.GetAsyncMethod();
            if (response == null)
            {
                return HttpNotFound($"{url} -> Request not found");
            }
            else
            {
                return View(JsonConvert.DeserializeObject<List<BookDB>>(response));
            }
        }

        // GET: Book/Details/5
        [HttpGet]
        [Route("SingleBookDetails/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            ViewBag.Title = "Book Details";
            url = $"{url}/api/GetSingleBookData/{id}";
            _httpUtility = new HttpUtilityClass(url);

            var response = await _httpUtility.GetAsyncMethod();

            if (response == null)
            {
                return HttpNotFound($"{url} -> Request not found");
            }
            else
            {
                var IdentifiedResult = JsonConvert.DeserializeObject<BookDB>(response);
                if (IdentifiedResult.ReleasedDate > DateTime.Now)
                {
                    Session["released"] = "Released Status : Not Active";
                }
                else
                {
                    Session["released"] = "Released Status : Active";
                }
                return View(IdentifiedResult);
            }
        }

        // GET: Book/Create
        [HttpGet]
        [Route("CreateNewBook")]
        public ActionResult Create()
        {
            ViewBag.Title = "Create New Book";
            return View();
        }

        // POST: Book/Create
        [HttpPost]
        [Route("CreateNewBook")]
        public async Task<ActionResult> Create(BookDB books)
        {
            ViewBag.Title = "Create New Book";
            try
            {
                // TODO: Add insert logic here
                url = $"{url}/api/GetBookData";
                _httpUtility = new HttpUtilityClass(url);

                var allTheBookDetails = JsonConvert.DeserializeObject<List<BookDB>>(await _httpUtility.GetAsyncMethod());

                foreach (var book in allTheBookDetails)
                {
                    if (String.Equals(book.BookName, books.BookName))
                    {
                        return HttpNotFound($"{book.BookName} already exists");
                    }
                }
                url = @"https://localhost:44399"; 
                url = $"{url}/api/CreateBookData";
                _httpUtility = new HttpUtilityClass(url);

                var serializedResult = JsonConvert.SerializeObject(books);
                var response = await _httpUtility.CreateAsyncMethod(serializedResult);

                if (response != null)
                {
                    if(books.ReleasedDate > DateTime.Now)
                    {
                        Session["released"] = "Released Status : Not Active";
                    }
                    else
                    {
                        Session["released"] = "Released Status : Active";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Edit/5
        [HttpGet]
        [Route("EditBook/{id}")]
        public async Task<ActionResult> Edit(int id)
        {
            ViewBag.Title = "Edit Book";
            url = $"{url}/api/GetSingleBookData/{id}";
            _httpUtility = new HttpUtilityClass(url);

            var response = await _httpUtility.GetAsyncMethod();

            if (response == null)
            {
                return HttpNotFound($"{url} -> Request not found (Edit)");
            }
            else
            {
                var IdentifiedResult = JsonConvert.DeserializeObject<BookDB>(response);
                if (IdentifiedResult.ReleasedDate > DateTime.Now)
                {
                    Session["released"] = "Released Status : Not Active";
                }
                else
                {
                    Session["released"] = "Released Status : Active";
                }
                return View(IdentifiedResult);
            }
        }

        // POST: Book/Edit/5
        [HttpPost]
        [Route("EditBook/{id}")]
        public async Task<ActionResult> Edit(int id, BookDB updatedBookDetails)
        {
            ViewBag.Title = "Edit Book";
            try
            {
                // TODO: Add update logic here

                url = $"{url}/api/EditBookData/{id}";
                _httpUtility = new HttpUtilityClass(url);

                var serializedResult = JsonConvert.SerializeObject(updatedBookDetails);
                var response = await _httpUtility.EditAsyncMethod(serializedResult);

                if (response != null)
                {
                    if (updatedBookDetails.ReleasedDate > DateTime.Now)
                    {
                        Session["released"] = "Released Status : Not Active";
                    }
                    else
                    {
                        Session["released"] = "Released Status : Active";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }
        // GET: Book/Delete/5
        [HttpGet]
        [Route("DeleteBook/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            ViewBag.Title = "Delete Book";
            url = $"{url}/api/GetSingleBookData/{id}";
            _httpUtility = new HttpUtilityClass(url);

            var response = await _httpUtility.GetAsyncMethod();

            if (response == null)
            {
                return HttpNotFound($"{url} -> Request not found (Delete)");
            }
            else
            {
                return View(JsonConvert.DeserializeObject<BookDB>(response));
            }
        }

        // POST: Book/Delete/5
        [HttpPost]
        [Route("DeleteBook/{id}")]
        public async Task<ActionResult> Delete(int id, BookDB bookdetails)
        {
            ViewBag.Title = "Delete Book";
            try
            {
                // TODO: Add delete logic here
                url = $"{url}/api/DeleteBookData/{id}";
                _httpUtility = new HttpUtilityClass(url);
                var response = await _httpUtility.DeleteAsyncMethod();
                if (response != null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
