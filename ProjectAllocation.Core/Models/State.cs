using System.Collections.Generic;
using System.Linq;

namespace ProjectAllocation.Core.Models
{
    public class State
    {
        public List<Supervisor> Supervisors { get; set; }
        public List<Project> Projects { get; set; }
        public List<Student> Students { get; set; }

        public bool IsValid => Supervisors.All(supervisor => supervisor.IsValid)
            && Projects.All(project => project.IsValid)
            && Students.All(student => student.IsValid);
    }
}
