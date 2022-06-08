namespace GeneticAlgo.Shared.Models;

public class Trajectory
{
    private readonly Point[] _points;
    private int _pointCount;

    public Point Result => _points[_pointCount - 1];
    public double Fitness { get; set; }

    public int Length => _pointCount;

    public Point[] Points => _points;

    public Trajectory(int maxLength)
    {
        _points = new Point[maxLength];
        _points[0] = new Point(0, 0);
        _pointCount = 1;
    }

    public void AddPoint(Point point)
    {
        _points[_pointCount] = point;
        _pointCount++;
    }
}