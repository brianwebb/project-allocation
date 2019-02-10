using System.Collections.Generic;
using System.Linq;

namespace ProjectAllocation.Core.Models
{
    public class Project
    {
        public string Name { get; set; }
        public int? Capacity { get; set; }
        public Supervisor Supervisor { get; set; }
        public List<Student> InterestedStudents { get; set; } = new List<Student>();
        public List<Student> AllocatedStudents { get; set; } = new List<Student>();

        public List<Student> OrderedInterestedStudents => InterestedStudents
            .Where(student => !student.IsSolved)
            .OrderByDescending(student => student.Gpa).ToList();

        public bool IsSolved => !Capacity.HasValue || AllocatedStudents.Count <= Capacity;

        public Project(Supervisor supervisor)
        {
            Supervisor = supervisor;
            supervisor.Projects.Add(this);
        }
    }
}
