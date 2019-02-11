using System.Collections.Generic;
using System.Linq;

namespace ProjectAllocation.Core.Models
{
    public class State
    {
        public List<Supervisor> Supervisors { get; set; } = new List<Supervisor>();
        public List<Project> Projects { get; set; } = new List<Project>();
        public List<Student> Students { get; set; } = new List<Student>();

        public bool IsSolved => Students.All(student => student.HasProject);
    }
}
