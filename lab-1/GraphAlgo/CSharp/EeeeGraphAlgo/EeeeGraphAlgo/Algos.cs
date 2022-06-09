namespace EeeeGraphAlgo
{
    public class Algos
    {
        public static void BFS(Node start)
        {
            var queue = new List<Node>();
            var next = new List<Node>();
            
            queue.Add(start);

            while (queue.Count > 0)
            {
                foreach (var node in queue)
                {
                    node.IsVisited = true;
                    next.AddRange(node.Children.Where(child => !child.IsVisited));
                }

                queue = next;
                next = new List<Node>();
            }
        }

        public static void DFS(Node start)
        {
            start.IsVisited = true;

            foreach (var child in start.Children.Where(child => !child.IsVisited))
            {
                DFS(child);
            }
        }
    }
}