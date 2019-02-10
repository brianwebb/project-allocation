using System.Collections.Generic;

namespace ProjectAllocation.Core.Models
{
    public class Project
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public Supervisor Supervisor { get; set; }
        public List<Student> InterestedStudents { get; set; } = new List<Student>();
        public List<Student> AllocatedStudents { get; set; } = new List<Student>();

        public bool IsValid => AllocatedStudents.Count <= Capacity;
    }
}
