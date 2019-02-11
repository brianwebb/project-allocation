using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using ProjectAllocation.Core;
using System;

namespace ProjectAllocation
{
    public class Coordinator
    {
        private const string DefaultSupervisorsFileLocation = "supervisors.csv";
        private const string DefaultProjectsFileLocation = "projects.csv";
        private const string DefaultStudentsFileLocation = "students.csv";
        private const string DefaultOutputFileLocation = "output.csv";

        private readonly ILogger<Coordinator> _logger;
        private readonly ICsvHelper _csvHelper;
        private readonly IProcessor _processor;

        public Coordinator(ILogger<Coordinator> logger, ICsvHelper csvHelper, IProcessor processor)
        {
            _logger = logger;
            _csvHelper = csvHelper;
            _processor = processor;
        }

        public int Run(string[] args)
        {
            var commandLineApp = new CommandLineApplication(throwOnUnexpectedArg: false);

            commandLineApp.HelpOption("-?|--help");

            var supervisorsOpt = commandLineApp.Option("-su|--supervisors <SUPERVISORS_FILE>", $"Override the location of the supervisors file. Default = {DefaultSupervisorsFileLocation}", CommandOptionType.SingleValue);
            var projectsOpt = commandLineApp.Option("-pr|--projects <PROJECTS_FILE>", $"Override the location of the projects file. Default = {DefaultProjectsFileLocation}", CommandOptionType.SingleValue);
            var studentsOpt = commandLineApp.Option("-st|--students <STUDENTS_FILE>", $"Override the location of the students file. Default = {DefaultStudentsFileLocation}", CommandOptionType.SingleValue);
            var outputOpt = commandLineApp.Option("-out|--output <OUTPUT_FILE>", $"Override the location of the output file. Default = {DefaultOutputFileLocation}", CommandOptionType.SingleValue);

            commandLineApp.OnExecute(() =>
            {
                var supervisorsLocation = supervisorsOpt.HasValue() ? supervisorsOpt.Value() : DefaultSupervisorsFileLocation;
                var projectsLocation = projectsOpt.HasValue() ? projectsOpt.Value() : DefaultProjectsFileLocation;
                var studentsLocation = studentsOpt.HasValue() ? studentsOpt.Value() : DefaultStudentsFileLocation;
                var outputLocation = outputOpt.HasValue() ? outputOpt.Value() : DefaultOutputFileLocation;

                var state = _csvHelper.BuildState(supervisorsLocation, projectsLocation, studentsLocation);
                var projectAllocations = _processor.AllocateProjects(state);
                _csvHelper.WriteOutput(outputLocation, projectAllocations);

                return 0;
            });

            try
            {
                try
                {
                    return commandLineApp.Execute(args);
                }
                catch (AggregateException ex)
                {
                    if (ex.Flatten().InnerExceptions[0] is CommandParsingException parsingException)
                    {
                        throw parsingException;
                    }

                    throw;
                }
            }
            catch (CommandParsingException ex)
            {
                _logger.LogWarning(ex.Message);
                commandLineApp.ShowHelp(ex.Command.Name);
                throw;
            }
        }
    }
}
