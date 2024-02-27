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
using DatabaseConnectionAPI.Models;

namespace DatabaseConnectionAPI.Controllers
{
    public class BookDatabaseController : ApiController
    {
        private ApplicationDBContext db = new ApplicationDBContext();

        // GET: api/BookDatabase
        [HttpGet]
        [Route("api/GetBookData")]
        public IQueryable<BookDB> GetBookData()
        {
            return db.Book;
        }

        // GET: api/BookDatabase/5
        [HttpGet]
        [Route("api/GetSingleBookData/{id}")]
        [ResponseType(typeof(BookDB))]
        public async Task<IHttpActionResult> GetSingleBookData(int id)
        {
            BookDB bookDB = await db.Book.FindAsync(id);
            if (bookDB == null)
            {
                return NotFound();
            }

            return Ok(bookDB);
        }

        // PUT: api/BookDatabase/5
        [HttpPut]
        [Route("api/EditBookData/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> EditBookData(int id, BookDB bookDB)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bookDB.BookID)
            {
                return BadRequest();
            }

            db.Entry(bookDB).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookDBExists(id))
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

        // POST: api/BookDatabase
        [HttpPost]
        [Route("api/CreateBookData")]
        [ResponseType(typeof(BookDB))]
        public async Task<IHttpActionResult> CreateBookData(BookDB bookDB)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Book.Add(bookDB);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new {controller= "BookDatabase", id = bookDB.BookID }, bookDB);
        }

        // DELETE: api/BookDatabase/5
        [HttpDelete]
        [Route("api/DeleteBookData/{id}")]
        [ResponseType(typeof(BookDB))]
        public async Task<IHttpActionResult> DeleteBookData(int id)
        {
            BookDB bookDB = await db.Book.FindAsync(id);
            if (bookDB == null)
            {
                return NotFound();
            }

            db.Book.Remove(bookDB);
            await db.SaveChangesAsync();

            return Ok(bookDB);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookDBExists(int id)
        {
            return db.Book.Count(e => e.BookID == id) > 0;
        }
    }
}