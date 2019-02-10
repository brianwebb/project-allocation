using System.Collections.Generic;

namespace ProjectAllocation.Core.Models
{
    public class Student
    {
        public string Id { get; set; }
        public decimal Gpa { get; set; }
        public List<Project> ProjectInterests { get; set; }
        public Project Project { get; set; }

        public bool HasProject => Project != null;

        public Student(List<Project> preferences)
        {
            ProjectInterests = preferences ?? new List<Project>();
        }
    }
}
