package eeee.dgar;

import java.util.ArrayList;
import java.util.List;

public class Algos
{
    public static void BFS(Node start)
    {
        List<Node> queue = new ArrayList<>();
        List<Node> next = new ArrayList<>();

        queue.add(start);

        while (queue.size() > 0)
        {
            for (Node node: queue)
            {
                node.setIsVisited(true);

                for (Node child: node.children())
                {
                    if (!child.isVisited())
                    {
                        next.add(child);
                    }
                }
            }

            queue = next;
            next = new ArrayList<>();
        }
    }

    public static void DFS(Node start)
    {
        start.setIsVisited(true);

        for (Node child: start.children())
        {
            if (!child.isVisited())
            {
                DFS(child);
            }
        }
    }
}