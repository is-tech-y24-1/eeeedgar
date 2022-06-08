// See https://aka.ms/new-console-template for more information

using GeneticAlgo.Shared;
using GeneticAlgo.Shared.Tools;
using Serilog;

Logger.Init();
Log.Information("Start console polygon");
var dummyExecutionContext = new DummyExecutionContext(10, 10, 3);
dummyExecutionContext.Reset();
await dummyExecutionContext.ExecuteIterationAsync();
Log.Information("Polygon end");

Console.WriteLine("Hello, World!");
