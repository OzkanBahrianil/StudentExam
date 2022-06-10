
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZNetCS.AspNetCore.Logging.EntityFrameworkCore;

namespace DataAccessLayer.Concrete
{
    public class Context  : IdentityDbContext<AppUser, AppRole, int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
           optionsBuilder.UseSqlServer("server=(localdb)\\mssqllocaldb;initial catalog=StudentExam;integrated security=true");
 
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<ExtendedLog> Logs => this.Set<ExtendedLog>();
        //public DbSet<ZNetCS.AspNetCore.Logging.EntityFrameworkCore.Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // build default model.
            LogModelBuilderHelper.Build(modelBuilder.Entity<ExtendedLog>());

            // real relation database can map table:
            modelBuilder.Entity<ExtendedLog>().ToTable("Log");

            modelBuilder.Entity<ExtendedLog>().Property(r => r.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<ExtendedLog>().HasIndex(r => r.TimeStamp).HasName("IX_Log_TimeStamp");
            modelBuilder.Entity<ExtendedLog>().HasIndex(r => r.EventId).HasName("IX_Log_EventId");
            modelBuilder.Entity<ExtendedLog>().HasIndex(r => r.Level).HasName("IX_Log_Level");

            modelBuilder.Entity<ExtendedLog>().Property(u => u.Name).HasMaxLength(255);
            modelBuilder.Entity<ExtendedLog>().Property(u => u.Browser).HasMaxLength(255);
            modelBuilder.Entity<ExtendedLog>().Property(u => u.User).HasMaxLength(255);
            modelBuilder.Entity<ExtendedLog>().Property(u => u.Host).HasMaxLength(255);
            modelBuilder.Entity<ExtendedLog>().Property(u => u.Path).HasMaxLength(255);
        }
   
    }
}
