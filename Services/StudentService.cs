// StudentService.cs
// This service handles all database operations for Student entities
// including CRUD operations and password hashing with BCrypt

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SkillSwap.Data;
using SkillSwap.Data.Entities;
using SkillSwap.Models;
using SkillSwap.Services.Interfaces;

namespace SkillSwap.Services
{
    /// <summary>
    /// Service for managing Student entities and their data access layer operations
    /// </summary>
    public class StudentService : IStudentService
    {
        // Database context for Entity Framework Core operations
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor with dependency injection of database context
        /// </summary>
        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all students from the database
        /// </summary>
        public IEnumerable<Student> GetAllStudents()
        {
            return _context.Students
                .AsNoTracking() // Read-only query optimization
                .ToList()
                .Select(e => new Student(e.StudentId, e.Name, e.Email, e.Password));
        }

        /// <summary>
        /// Finds a student by their unique ID
        /// </summary>
        public Student? GetStudentById(Guid id)
        {
            var e = _context.Students.Find(id);
            return e == null ? null : new Student(e.StudentId, e.Name, e.Email, e.Password);
        }

        /// <summary>
        /// Finds a student by their email address (case-insensitive)
        /// </summary>
        public Student? GetStudentByEmail(string email)
        {
            var e = _context.Students
                .AsNoTracking()
                .FirstOrDefault(s => s.Email.ToLower() == email.ToLower());
            return e == null ? null : new Student(e.StudentId, e.Name, e.Email, e.Password);
        }

        /// <summary>
        /// Creates a new student with default password (for admin-created accounts)
        /// </summary>
        public Student CreateStudent(string name, string email)
        {
            return CreateStudent(name, email, "password");
        }

        /// <summary>
        /// Creates a new student with specified password
        /// Password is hashed using BCrypt before storage for security
        /// </summary>
        public Student CreateStudent(string name, string email, string password)
        {
            // Hash the password using BCrypt with salt rounds of 12
            // BCrypt automatically generates and embeds a salt, making rainbow table attacks ineffective
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

            // Create entity with hashed password
            var entity = new StudentEntity 
            { 
                StudentId = Guid.NewGuid(), 
                Name = name, 
                Email = email, 
                Password = hashedPassword // Store hashed password, never plain text
            };
            
            _context.Students.Add(entity);
            _context.SaveChanges();
            
            return new Student(entity.StudentId, entity.Name, entity.Email, entity.Password);
        }

        /// <summary>
        /// Updates an existing student's name and email
        /// </summary>
        public void UpdateStudent(Guid id, string name, string email)
        {
            var entity = _context.Students.Find(id);
            if (entity == null) return;
            entity.Name = name;
            entity.Email = email;
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes a student by their ID
        /// </summary>
        public void DeleteStudent(Guid id)
        {
            var entity = _context.Students.Find(id);
            if (entity == null) return;
            _context.Students.Remove(entity);
            _context.SaveChanges();
        }
    }
}
