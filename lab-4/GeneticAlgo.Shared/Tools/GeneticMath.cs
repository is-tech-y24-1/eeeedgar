using GeneticAlgo.Shared.Models;

namespace GeneticAlgo.Shared.Tools;

public class GeneticMath
{
    private readonly Random _random;
    private readonly BarrierCircle[] _barrierCircles;
    private readonly double _leftBarrier;
    private readonly double _rightBarrier;
    private readonly double _bottomBarrier;
    private readonly double _topBarrier;

    public GeneticMath(Random random, BarrierCircle[] barrierCircles, double leftBarrier, double rightBarrier, double bottomBarrier, double topBarrier)
    {
        _random = random;
        _barrierCircles = barrierCircles;
        _leftBarrier = leftBarrier;
        _rightBarrier = rightBarrier;
        _bottomBarrier = bottomBarrier;
        _topBarrier = topBarrier;
    }

    public double NextSigned => (_random.NextDouble() * 2 - 1);

    public void Mutate(Trajectory trajectory)
    {
        double moveX, moveY;
        Point point;
        do
        {
            moveX = NextSigned;
            moveY = NextSigned;
            point = new Point(moveX + trajectory.Result.X, moveY + trajectory.Result.Y);
        }
        while (
            !IsGoodPoint(point)
        );

        trajectory.AddVector(point);
    }

    public double Fitness(Point point)
    {
        return Math.Sqrt(Math.Pow(_rightBarrier - point.X, 2) + Math.Pow(_topBarrier - point.Y, 2));
    }

    public bool IsInCanvas(Point point)
    {
        return (point.X >= _leftBarrier
                && point.X <= _rightBarrier
                && point.Y >= _bottomBarrier
                && point.Y <= _topBarrier);
    }

    public bool IsInCircle(Point point, BarrierCircle[] barrierCircles)
    {
        foreach (var barrierCircle in barrierCircles)
        {
            if (Length(barrierCircle.Center, point) < barrierCircle.Radius)
                return true;
        }

        return false;
    }

    public double Length(Point a, Point b)
    {
        return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
    }

    public bool IsGoodPoint(Point point)
    {
        return IsInCanvas(point)
               &&
               !IsInCircle(point, _barrierCircles)
               ;
    }
}