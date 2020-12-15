using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University3.Data;
using University3.Models.DTO;
using University3.Models.Entities;

namespace University3.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private StudentRepository repo;
        private readonly IMapper mapper;

        public StudentsController(University3Context context, IMapper mapper)
        {
            repo = new StudentRepository(context);
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAllStudents(bool includeCourses = false)
        {
            var result = await repo.GetAllStudentsAsync(includeCourses);
            var mappedResult = mapper.Map<IEnumerable<StudentDto>>(result);
            return Ok(mappedResult);

        }

        // Another way to get all students including their courses
        [HttpGet("course")]
        public async Task<ActionResult<IEnumerable<Student>>> GetAllStudentsAndCourse()
        {
            var result = await repo.GetAllStudentsAsync(true);
            var mappedResult = mapper.Map<IEnumerable<StudentDto>>(result);
            return Ok(mappedResult);

        }

        [HttpGet("searchByEmail/{email}")]
        public async Task<ActionResult<StudentDto>> GetStudent(string email, bool includeCourses = false)
        {
            var result = await repo.GetStudentAsync(email, includeCourses);
            var mappedResult = mapper.Map<StudentDto>(result);
            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id, bool includeCourses = false)
        {

            var result = await repo.GetStudentAsync(id, includeCourses);
            var mappedResult = mapper.Map<StudentDto>(result);
            return Ok(mappedResult);
        }

        // Another way to get a student including their course
        [HttpGet]
        [Route("{id}/course")]
        public async Task<ActionResult<Student>> GetStudentAndCourse(int id)
        {
            var result = await repo.GetStudentAsync(id, true);
            var mappedResult = mapper.Map<StudentDto>(result);
            return Ok(mappedResult);
        }

        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent(StudentDto studentDto)
        {
            if (await repo.GetStudentAsync(studentDto.Email, false) != null)
            {
                ModelState.AddModelError("Email", "Email in use");
                return BadRequest(ModelState);
            }

            var student = mapper.Map<Student>(studentDto);
            await repo.AddAsync(student);

            if (await repo.SaveAsync())
            {
                var model = mapper.Map<StudentDto>(student);
                return CreatedAtAction(nameof(GetStudent), new { email = model.Email }, model);
            }
            else
            {
                return BadRequest();
            }
        }


        // TODO: test, move this to CoursesController
        [HttpGet("allcourses")]
        public async Task<ActionResult<IEnumerable<Student>>> GetAllCourses()
        {
            var result = await repo.GetAllCoursesAsync(true);
            var mappedResult = mapper.Map<IEnumerable<CourseDto>>(result);
            return Ok(mappedResult);

        }

    }
}
