﻿using AutoMapper;
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
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents(bool includeCourses = false)
        {
            var result = await repo.GetAllStudentsAsync(includeCourses);
            var mappedResult = mapper.Map<IEnumerable<StudentDto>>(result);
            return Ok(mappedResult);

        }

        // Another way to get all students including their courses
        [HttpGet("course")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudentsAndCourse()
        {
            var result = await repo.GetAllStudentsAsync(true);
            var mappedResult = mapper.Map<IEnumerable<StudentDto>>(result);
            return Ok(mappedResult);

        }

        [HttpGet("searchByEmail/{email}")]
        public async Task<ActionResult<StudentDto>> GetStudent(string email, bool includeCourses = false)
        {
            email = email.Trim();
            var result = await repo.GetStudentAsync(email, includeCourses);
            var mappedResult = mapper.Map<StudentDto>(result);
            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id, bool includeCourses = false)
        {
            var result = await repo.GetStudentAsync(id, includeCourses);
            var mappedResult = mapper.Map<StudentDto>(result);
            return Ok(mappedResult);
        }

        // Another way to get a student including their course
        [HttpGet]
        [Route("{id}/course")]
        public async Task<ActionResult<StudentDto>> GetStudentAndCourse(int id)
        {
            var result = await repo.GetStudentAsync(id, true);
            var mappedResult = mapper.Map<StudentDto>(result);
            return Ok(mappedResult);
        }

        [HttpPost]
        public async Task<ActionResult<StudentDto>> CreateStudent(StudentDto studentDto)
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

        // Update Student data only
        [HttpPut("{id}")]
        public async Task<ActionResult<StudentDto>> UpdateStudent(int id, StudentDto studentDto)
        {
            var student = await repo.GetStudentAsync(id, false);
            if (student is null) return StatusCode(StatusCodes.Status404NotFound);
            // Map incoming StudentDto to a Student
            mapper.Map(studentDto, student);
            if (await repo.SaveAsync())
            {
                // Map updated Student to StudentDto to return
                return Ok(mapper.Map<StudentDto>(student));
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{id}/course/{courseId}")]
        public async Task<ActionResult<StudentDto>> AddCourseToStudent(int id, int courseId)
        { 
            var student = await repo.GetStudentAsync(id, false);
            if (student is null) return StatusCode(StatusCodes.Status404NotFound);
            var course = await repo.GetCourseAsync(courseId, false);
            if (course is null) return StatusCode(StatusCodes.Status404NotFound);
            if (repo.EnrollmentExists(id, courseId))
            {
                ModelState.AddModelError("StudentId", "Student already enrolled to this course");
                return BadRequest(ModelState);
            }
            
            var enrollment = new Enrollment
            {
                StudentId = id,
                CourseId = courseId
            };
            await repo.AddAsync(enrollment);
            if (await repo.SaveAsync())
            {
                student = await repo.GetStudentAsync(id, true);
                return Ok(mapper.Map<StudentDto>(student));
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}/course/{courseId}")]
        public async Task<ActionResult<StudentDto>> RemoveCourseFromStudent(int id, int courseId)
        {
            var enrollment = await repo.GetEnrollmentAsync(id, courseId);
            if (enrollment is null) return StatusCode(StatusCodes.Status404NotFound);
            repo.Remove(enrollment);
            if (await repo.SaveAsync())
            {
                var student = await repo.GetStudentAsync(id, true);
                return Ok(mapper.Map<StudentDto>(student));
            }
            else
            {
                return BadRequest();
            }
        }

        // TODO: test, move this to CoursesController
        [HttpGet("allcourses")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllCourses()
        {
            var result = await repo.GetAllCoursesAsync(true);
            var mappedResult = mapper.Map<IEnumerable<CourseDto>>(result);
            return Ok(mappedResult);

        }

    }
}
