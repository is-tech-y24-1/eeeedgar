namespace GeneticAlgo.Shared.Models;

public class Trajectory
{
    private List<Point> _vectors;
    private Point _result;

    public Point Result => _result;

    public Trajectory(Trajectory? trajectory = null)
    {
        if (trajectory is null)
        {
            _vectors = new List<Point>();
            _result = new Point(0, 0);
        }
        else
        {
            _vectors = trajectory._vectors.ToList();
            _result = trajectory._result;
        }
        
    }

    public void AddVector(Point point)
    {
        _vectors.Add(point);
        _result.X += point.X;
        _result.Y += point.Y;
    }
}