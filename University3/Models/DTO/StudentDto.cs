using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University3.Models.DTO
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<CourseDto> Courses { get; set; }
    }
}
