using Microsoft.Extensions.Logging;
using ProjectAllocation.Core.Models;

namespace ProjectAllocation.Core.Repositories
{
    // TODO: When a database exists, this should update it
    public class StudentRepository : IStudentRepository
    {
        private readonly ILogger<StudentRepository> _logger;

        public StudentRepository(ILogger<StudentRepository> logger)
        {
            _logger = logger;
        }

        public void AssignProject(Student student, Project project)
        {
            _logger.LogInformation("Assigning student '{0}' to project '{1}'", student.Id, project.Id);
            student.Project = project;
            project.AllocatedStudents.Add(student);
        }
    }
}
