using FluentAssertions;
using Moq;
using NUnit.Framework;
using ProjectAllocation.Core;
using ProjectAllocation.Core.Models;
using ProjectAllocation.Core.Repositories;
using ProjectAllocation.Core.Utilities;
using System.Collections.Generic;

namespace Tests
{
    public class ProcessorTests
    {
        private Mock<IStudentRepository> _studentRepository;
        private Mock<IRandomNumberProvider> _randomNumberProvider;

        private State _state;

        [SetUp]
        public void SetUp()
        {
            _studentRepository = new Mock<IStudentRepository>();
            _randomNumberProvider = new Mock<IRandomNumberProvider>();

            var supervisors = new List<Supervisor>
            {
                new Supervisor { Name = "supervisor 1", Capacity = 1 },
                new Supervisor { Name = "supervisor 2", Capacity = 3 }
            };
            var projects = new List<Project>
            {
                new Project { Name = "some example project", Capacity = 1, Supervisor = supervisors[0] },
                new Project { Name = "some other example project", Capacity = 2, Supervisor = supervisors[1] },
                new Project { Name = "some third example project", Capacity = 1, Supervisor = supervisors[1] }
            };
            supervisors[0].Projects.Add(projects[0]);
            supervisors[1].Projects.Add(projects[1]);
            supervisors[1].Projects.Add(projects[2]);
            var students = new List<Student>
            {
                new Student { Id = "firststudentid" },
                new Student { Id = "secondstudentid" },
                new Student { Id = "thirdstudentid" },
                new Student { Id = "fourthstudentid" }
            };

            _state = new State
            {
                Projects = projects,
                Students = students,
                Supervisors = supervisors
            };
        }

        private Processor Processor() => new Processor(_studentRepository.Object, _randomNumberProvider.Object);

        [Test]
        public void ShouldRandomlyAssignStudentsIfNoneHavePreferences()
        {
            _studentRepository
                .Setup(s => s.AssignProject(It.IsAny<Student>(), It.IsAny<Project>()))
                .Callback<Student, Project>((student, project) =>
                {
                    student.Project = project;
                    project.AllocatedStudents.Add(student);
                });
            // This is the default anyway. Just being explicit. Always just give them the first project that's left.
            _randomNumberProvider
                .Setup(r => r.NextInt(It.IsAny<int>()))
                .Returns(0);

            var result = Processor().Process(_state);

            result[_state.Students[0]].Should().Be(_state.Projects[0]);
            result[_state.Students[1]].Should().Be(_state.Projects[1]);
            result[_state.Students[2]].Should().Be(_state.Projects[1]);
            result[_state.Students[3]].Should().Be(_state.Projects[2]);
        }
    }
}