using DatabaseConnectionAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DatabaseConnectionAPI.Data
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext():base("name=UtilityDB")
        {
            Database.SetInitializer<ApplicationDBContext>(new DropCreateDatabaseIfModelChanges<ApplicationDBContext>());
        }
        public DbSet<IdentityDB> Identity { get; set; }
        public DbSet<BookDB> Book { get; set; }
    }
}