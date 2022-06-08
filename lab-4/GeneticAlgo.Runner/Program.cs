using GeneticAlgo.Shared.Models;
using GeneticAlgo.Shared.Tools;

var pointCount = 50000;
var maximumValue = 10;
var circleCount = 5;
var iterationLimit = 1000;
var fitnessAccuracy = 0.05;
var partOfBadElementsToReplace = 0.1;

var executionContext = new SolutionFindingExecutionContext(pointCount, maximumValue, circleCount, iterationLimit, fitnessAccuracy, partOfBadElementsToReplace);


IterationResult result;
do
{
    result = await executionContext.ExecuteIterationAsync();
} while (result == IterationResult.IterationFinished);
    
Console.WriteLine(result);