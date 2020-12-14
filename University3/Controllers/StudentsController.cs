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
        public async Task<ActionResult<IEnumerable<Student>>> GetAllStudents()
        {

            var result = await repo.GetAllStudentsAsync();
            var mappedResult = mapper.Map<IEnumerable<StudentDto>>(result);
            return Ok(mappedResult);

        }
    }
}
