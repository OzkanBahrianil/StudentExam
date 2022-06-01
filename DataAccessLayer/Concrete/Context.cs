using EntityLayer.Concrate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class Context  : IdentityDbContext<AppUser, AppRole, int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
           optionsBuilder.UseSqlServer("server=(localdb)\\mssqllocaldb;initial catalog=Enoca;integrated security=true");

        }

        public DbSet<Student> Students { get; set; }

    }
}
