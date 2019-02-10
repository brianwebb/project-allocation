using System.Collections.Generic;

namespace ProjectAllocation.Core.Models
{
    public class Student
    {
        public string Id { get; set; }
        public decimal Gpa { get; set; }
        public List<Project> Preferences { get; set; }
        public Project Project { get; set; }

        public bool IsSolved => Project != null;

        public Student(List<Project> preferences)
        {
            Preferences = preferences ?? new List<Project>();
        }
    }
}
