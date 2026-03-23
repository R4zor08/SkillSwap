// Student.cs
// Domain model representing a student/user in the SkillSwap system
// Contains student information, credentials, and their talents

using System;
using System.Collections.Generic;

namespace SkillSwap.Models
{
    /// <summary>
    /// Represents a student user in the SkillSwap application
    /// Stores personal information, authentication credentials, and skills/talents
    /// </summary>
    public class Student
    {
        // Private backing fields for encapsulation
        private Guid _studentId;      // Unique identifier for the student
        private string _name;         // Student's full name
        private string _email;        // Student's email address (unique)
        private string _password;     // Hashed password (BCrypt)

        /// <summary>
        /// Public constructor for creating new students
        /// Automatically generates a new Guid for StudentId
        /// </summary>
        public Student(string name, string email)
        {
            _studentId = Guid.NewGuid();
            _name = name;
            _email = email;
            Talents = new List<Talent>();
        }

        /// <summary>
        /// Internal constructor used by DataStore when loading from database
        /// Preserves existing ID and password hash from storage
        /// </summary>
        internal Student(Guid id, string name, string email, string password)
        {
            _studentId = id;
            _name = name;
            _email = email;
            _password = password;
            Talents = new List<Talent>();
        }

        /// <summary>
        /// Student's hashed password
        /// Should only contain BCrypt hashed values, never plain text
        /// </summary>
        public string Password
        {
            get => _password;
            set => _password = value;
        }

        /// <summary>
        /// Unique identifier for the student (read-only)
        /// </summary>
        public Guid StudentId { get => _studentId; }

        /// <summary>
        /// Student's full name
        /// Validates: cannot be empty or whitespace
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Name cannot be empty");
                _name = value;
            }
        }

        /// <summary>
        /// Student's email address
        /// Validates: cannot be empty or whitespace
        /// </summary>
        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Email cannot be empty");
                _email = value;
            }
        }

        /// <summary>
        /// Navigation property: collection of student's skills/talents
        /// </summary>
        public List<Talent> Talents { get; }

        /// <summary>
        /// Returns formatted string representation of the student
        /// </summary>
        public override string ToString() => $"{Name} ({Email})";
    }
}
