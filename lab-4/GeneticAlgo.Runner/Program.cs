using GeneticAlgo.Shared.Models;
using GeneticAlgo.Shared.Tools;

var pointCount = 50000;
var maximumValue = 10;
var circleCount = 5;
var iterationLimit = 1000;
var fitnessAccuracy = 0.05;
var partOfBadElementsToReplace = 0.1;

var executionContext = new SolutionFindingExecutionContext(pointCount, maximumValue, circleCount, iterationLimit, fitnessAccuracy, partOfBadElementsToReplace);


var result = IterationResult.IterationFinished;
var iterationCounter = 0;
while (result != IterationResult.SolutionFound && iterationCounter++ < iterationLimit)
    result = await executionContext.ExecuteIterationAsync();
    
Console.WriteLine(result);