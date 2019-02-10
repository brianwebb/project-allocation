using FluentAssertions;
using Moq;
using NUnit.Framework;
using ProjectAllocation.Core;
using ProjectAllocation.Core.Models;
using ProjectAllocation.Core.Repositories;
using ProjectAllocation.Core.Utilities;
using System;
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
                new Project(supervisors[0]) { Name = "some example project", Capacity = 1 },
                new Project(supervisors[1]) { Name = "some other example project", Capacity = 2 },
                new Project(supervisors[1]) { Name = "some third example project", Capacity = 1 }
            };
            _state = new State
            {
                Projects = projects,
                Supervisors = supervisors
            };

            _studentRepository
                .Setup(s => s.AssignProject(It.IsAny<Student>(), It.IsAny<Project>()))
                .Callback<Student, Project>((student, project) =>
                {
                    student.Project = project;
                    project.AllocatedStudents.Add(student);
                });
        }

        private Processor Processor() => new Processor(_studentRepository.Object, _randomNumberProvider.Object);

        [Test]
        public void ShouldBlowUpIfNotEnoughCapacityForStudents()
        {
            _state.Students.Add(new Student(null));
            _state.Students.Add(new Student(null));
            _state.Students.Add(new Student(null));
            _state.Students.Add(new Student(null));
            _state.Students.Add(new Student(null));
            _state.Students.Add(new Student(null));

            Processor()
                .Invoking(processor => processor.Process(_state))
                .Should().Throw<ArgumentException>();
        }

        [Test]
        public void ShouldAssignStudentsToPreferredProject()
        {
            _state.Students.Add(new Student(new List<Project> { _state.Projects[2] }));
            _state.Students.Add(new Student(new List<Project> { _state.Projects[1] }));
            _state.Students.Add(new Student(new List<Project> { _state.Projects[0] }));
            _state.Students.Add(new Student(new List<Project> { _state.Projects[1] }));

            var result = Processor().Process(_state);

            result[_state.Students[0]].Should().Be(_state.Projects[2]);
            result[_state.Students[1]].Should().Be(_state.Projects[1]);
            result[_state.Students[2]].Should().Be(_state.Projects[0]);
            result[_state.Students[3]].Should().Be(_state.Projects[1]);
        }

        [Test]
        public void ShouldRandomlyAssignStudentsIfNoneHavePreferences()
        {
            _state.Students = new List<Student>
            {
                new Student(null) { Id = "firststudentid" },
                new Student(null) { Id = "secondstudentid" },
                new Student(null) { Id = "thirdstudentid" },
                new Student(null) { Id = "fourthstudentid" }
            };

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