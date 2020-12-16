using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University3.Models.Entities;

namespace University3.Data
{
    public class StudentRepository
    {
        private readonly University3Context db;
        public StudentRepository(University3Context db)
        {
            this.db = db;
        }

        // Skip and Take used for paging
        public async Task<IEnumerable<Student>> GetAllStudentsAsync(bool includeCourses, int skip = 0, int take = 10)
        {
            return includeCourses ?
                await db.Students
                .Include(s => s.Enrollments)
                .ThenInclude(s => s.Course)
                .Skip(skip)
                .Take(take)
                .ToListAsync() :
                await db.Students
                .Skip(skip)
                .Take(take)
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

        public async Task<Course> GetCourseAsync(int courseId, bool includeStudents)
        {
            return includeStudents ?
                await db.Courses
                .Include(s => s.Enrollments)
                .ThenInclude(s => s.Student)
                .FirstOrDefaultAsync(c => c.Id == courseId) :
                await db.Courses
                .FirstOrDefaultAsync(c => c.Id == courseId);
        }

        public async Task<Enrollment> GetEnrollmentAsync(int studentId, int courseId)
        {
            return await db.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }

        public bool EnrollmentExists(int studentId, int courseId)
        {
            return db.Enrollments.Any(e => e.StudentId == studentId && e.CourseId == courseId);
        }
         
        public void Remove<T>(T removed)
        {
            db.Remove(removed);
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
