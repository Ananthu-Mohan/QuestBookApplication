using Newtonsoft.Json;
using QuestLibraryApplication.Abstraction;
using QuestLibraryApplication.Models.UserModel;
using QuestLibraryApplication.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuestLibraryApplication.Controllers
{
    public class IdentityController : Controller
    {
        private string url = @"https://localhost:44399";
        IHttpUtility _httpUtility;

        // GET: Identity
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            url = $"{url}/api/GetMultipleUsers";
            _httpUtility = new HttpUtilityClass(url);
            
            var response = await _httpUtility.GetAsyncMethod();
            if(response == null )
            {
                return HttpNotFound($"{url} -> Request not found");
            }
            else
            {
                return View(JsonConvert.DeserializeObject<List<IdentityDB>>(response));
            }
            
        }

        // GET: Identity/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            url = $"{url}/api/GetSingleUser/{id}";
            _httpUtility = new HttpUtilityClass(url);

            var response = await _httpUtility.GetAsyncMethod();

            if (response == null)
            {
                return HttpNotFound($"{url} -> Request not found");
            }
            else
            {
                return View(JsonConvert.DeserializeObject<IdentityDB>(response));
            }
        }

        // GET: Identity/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Identity/Create
        [HttpPost]
        public async Task<ActionResult> Create(IdentityDB createUserDetails)
        {
            try
            {
                // TODO: Add insert logic here

                url = $"{url}/api/GetMultipleUsers";
                _httpUtility = new HttpUtilityClass(url);

                var allTheUserDetails = JsonConvert.DeserializeObject<List<IdentityDB>>(await _httpUtility.GetAsyncMethod());

                foreach(var user in allTheUserDetails)
                {
                    if(String.Equals(user.UserName,createUserDetails.UserName))
                    {
                        return HttpNotFound($"{user.UserName} already exists");
                    }
                }

                url = @"https://localhost:44399";
                url = $"{url}/api/CreateUser";
                _httpUtility = new HttpUtilityClass(url);

                createUserDetails.CreatedAt= DateTime.Now;
                createUserDetails.LastLoginAt= DateTime.Now;

                var serializedResult = JsonConvert.SerializeObject(createUserDetails);
                var response = await _httpUtility.CreateAsyncMethod(serializedResult);

                if(response !=null)
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

        // GET: Identity/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            url = $"{url}/api/GetSingleUser/{id}";
            _httpUtility = new HttpUtilityClass(url);

            var response = await _httpUtility.GetAsyncMethod();

            if (response == null)
            {
                return HttpNotFound($"{url} -> Request not found (Edit)");
            }
            else
            {
                return View(JsonConvert.DeserializeObject<IdentityDB>(response));
            }
        }

        // POST: Identity/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, IdentityDB UpdatedUserDetails)
        {
            try
            {
                // TODO: Add update logic here
                url = $"{url}/api/EditUser/{id}";
                _httpUtility = new HttpUtilityClass(url);

                var serializedResult = JsonConvert.SerializeObject(UpdatedUserDetails);
                var response = await _httpUtility.EditAsyncMethod(serializedResult);

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

        // GET: Identity/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            url = $"{url}/api/GetSingleUser/{id}";
            _httpUtility = new HttpUtilityClass(url);

            var response = await _httpUtility.GetAsyncMethod();

            if (response == null)
            {
                return HttpNotFound($"{url} -> Request not found (Delete)");
            }
            else
            {
                return View(JsonConvert.DeserializeObject<IdentityDB>(response));
            }
        }

        // POST: Identity/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, IdentityDB userDetails)
        {
            try
            {
                // TODO: Add delete logic here
                url = $"{url}/api/DeleteUser/{id}";
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
