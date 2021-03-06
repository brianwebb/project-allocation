﻿using System.Collections.Generic;
using System.Linq;

namespace ProjectAllocation.Core.Models
{
    public class Supervisor
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public List<Project> Projects { get; set; } = new List<Project>();

        public bool HasSpaceRemaining => Projects.Sum(project => project.AllocatedStudents.Count) < Capacity;
    }
}
