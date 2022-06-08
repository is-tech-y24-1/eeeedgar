namespace GeneticAlgo.Shared.Models;

public class Trajectory
{
    private readonly List<Point> _points;
    private double _fitness;

    public Point Result => _points.Last();
    public double Fitness { get; set; }

    public Trajectory(Trajectory? trajectory = null)
    {
        if (trajectory is null)
        {
            _points = new List<Point>();
            _points.Add(new Point(0, 0));
        }
        else
        {
            _points = trajectory._points.ToList();
        }
        
    }

    public void AddVector(Point point)
    {
        _points.Add(point);
    }
}