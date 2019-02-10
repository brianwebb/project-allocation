using ProjectAllocation.Core.Models;

namespace ProjectAllocation.Core.Repositories
{
    // TODO: When a database exists, this should update it
    public class StateRepository : IStudentRepository
    {
        public void AssignProject(Student student, Project project)
        {
            student.Project = project;
            project.AllocatedStudents.Add(student);
        }
    }
}
