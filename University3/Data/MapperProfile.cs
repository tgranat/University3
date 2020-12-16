using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University3.Models.DTO;
using University3.Models.Entities;

namespace University3.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Student, StudentDto>().ReverseMap();
            //CreateMap<Student, StudentDto>()
            //    .ForMember(
            //    dest => dest.Courses,
            //    from => from.MapFrom(c => c.Enrollments.Select(e => e.Course).ToList()));

            CreateMap<Enrollment, EnrollmentDto>().ReverseMap();
            CreateMap<Course, CourseDto>().ReverseMap(); 
            

        }
    }
}
