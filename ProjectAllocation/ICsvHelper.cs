using ProjectAllocation.Core.Models;
using System.Collections.Generic;

namespace ProjectAllocation
{
    public interface ICsvHelper
    {
        State BuildState(string supervisorsLocation, string projectsLocation, string studentsLocation);
        void WriteOutput(string location, Dictionary<Student, Project> allocations);
    }
}