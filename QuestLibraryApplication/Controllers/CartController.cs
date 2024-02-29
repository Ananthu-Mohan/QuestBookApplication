using Newtonsoft.Json;
using QuestLibraryApplication.Abstraction;
using QuestLibraryApplication.Models.BookModel;
using QuestLibraryApplication.Models.OrderModel;
using QuestLibraryApplication.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using Syncfusion.Pdf.Grid;
using System.Data;
using System.Web.DynamicData;

namespace QuestLibraryApplication.Controllers
{
    [RoutePrefix("Cart")]
    public class CartController : Controller
    {
        private string url = @"https://localhost:44399";
        IHttpUtility _httpUtility;
        List<Order> SearchedOrderDetails = new List<Order>();
        // GET: Cart
        [HttpGet]
        [Route("")]
        [Route("UserOrderDetails")]
        public async Task<ActionResult> UserDefinedIndex()
        {
            ViewBag.Title = "Order Details";
            url = $"{url}/api/GetMultipleOrderDetails";
            _httpUtility = new HttpUtilityClass(url);
            var response = await _httpUtility.GetAsyncMethod();

            if(response !=null)
            {
                if (Session["userID"] != null)
                {
                    int userID = Convert.ToInt32(Session["userID"]);
                    List<Order> orderDetails = JsonConvert.DeserializeObject<List<Order>>(response);
                    
                    foreach (var order in orderDetails)
                    {
                        if (order.UserID == userID)
                        {
                            SearchedOrderDetails.Add(order);
                        }
                    }
                    
                    if (SearchedOrderDetails.Count > 0)
                    {
                        Session["OrderDetails"] = SearchedOrderDetails;
                        return View(SearchedOrderDetails);
                    }
                    else
                    {
                        return HttpNotFound("SearchedOrderDetails = null");
                    }
                }
                else
                {
                    return HttpNotFound("No orders associated with this User");
                }
            }
            else
            {
                return HttpNotFound("Orders Not Found");
            }
            
        }
        [HttpGet]
        [Route("AllOrderDetails")]
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "All the Order Details";
            url = $"{url}/api/GetMultipleOrderDetails";
            _httpUtility = new HttpUtilityClass(url);
            var response = await _httpUtility.GetAsyncMethod();

            if (response != null)
            {
                List<Order> orderDetails = JsonConvert.DeserializeObject<List<Order>>(response);
                return View(orderDetails);
            }
            else
            {
                return HttpNotFound("Orders Not Found");
            }
        }
        [HttpGet]
        [Route("GeneratePDF")]
        public async Task<ActionResult> CreateInvoice()
        {
            ViewBag.Title = "Creating Invoice PDF";
            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();
            PdfGrid pdfGrid = new PdfGrid();
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("OrderRefID");
            dataTable.Columns.Add("BookName");
            dataTable.Columns.Add("TotalBookCount");
            dataTable.Columns.Add("Username");
            dataTable.Columns.Add("DateOfPurchase");
            dataTable.Columns.Add("TotalPrice");
            List<Order> NewSearchedOrderDetails = Session["OrderDetails"] as List<Order>;

            foreach (var order in NewSearchedOrderDetails)
            {
                dataTable.Rows.Add(new object[] { $"{order.OrderRefID}", $"{order.BookName}", $"{order.TotalBookCount}", $"{order.Username}", $"{order.DateOfPurchase}", $"{order.TotalPrice}" });
            }
            pdfGrid.DataSource = dataTable;
            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);
            pdfGrid.Draw(page, new PointF(10, 10));
            doc.Save("Invoice.pdf", HttpContext.ApplicationInstance.Response, HttpReadType.Save);
            doc.Close(true);

            return RedirectToAction("UserDefinedIndex");
        }
        [HttpGet]
        [Route("CreateNewOrder/{id}")]
        public async Task<ActionResult> CreateCart(int? id)
        {
            ViewBag.Title = "Create new Orders";
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

                Order newOrder = new Order();
                //newOrder.OrderRefID = Convert.ToInt32(DateTime.Now.ToString().Replace(" ","").Replace(":","").Replace("-",""));
                newOrder.BookID = (int)id;
                newOrder.BookName = IdentifiedResult.BookName;
                newOrder.Username = Session["user"].ToString();
                newOrder.UserID = Convert.ToInt32(Session["userID"]);
                string refNp = DateTime.UtcNow.ToString().Replace(" ", "").Replace(":", "").Replace("-", "").Replace("AM", "").Replace("PM", "") + newOrder.UserID + newOrder.BookID;
                newOrder.DateOfPurchase = DateTime.Now;
                newOrder.OrderRefID = Convert.ToInt64(refNp);
                return View(newOrder);
            }
        }
        [HttpPost]
        [Route("CreateNewOrder/{id}")]
        public async Task<ActionResult> CreateCart(int? id,Order newOrderDetails)
        {
            ViewBag.Title = "Create new Orders";
            url = $"{url}/api/GetSingleBookData/{newOrderDetails.BookID}";
            _httpUtility = new HttpUtilityClass(url);

            var response = await _httpUtility.GetAsyncMethod();

            if (response == null)
            {
                return HttpNotFound($"{url} -> Request not found (Edit)");
            }
            else
            {
                var IdentifiedResult = JsonConvert.DeserializeObject<BookDB>(response);
                newOrderDetails.BookID = IdentifiedResult.BookID;
                newOrderDetails.BookName = IdentifiedResult.BookName;
                newOrderDetails.Username = Session["user"].ToString();
                newOrderDetails.UserID = Convert.ToInt32(Session["userID"]);
                string refNp = DateTime.UtcNow.ToString().Replace(" ", "").Replace(":", "").Replace("-", "").Replace("AM", "").Replace("PM", "") + newOrderDetails.UserID + newOrderDetails.BookID;
                newOrderDetails.DateOfPurchase = DateTime.Now;
                newOrderDetails.OrderRefID = Convert.ToInt64(refNp);
                if (newOrderDetails.TotalBookCount != 0)
                {
                    newOrderDetails.TotalPrice = newOrderDetails.TotalBookCount * IdentifiedResult.BookPrice;
                }
                else
                {
                    newOrderDetails.TotalPrice = IdentifiedResult.BookPrice;
                }

                url = @"https://localhost:44399";
                url = $"{url}/api/CreateNewOrderDetails";
                _httpUtility = new HttpUtilityClass(url);

                var serializedResult = JsonConvert.SerializeObject(newOrderDetails);
                var CreateResponse = await _httpUtility.CreateAsyncMethod(serializedResult);
                if (CreateResponse != null)
                {
                    return RedirectToAction("Index","Book");
                }
                else
                {
                    return View();
                }
            }
        }
    }
}