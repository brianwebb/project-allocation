using System.Collections.Generic;

namespace ProjectAllocation.Core.Models
{
    public class Student
    {
        public string Id { get; set; }
        public List<Project> Preferences { get; set; } = new List<Project>();
        public Project Project { get; set; }

        public bool IsValid => Project != null;
    }
}
