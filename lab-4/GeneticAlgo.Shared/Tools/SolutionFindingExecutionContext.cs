using GeneticAlgo.Shared.Models;

namespace GeneticAlgo.Shared.Tools;

public class SolutionFindingExecutionContext : IExecutionContext
{
    private readonly int _circleCount;
    private readonly int _pointCount;
    private readonly int _maximumValue;
    private readonly Random _random;
    
    private readonly BarrierCircle[] _barrierCircles;
    private Trajectory[] _trajectories;
    private GeneticMath _math;
    private int _iterationCounter;
    private readonly int _iterationLimit;
    private double _fitnessAccuracy; // 0.1
    private double _partOfBadTrajectoriesToReplace; // 0.2

    public SolutionFindingExecutionContext(int pointCount, int maximumValue, int circleCount, int iterationLimit, double fitnessAccuracy, double partOfBadTrajectoriesToReplace)
    {
        _pointCount = pointCount;
        _maximumValue = maximumValue;
        _circleCount = circleCount;
        _random = Random.Shared;
        _iterationCounter = 0;
        _iterationLimit = iterationLimit;
        _fitnessAccuracy = fitnessAccuracy;
        _partOfBadTrajectoriesToReplace = partOfBadTrajectoriesToReplace;

        _barrierCircles = Enumerable.Range(0, _circleCount)
            .Select(_ => 
                new BarrierCircle(
                    new Point(
                        3 + Next / 2, 3 + Next / 2
                    ), 
                    Next / 3
                )
            )
            .ToArray();
        
        _trajectories = Enumerable.Range(0, _pointCount)
            .Select(_ => new Trajectory())
            .ToArray();
        
        _math = new GeneticMath(_random, _barrierCircles, 0, 10, 0, 10);
    }
    
    private double Next => _random.NextDouble() * _random.Next(_maximumValue);

    public void Reset() { }

    public Task<IterationResult> ExecuteIterationAsync()
    {
        if (++_iterationCounter > _iterationLimit)
            return Task.FromResult(IterationResult.SolutionCannotBeFound);
        
        
        _trajectories = _trajectories.OrderBy(t => _math.Fitness(t.Result)).ToArray();

        if (_math.Fitness(_trajectories[0].Result) < _fitnessAccuracy)
            return Task.FromResult(IterationResult.SolutionFound);

        
        for (var i = 0; i < _trajectories.Length * _partOfBadTrajectoriesToReplace; i++)
        {
            _trajectories[^(i + 1)] = new Trajectory(_trajectories[i]);
        }

        for (var i = 1; i < _trajectories.Length; i++)
        {
            _math.Mutate(_trajectories[i]);
        }

        return Task.FromResult(IterationResult.IterationFinished);
    }

    public void ReportStatistics(IStatisticsConsumer statisticsConsumer)
    {
        var statistics = new Statistic[_pointCount];
        var i = 0;
        
        foreach (var trajectory in _trajectories)
            statistics[i] = new Statistic(i++, trajectory.Result, _math.Fitness(trajectory.Result));

        statisticsConsumer.Consume(statistics, _barrierCircles);
    }

    
}