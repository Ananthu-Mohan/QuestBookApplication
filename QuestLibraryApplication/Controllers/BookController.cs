using Newtonsoft.Json;
using QuestLibraryApplication.Abstraction;
using QuestLibraryApplication.Models.BookModel;
using QuestLibraryApplication.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuestLibraryApplication.Controllers
{
    public class BookController : Controller
    {
        private string url = @"https://localhost:44399";
        IHttpUtility _httpUtility;
        // GET: Book
        [HttpGet]
        public async Task<ActionResult> Index()
        {
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
        public async Task<ActionResult> Details(int id)
        {
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        [HttpPost]
        public async Task<ActionResult> Create(BookDB books)
        {
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
        public async Task<ActionResult> Edit(int id)
        {
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
        public async Task<ActionResult> Edit(int id, BookDB updatedBookDetails)
        {
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
        public async Task<ActionResult> Delete(int id)
        {
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
        public async Task<ActionResult> Delete(int id, BookDB bookdetails)
        {
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
