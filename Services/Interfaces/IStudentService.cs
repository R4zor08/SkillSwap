using System;
using System.Collections.Generic;
using SkillSwap.Models;

namespace SkillSwap.Services.Interfaces
{
    public interface IStudentService
    {
        IEnumerable<Student> GetAllStudents();
        Student? GetStudentById(Guid id);
        Student? GetStudentByEmail(string email);
        Student CreateStudent(string name, string email);
        Student CreateStudent(string name, string email, string password);
        void UpdateStudent(Guid id, string name, string email);
        void DeleteStudent(Guid id);
    }
}
