using Microsoft.Extensions.Logging;
using ProjectAllocation.Core.Models;
using ProjectAllocation.Core.Repositories;
using ProjectAllocation.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectAllocation.Core
{
    public class Processor : IProcessor
    {
        private ILogger<Processor> _logger;
        private readonly IStudentRepository _studentRepository;
        private readonly IRandomNumberProvider _randomNumberProvider;

        public Processor(ILogger<Processor> logger, IStudentRepository studentRepository, IRandomNumberProvider randomNumberProvider)
        {
            _logger = logger;
            _studentRepository = studentRepository;
            _randomNumberProvider = randomNumberProvider;
        }

        public Dictionary<Student, Project> AllocateProjects(State state)
        {
            if (state.Students.Count > state.Supervisors.Sum(supervisor => supervisor.Capacity))
                throw new ArgumentException($"Not enough capacity for the student list provided. Capacity = '{state.Supervisors.Sum(supervisor => supervisor.Capacity)}', Student count = '{state.Students.Count}'", nameof(state));

            // Assign based on preferences. Highest GPA -> lowest
            // TODO: Group by GPA and pick randomly within group
            var studentsByGpa = state.Students
                .Where(student => student.ProjectInterests.Count > 0)
                .OrderByDescending(student => student.Gpa);

            _logger.LogInformation("Assigning students to projects based on their preferences, ordered by GPA.");

            foreach (var student in studentsByGpa)
            {
                _logger.LogTrace("Trying to assign a project to student '{0}'", student.Id);
                var project = student.ProjectInterests.FirstOrDefault(projectInterest => projectInterest.HasSpaceRemaining);

                if (project == null)
                {
                    _logger.LogInformation("None of student '{0}'s preferences have space remaining. Skipping for now.", student.Id);
                    continue;
                }

                _logger.LogTrace("Student '{0}'s preference #{1} is still available. Assigning...", student.Id, student.ProjectInterests.IndexOf(project) + 1);
                _studentRepository.AssignProject(student, project);
            }

            _logger.LogInformation("All student preferences assigned (where available)");

            // Randomly assign remaining students
            var projectsLeft = state.Projects
                .Where(project => project.HasSpaceRemaining)
                .ToList();
            var studentsLeft = state.Students
                .Where(student => !student.HasProject)
                .ToList();

            _logger.LogInformation("Assigning remaining {0} students a project at random.", studentsLeft.Count);

            foreach (var student in studentsLeft)
            {
                var project = projectsLeft[_randomNumberProvider.NextInt(projectsLeft.Count)];

                _studentRepository.AssignProject(student, project);

                if (!project.HasSpaceRemaining)
                {
                    projectsLeft.Remove(project);
                }
            }

            return state.Students.ToDictionary(
                student => student,
                student => student.Project
            );
        }
    }
}
