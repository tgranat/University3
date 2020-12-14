using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University3.Data;

namespace University3.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private StudentRepository repo;

        public StudentsController(University3Context context)
        {
            repo = new StudentRepository(context);
        }
    }
}
