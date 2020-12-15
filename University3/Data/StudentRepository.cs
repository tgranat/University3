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

        public async Task<IEnumerable<Student>> GetAllStudentsAsync(bool includeCourses)
        {
            return includeCourses ?
                await db.Students
                .Include(s => s.Enrollments)
                .ThenInclude(s => s.Course)
                .ToListAsync() :
                await db.Students
                .ToListAsync();
        }
        public async Task<Student> GetStudentAsync(string email, bool includeCourses)
        {
            return includeCourses ?
                await db.Students
                .Include(s => s.Enrollments)
                .ThenInclude(s => s.Course)
                .FirstOrDefaultAsync(s => s.Email == email) :
                await db.Students
                .FirstOrDefaultAsync(s => s.Email == email);
        }
        public async Task<Student> GetStudentAsync(int id, bool includeCourses)
        {
            var query = db.Students.AsQueryable();
            query = includeCourses ? query.Include(s => s.Enrollments).ThenInclude(s => s.Course) : query;
            return await query.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync(bool includeStudents)
        {
            return includeStudents ?
                await db.Courses
                .Include(s => s.Enrollments)
                .ThenInclude(s => s.Student)
                .ToListAsync() :
                await db.Courses
                .ToListAsync();
        }

        public async Task<Course> GetCourseAsync(bool includeStudents)
        {
            return includeStudents ?
                await db.Courses
                .Include(s => s.Enrollments)
                .ThenInclude(s => s.Student)
                .FirstOrDefaultAsync() :
                await db.Courses
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync<T>(T added)
        {
            await db.AddAsync(added);
        }

        public async Task<bool> SaveAsync()
        {
            return (await db.SaveChangesAsync()) >= 0;
        }
    }
}
