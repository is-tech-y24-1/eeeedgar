using GeneticAlgo.Shared.Models;

namespace GeneticAlgo.Shared.Tools;

public class DummyExecutionContext : IExecutionContext
{
    private readonly int _circleCount;
    private readonly int _pointCount;
    private readonly int _maximumValue;
    private readonly Random _random;

    public DummyExecutionContext(int pointCount, int maximumValue, int circleCount)
    {
        _pointCount = pointCount;
        _maximumValue = maximumValue;
        _circleCount = circleCount;
        _random = Random.Shared;
    }
    
    private double Next => _random.NextDouble() * _random.Next(_maximumValue);

    public void Reset() { }

    public Task<IterationResult> ExecuteIterationAsync()
    {
        return Task.FromResult(IterationResult.IterationFinished);
    }

    public void ReportStatistics(IStatisticsConsumer statisticsConsumer)
    {
        Statistic[] statistics = Enumerable.Range(0, _pointCount)
            .Select(i => new Statistic(i, new Point(Next, Next), Next))
            .ToArray();

        BarrierCircle[] circles = Enumerable.Range(0, _circleCount)
            .Select(_ => new BarrierCircle(new Point(Next, Next), 10 * Next))
            .ToArray();

        statisticsConsumer.Consume(statistics, circles);
    }
}