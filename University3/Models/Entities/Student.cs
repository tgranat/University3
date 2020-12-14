using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University3.Models.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}
