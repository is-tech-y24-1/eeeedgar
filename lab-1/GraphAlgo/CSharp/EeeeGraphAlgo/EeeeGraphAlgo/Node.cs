namespace EeeeGraphAlgo
{
    public class Node
    {
        public Node(int id)
        {
            Id = id;
            IsVisited = false;
            Children = new List<Node>();
        }

        public int Id { get; }
        
        public bool IsVisited { get; set; }
        
        public List<Node> Children { get; }
    }   
}