using ProjectAllocation.Core.Models;

namespace ProjectAllocation.Core.Repositories
{
    public interface ISupervisorRepository
    {
        void AddProject(Supervisor supervisor, Project project);
    }
}