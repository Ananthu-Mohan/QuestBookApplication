﻿using Newtonsoft.Json;
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
    [RoutePrefix("User")]
    public class IdentityController : Controller
    {
        private string url = @"https://localhost:44399";
        IHttpUtility _httpUtility;

        // GET: Identity
        [HttpGet]
        [Route("")]
        [Route("MultipleUserDetails")]
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "All the User details";
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
        [Route("UserDetails/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            ViewBag.Title = "User detail";
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
        [Route("CreateNewUser")]
        public ActionResult Create()
        {
            ViewBag.Title = "Create new User";
            return View();
        }

        // POST: Identity/Create
        [HttpPost]
        [Route("CreateNewUser")]
        public async Task<ActionResult> Create(IdentityDB createUserDetails)
        {
            ViewBag.Title = "Create new User";
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
        [HttpGet]
        [Route("EditUser/{id}")]
        public async Task<ActionResult> Edit(int id)
        {
            ViewBag.Title = "Edit User";
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
        [Route("EditUser/{id}")]
        public async Task<ActionResult> Edit(int id, IdentityDB UpdatedUserDetails)
        {
            ViewBag.Title = "Edit User";
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
        [HttpGet]
        [Route("DeleteUser/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            ViewBag.Title = "Delete User";
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
        [Route("DeleteUser/{id}")]
        public async Task<ActionResult> Delete(int id, IdentityDB userDetails)
        {
            ViewBag.Title = "Delete User";
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
