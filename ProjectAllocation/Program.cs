using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using ProjectAllocation.Core;
using ProjectAllocation.Core.Repositories;
using ProjectAllocation.Core.Utilities;
using System;

namespace ProjectAllocation
{
    class Program
    {
        static int Main(string[] args)
        {
            ILogger<Program> logger = null;
            try
            {
                var provider = new ServiceCollection()
                    .AddLogging(builder =>
                    {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddNLog();
                    })
                    .AddTransient<ICsvHelper, CsvHelper>()
                    .AddTransient<IStudentRepository, StudentRepository>()
                    .AddTransient<IRandomNumberProvider, RandomNumberProvider>()
                    .AddTransient<IProcessor, Processor>()
                    .AddTransient<Coordinator>()
                    .BuildServiceProvider();

                logger = provider.GetService<ILogger<Program>>();
                var coordinator = provider.GetService<Coordinator>();

                logger.LogInformation("Application started");
                
                var result = coordinator.Run(args);

                if (result != 0)
                    throw new Exception($"Coordinator returned <{result}> from run. Expected 0.");

                logger.LogInformation("Application ran successfully");

                return 0;
            }
            catch (Exception ex)
            {
                var exception = (ex as AggregateException)?.Flatten().InnerExceptions[0] ?? ex;
                if (logger != null)
                {
                    logger.LogCritical(exception, "Fatal exception detected. Exiting.");
                }
                else
                {
                    Console.Error.WriteLine(exception);
                }

                return 1;
            }
        }
    }
}
