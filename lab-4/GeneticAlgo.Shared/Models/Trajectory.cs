﻿namespace GeneticAlgo.Shared.Models;

public class Trajectory
{
    private readonly Point[] _points;

    public Point Result => _points[Length - 1];
    public double Fitness { get; set; }

    public int Length { get; set; }

    public Point[] Points => _points;

    public Trajectory(int maxLength)
    {
        _points = new Point[maxLength];
        _points[0] = new Point(0, 0);
        Length = 1;
    }

    public void AddPoint(Point point)
    {
        _points[Length++] = point;
    }
}