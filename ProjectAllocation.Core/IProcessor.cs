using System.Collections.Generic;
using ProjectAllocation.Core.Models;

namespace ProjectAllocation.Core
{
    public interface IProcessor
    {
        Dictionary<Student, Project> AllocateProjects(State state);
    }
}