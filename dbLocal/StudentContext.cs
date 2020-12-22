using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Data.Common;
using AllModels;

namespace Localdb
{
    public class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Subject>().ToTable("Subject");
           
        }

        public DbSet<Student> Student { get; set; }
        public DbSet<Subject> Subject { get; set; }
       
    }
}
