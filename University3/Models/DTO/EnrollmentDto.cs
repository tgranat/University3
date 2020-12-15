using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University3.Models.DTO
{
    public class EnrollmentDto
    {
        //public StudentDto Student { get; set; }
        public CourseDto Course { get; set; }
        public int CourseId { get; set; }
    }
}
