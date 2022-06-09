package eee.edgar;


import eeee.dgar.*;


public class App 
{
    public static void main( String[] args )
    {
        Node node1 = new Node(0);
        Node node2 = new Node(1);

        System.out.println(node1.isVisited());
        System.out.println(node2.isVisited());

        node1.children().add(node2);

        Algos.BFS(node1);

        System.out.println(node1.isVisited());
        System.out.println(node2.isVisited());


        Node node3 = new Node(2);
        Node node4 = new Node(3);

        System.out.println(node3.isVisited());
        System.out.println(node4.isVisited());

        node3.children().add(node4);

        Algos.DFS(node3);

        System.out.println(node3.isVisited());
        System.out.println(node4.isVisited());
    }
}