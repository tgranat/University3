using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University3.Models.Entities;

namespace University3.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider services)
        {
            // get options to be used by DbContext 
            var options = services.GetRequiredService<DbContextOptions<University3Context>>();

            using (var db = new University3Context(options))
            {
                if (db.Students.Any())
                {
                    //db.Students.RemoveRange(db.Students);
                    return;
                }
                var fake = new Faker("sv");

                var students = new List<Student>();

                for (int i = 0; i < 200; i++)
                {
                    var fName = fake.Name.FirstName();
                    var lName = fake.Name.LastName();

                    var student = new Student
                    {
                        FirstName = fName,
                        LastName = lName,
                        Email = fake.Internet.Email($"{fName} {lName}"),

                    };

                    students.Add(student);
                }

                db.AddRange(students);

                var courses = new List<Course>();

                for (int i = 0; i < 20; i++)
                {
                    var course = new Course
                    {
                        Name = fake.Company.CatchPhrase()
                    };

                    courses.Add(course);
                }

                db.AddRange(courses);

                var enrollments = new List<Enrollment>();

                foreach (var student in students)
                {
                    foreach (var course in courses)
                    {
                        if (fake.Random.Int(0, 5) == 0)
                        {
                            var enrollment = new Enrollment
                            {
                                Course = course,
                                Student = student,

                            };
                            enrollments.Add(enrollment);
                        }
                    }
                }

                db.AddRange(enrollments);
                db.SaveChanges();
            }
        }
    }
}
