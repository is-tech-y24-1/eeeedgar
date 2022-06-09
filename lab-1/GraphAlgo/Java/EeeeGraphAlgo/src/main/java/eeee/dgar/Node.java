package eeee.dgar;

import java.util.ArrayList;
import java.util.List;

public class Node
{
    private int _id;
    private boolean _isVisited;
    private List<Node> _children;

    public Node(int id)
    {
        _id = id;
        _isVisited = false;
        _children = new ArrayList<>();
    }

    public int getId()
    {
        return _id;
    }

    public boolean isVisited()
    {
        return _isVisited;
    }

    public void setIsVisited(boolean isVisited)
    {
        _isVisited = isVisited;
    }

    public List<Node> children()
    {
        return _children;
    }
}