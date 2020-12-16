using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University3.Models.Entities;

namespace University3.Data
{
    public class University3Context : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public University3Context(DbContextOptions<University3Context> options) : base(options) { }
    }
}
