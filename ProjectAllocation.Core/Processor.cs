using ProjectAllocation.Core.Models;
using ProjectAllocation.Core.Repositories;
using ProjectAllocation.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectAllocation.Core
{
    public class Processor
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IRandomNumberProvider _randomNumberProvider;

        public Processor(IStudentRepository studentRepository, IRandomNumberProvider randomNumberProvider)
        {
            _studentRepository = studentRepository;
            _randomNumberProvider = randomNumberProvider;
        }

        public Dictionary<Student, Project> Process(State state)
        {
            if (state.Students.Count < state.Supervisors.Sum(supervisor => supervisor.Capacity))
                throw new ArgumentException($"Not enough capacity for the student list provided. Capacity = {state.Supervisors.Sum(supervisor => supervisor.Capacity)}, Student count = {state.Students.Count}", nameof(state));

            while (!state.IsSolved)
            {
                // TODO: Assign based on preferences
                
                var projectsLeft = state.Supervisors
                    .Where(supervisor => supervisor.AllocatedCount < supervisor.Capacity)
                    .SelectMany(supervisor => supervisor.Projects)
                    .ToList();

                var studentsLeft = state.Students
                    .Where(student => !student.IsSolved);

                foreach (var student in studentsLeft)
                {
                    var projectIndex = _randomNumberProvider.NextInt(projectsLeft.Count);
                    var project = projectsLeft[projectIndex];

                    _studentRepository.AssignProject(student, project);

                    if ((project.Capacity.HasValue && project.AllocatedStudents.Count == project.Capacity)
                        || project.Supervisor.AllocatedCount == project.Supervisor.Capacity)
                    {
                        projectsLeft.Remove(project);
                    }
                }
            }

            return state.Students.ToDictionary(
                student => student,
                student => student.Project
            );
        }
    }
}
