using CsvHelper;
using ProjectAllocation.Core.Models;
using ProjectAllocation.CsvModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectAllocation
{
    public class CsvHelper : ICsvHelper
    {
        public State BuildState(string supervisorsLocation, string projectsLocation, string studentsLocation)
        {
            IEnumerable<SupervisorCsv> supervisors;
            using (var reader = new StreamReader(supervisorsLocation))
            using (var csv = new CsvReader(reader))
            {
                supervisors = csv.GetRecords<SupervisorCsv>().ToList();
            }

            IEnumerable<ProjectCsv> projects;
            using (var reader = new StreamReader(projectsLocation))
            using (var csv = new CsvReader(reader))
            {
                projects = csv.GetRecords<ProjectCsv>().ToList();
            }

            IEnumerable<StudentCsv> students;
            using (var reader = new StreamReader(studentsLocation))
            using (var csv = new CsvReader(reader))
            {
                students = csv.GetRecords<StudentCsv>().ToList();
            }

            var state = new State();

            foreach (var supervisor in supervisors)
            {
                state.Supervisors.Add(new Supervisor
                {
                    Name = supervisor.Name.Trim(),
                    Capacity = supervisor.Capacity
                });
            }

            foreach (var project in projects)
            {
                var projectSupervisor = state.Supervisors.FirstOrDefault(supervisor => supervisor.Name == project.SupervisorName.Trim());

                if (projectSupervisor == null)
                    throw new ArgumentException($"Could not find supervisor '{project.SupervisorName}' for project '{project.Name}'", nameof(project));

                state.Projects.Add(new Project(projectSupervisor)
                {
                    Id = project.Id,
                    Capacity = project.Capacity,
                    Name = project.Name.Trim()
                });
            }

            foreach (var student in students)
            {
                var preferences = new List<Project>();

                if (!string.IsNullOrEmpty(student.Project1))
                {
                    preferences.Add(FindStateProject(state, student, student.Project1.Trim()));
                }
                if (!string.IsNullOrEmpty(student.Project2))
                {
                    preferences.Add(FindStateProject(state, student, student.Project2.Trim()));
                }
                if (!string.IsNullOrEmpty(student.Project3))
                {
                    preferences.Add(FindStateProject(state, student, student.Project3.Trim()));
                }
                if (!string.IsNullOrEmpty(student.Project4))
                {
                    preferences.Add(FindStateProject(state, student, student.Project4.Trim()));
                }
                if (!string.IsNullOrEmpty(student.Project5))
                {
                    preferences.Add(FindStateProject(state, student, student.Project5.Trim()));
                }

                state.Students.Add(new Student(preferences)
                {
                    Gpa = student.Gpa,
                    Id = student.Id                    
                });
            }

            return state;
        }

        private static Project FindStateProject(State state, StudentCsv student, string projectId)
        {
            var preferredProject = state.Projects.FirstOrDefault(project => project.Id == projectId);

            if (preferredProject == null)
                throw new ArgumentException($"Could not find project '{projectId}' for student '{student.Id}'");

            return preferredProject;
        }

        public void WriteOutput(string location, Dictionary<Student, Project> projectAllocations)
        {
            var records = projectAllocations
                .Select(projectAllocation => new StudentProjectCsv
                {
                    ProjectId = projectAllocation.Value.Id,
                    StudentId = projectAllocation.Key.Id
                });

            using (var writer = new StreamWriter(location))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(records);
            }
        }
    }
}
