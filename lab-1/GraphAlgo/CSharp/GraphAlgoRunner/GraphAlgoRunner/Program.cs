using System;
using EeeeGraphAlgo;

var node1 = new Node(0);
var node2 = new Node(1);

node1.Children.Add(node2);

Console.WriteLine(node1.IsVisited);
Console.WriteLine(node2.IsVisited);

Algos.BFS(node1);

Console.WriteLine(node1.IsVisited);
Console.WriteLine(node2.IsVisited);


var node3 = new Node(2);
var node4 = new Node(3);

node3.Children.Add(node4);

Console.WriteLine(node3.IsVisited);
Console.WriteLine(node4.IsVisited);

Algos.DFS(node3);

Console.WriteLine(node3.IsVisited);
Console.WriteLine(node4.IsVisited);