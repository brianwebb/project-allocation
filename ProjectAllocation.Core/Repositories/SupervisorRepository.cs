using ProjectAllocation.Core.Models;

namespace ProjectAllocation.Core.Repositories
{
    // TODO: When a database exists, this should update it
    public class SupervisorRepository : ISupervisorRepository
    {
        public void AddProject(Supervisor supervisor, Project project)
        {
            supervisor.Projects.Add(project);
            project.Supervisor = supervisor;
        }
    }
}
