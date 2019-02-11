# Project Allocation

A tool for allocating projects to a set of students based on Supervisor + Project capacity and Student's preferences + GPA

## Building the code

1. Download + install [dotnet core 2.1+](https://dotnet.microsoft.com/download)
2. Run `dotnet build`

## Running the tests

1. Run `dotnet test`

## Publishing the app

1. Run `dotnet publish --configuration Release --self-contained --runtime osx.10.12-x64`

   More publishing options on [Microsoft's site](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish?tabs=netcore21)

   [Available runtimes](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog)

## Usage

```
Usage:  [options]

Options:
  -?|--help                             Show help information
  -su|--supervisors <SUPERVISORS_FILE>  Override the location of the supervisors file. Default = supervisors.csv
  -pr|--projects <PROJECTS_FILE>        Override the location of the projects file. Default = projects.csv
  -st|--students <STUDENTS_FILE>        Override the location of the students file. Default = students.csv
  -out|--output <OUTPUT_FILE>           Override the location of the output file. Default = output.csv
```

### *supervisors.csv*

expects a .csv file in the following format
```
Name,Capacity
Tom,2
Jerry,5
```

### *projects.csv*

expects a .csv file in the following format (note that Capacity can be left empty for projects with no particular restrictions)
```
SupervisorName,Name,Id,Capacity
Tom,relax on the couch all day,1,3
Jerry,try to frame the cat for stealing cheese,2,
Tom,chase the cat,3,1
```

### *students.csv*

expects a .csv file in the following format (all Project columns can be left empty. Currently supports up to 5 project choices)
```
Id,Gpa,Project1,Project2,Project3,Project4,Project5
1001,90,1,2,,,
1002,92,2,1,3,,
```

### *output.csv*

a .csv file in the following format
```
StudentId,ProjectId
1001,1
1002,2
```

The locations can all be overridden however by default it will look (and put) wherever you're running it from.
