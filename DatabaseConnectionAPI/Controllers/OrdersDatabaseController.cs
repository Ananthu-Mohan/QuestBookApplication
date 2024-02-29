using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DatabaseConnectionAPI.Data;
using QuestLibraryApplication.Models.OrderModel;

namespace DatabaseConnectionAPI.Controllers
{
    public class OrdersDatabaseController : ApiController
    {
        private ApplicationDBContext db = new ApplicationDBContext();

        // GET: api/OrdersDatabase
        [HttpGet]
        [Route("api/GetMultipleOrderDetails")]
        public IQueryable<Order> GetMultipleOrderDetails()
        {
            return db.Orders;
        }

        // GET: api/OrdersDatabase/5
        [HttpGet]
        [Route("api/GetSingleOrderDetails/{id}")]
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> GetSingleOrderDetails(long id)
        {
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/OrdersDatabase/5
        [HttpPut]
        [Route("api/EditOrderDetails/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> EditOrderDetails(long id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.OrderRefID)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/OrdersDatabase
        [HttpPost]
        [Route("api/CreateNewOrderDetails")]
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> CreateNewOrderDetails(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Orders.Add(order);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new {controller= "OrdersDatabase", id = order.OrderRefID }, order);
        }

        // DELETE: api/OrdersDatabase/5
        [HttpDelete]
        [Route("api/DeleteOrderDetails/{id}")]
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> DeleteOrderDetails(long id)
        {
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            await db.SaveChangesAsync();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(long id)
        {
            return db.Orders.Count(e => e.OrderRefID == id) > 0;
        }
    }
}