﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University3.Models.Entities;

namespace University3.Data
{
    public class StudentRepository
    {
        private University3Context db;
        public StudentRepository(University3Context db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await db.Students
                .Include(s => s.Enrollments)
                .ThenInclude(s => s.Course)
                .ToListAsync();
        }
        public async Task<Student> GetStudentAsync(int id)
        {
            return await db.Students
                .Include(s => s.Enrollments)
                .ThenInclude(s => s.Course)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
