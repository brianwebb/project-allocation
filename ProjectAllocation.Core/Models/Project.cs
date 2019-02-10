using System.Collections.Generic;
using System.Linq;

namespace ProjectAllocation.Core.Models
{
    public class Project
    {
        public string Name { get; set; }
        public int? Capacity { get; set; }
        public Supervisor Supervisor { get; set; }
        public List<Student> AllocatedStudents { get; set; } = new List<Student>();

        public bool HasSpaceRemaining => Supervisor.HasSpaceRemaining && (!Capacity.HasValue || AllocatedStudents.Count < Capacity);

        public Project(Supervisor supervisor)
        {
            Supervisor = supervisor;
            supervisor.Projects.Add(this);
        }
    }
}
