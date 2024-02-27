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
    public class UserLoginController : Controller
    {
        private string url = @"https://localhost:44399";
        IHttpUtility _httpUtility;
        // GET: UserLogin
        [HttpGet]
        public async Task<ActionResult> IdentityLogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> IdentityLogin(IdentityDB userLogin)
        {
            url = $"{url}/api/GetMultipleUsers";
            _httpUtility = new HttpUtilityClass(url);

            var response = await _httpUtility.GetAsyncMethod();
            if (response == null)
            {
                return HttpNotFound($"{url} -> Request not found");
            }
            else
            {
                List<IdentityDB> databaseUserDetails = JsonConvert.DeserializeObject<List<IdentityDB>>(response);

                IdentityDB searchedUser = new IdentityDB();

                foreach (var user in databaseUserDetails)
                {
                    if(user.UserName == userLogin.UserName)
                    {
                        searchedUser = user;
                        break;
                    }
                }

                if(searchedUser == null)
                {
                    return HttpNotFound($"User - {userLogin.UserName} not found");
                }
                else
                {
                    if((String.Equals(searchedUser.UserName,userLogin.UserName, StringComparison.CurrentCultureIgnoreCase))
                        &&(String.Equals(searchedUser.Password,userLogin.Password, StringComparison.CurrentCultureIgnoreCase))) 
                    {
                        Session["user"] = userLogin.UserName.ToUpper();

                        url = @"https://localhost:44399";
                        url = $"{url}/api/EditUser/{searchedUser.UserID}";
                        _httpUtility = new HttpUtilityClass(url);

                        searchedUser.LastLoginAt = DateTime.Now;
                        var serializedResult = JsonConvert.SerializeObject(searchedUser);
                        var editResponse = await _httpUtility.EditAsyncMethod(serializedResult);

                        if (String.Equals(userLogin.UserName,"admin", StringComparison.CurrentCultureIgnoreCase))
                        { 
                            return RedirectToAction("Index","Identity");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Book");
                        }
                    }
                    else
                    {
                        return HttpNotFound($"Username and password mismatch occurred");
                    }
                }
            }
        }
    }
}