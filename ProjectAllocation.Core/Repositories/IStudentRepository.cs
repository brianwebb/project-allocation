using ProjectAllocation.Core.Models;

namespace ProjectAllocation.Core.Repositories
{
    public interface IStudentRepository
    {
        void AssignProject(Student student, Project project);
    }
}