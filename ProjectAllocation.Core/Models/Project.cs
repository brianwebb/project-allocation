using System.Collections.Generic;

namespace ProjectAllocation.Core.Models
{
    public class Project
    {
        public int? Capacity { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
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
