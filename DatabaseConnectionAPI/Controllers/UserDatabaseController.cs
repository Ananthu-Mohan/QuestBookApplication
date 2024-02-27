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
    public class UserDatabaseController : ApiController
    {
        private ApplicationDBContext db = new ApplicationDBContext();

        // GET: api/UserDatabase
        [HttpGet]
        [Route("api/GetMultipleUsers")]
        public IQueryable<IdentityDB> GetMultipleUsers()
        {
            return db.Identity;
        }

        // GET: api/UserDatabase/5
        [HttpGet]
        [Route("api/GetSingleUser/{id}")]
        [ResponseType(typeof(IdentityDB))]
        public async Task<IHttpActionResult> GetSingleUser(int id)
        {
            IdentityDB identityDB = await db.Identity.FindAsync(id);
            if (identityDB == null)
            {
                return NotFound();
            }

            return Ok(identityDB);
        }

        // PUT: api/UserDatabase/5
        [HttpPut]
        [Route("api/EditUser/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> EditUser(int id, IdentityDB identityDB)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != identityDB.UserID)
            {
                return BadRequest();
            }

            db.Entry(identityDB).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IdentityDBExists(id))
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

        // POST: api/UserDatabase
        [HttpPost]
        [Route("api/CreateUser")]
        [ResponseType(typeof(IdentityDB))]
        public async Task<IHttpActionResult> CreateUser(IdentityDB identityDB)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Identity.Add(identityDB);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { controller= "UserDatabase", id = identityDB.UserID }, identityDB);
        }

        // DELETE: api/UserDatabase/5
        [HttpDelete]
        [Route("api/DeleteUser/{id}")]
        [ResponseType(typeof(IdentityDB))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            IdentityDB identityDB = await db.Identity.FindAsync(id);
            if (identityDB == null)
            {
                return NotFound();
            }

            db.Identity.Remove(identityDB);
            await db.SaveChangesAsync();

            return Ok(identityDB);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IdentityDBExists(int id)
        {
            return db.Identity.Count(e => e.UserID == id) > 0;
        }
    }
}