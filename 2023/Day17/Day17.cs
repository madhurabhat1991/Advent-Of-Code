using System;
using System.Collections.Generic;
using System.Text;
using Template;
using System.Linq;
using Helpers;

namespace _2023.Day17
{
    public class Day17 : Day<int[,], long, long>
    {
        public override string DayNumber { get { return "17"; } }

        public override long PartOne(int[,] input)
        {

            #region tries

            //var distances = GetShortestPathX2(input, (0, 0));
            //var path = new List<(int, int)>();
            //(int, int)  from = (input.GetLength(0) - 1, input.GetLength(1) - 1);
            //while (from != (0, 0))
            //{
            //    path.Add(from);
            //    from = distances[from].Item2;
            //}
            //var display = path.Select(r => (r.Item2, r.Item1)).ToList().MarkGrid('#', '.');
            //display.Print(false);
            //var last = distances[(input.GetLength(0) - 1, input.GetLength(1) - 1)].Item1;

            //var dij = input.Dijkstra((0, 0));
            //List<(int, int)> path = new List<(int, int)>();
            //(int, int) from = (input.GetLength(0) - 1, input.GetLength(1) - 1);
            //while (from != (0, 0))
            //{
            //    path.Add(from);
            //    from = dij[from].Item2;
            //}
            //var display = path.Select(r => (r.Item2,r.Item1)).ToList().MarkGrid('#', '.');
            //display.Print(false);
            //var last = dij[(input.GetLength(0) - 1, input.GetLength(1) - 1)].Item1;

            //var distances = GetShortestPathX(input, (0, 0));
            //path = new List<(int, int)>();
            //from = (input.GetLength(0) - 1, input.GetLength(1) - 1);
            //while (from != (0, 0))
            //{
            //    path.Add(from);
            //    from = distances[from].Item2;
            //}
            //display = path.Select(r => (r.Item2, r.Item1)).ToList().MarkGrid('#', '.');
            //display.Print(false);
            //last = distances[(input.GetLength(0) - 1, input.GetLength(1) - 1)].Item1;

            #endregion


            return 0;
        }

        public override long PartTwo(int[,] input)
        {
            return 0;
        }

        public override int[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D().CharToIntGrid2D();
        }

        //private Dictionary<(int, int), SortedSet<(long, (int, int), int)>> GetShortestPathX2(int[,] input, (int, int) start)
        //{
        //    // variation of Dijkstra - as you cannot move in a straight line for more than 3 blocks
        //    // Dictionary<node, (cost, prev node, prev cont)>
        //    Dictionary<(int, int), SortedSet<(long, (int, int), int)>> costs = new Dictionary<(int, int), SortedSet<(long, (int, int), int)>>();
        //    HashSet<(int, int)> visited = new HashSet<(int, int)>();
        //    Queue<((int, int), (int, int), long, int)> queue = new Queue<((int, int), (int, int), long, int)>();    //(neighbor, previous, prev cost, continuous)

        //    // begin with start
        //    queue.Enqueue((start, start, 0, 0));
        //    costs.Add(start, new SortedSet<(long, (int, int), int)>() { (0, start, 0) });

        //    while (queue.Any())
        //    {
        //        // dequeue and visited
        //        var f = queue.Dequeue();
        //        var node = f.Item1;
        //        var prev = f.Item2;
        //        var prevCost = costs[node].First(v => v.Item2 == prev).Item1;
        //        var continuous = f.Item4;
        //        visited.Add(node);

        //        // get neighbors
        //        var neighbors = input.GetNeighbors(node.Item1, node.Item2, false);
        //        if (continuous == 3) { neighbors.RemoveAll(r => r.Item2 == prev.Item1 || r.Item3 == prev.Item2); }
        //        foreach (var neighbor in neighbors)
        //        {
        //            // if neighbor is not visited
        //            if (!visited.Contains((neighbor.Item2, neighbor.Item3)))
        //            {
        //                var newCost = prevCost + neighbor.Item1;
        //                int newContinuous = (prev.Item1 == neighbor.Item2 || prev.Item2 == neighbor.Item3 ? continuous + 1 : 1);
        //                SortedSet<(long, (int, int), int)> value;
        //                if (costs.ContainsKey((neighbor.Item2, neighbor.Item3)))
        //                {
        //                    value = costs[(neighbor.Item2, neighbor.Item3)];
        //                    if (value.Where(v => v.Item2 == node).Any())
        //                    {
        //                        var v = value.First(r => r.Item2 == node);
        //                        if (newCost < v.Item1)
        //                        {
        //                            value.RemoveWhere(r => r.Item2 == node);
        //                            v.Item1 = newCost;
        //                            v.Item3 = newContinuous;
        //                            value.Add(v);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        value.Add((newCost, node, newContinuous));
        //                    }
        //                }
        //                else
        //                {
        //                    value = new SortedSet<(long, (int, int), int)>() { (newCost, node, newContinuous) };
        //                }
        //                costs[(neighbor.Item2, neighbor.Item3)] = value;
        //                queue.Enqueue(((neighbor.Item2, neighbor.Item3), node, newCost, newContinuous));
        //            }
        //        }
        //    }
        //    var check = costs[(input.GetLength(0) - 1, input.GetLength(1) - 1)];
        //    return costs;
        //}

        //private void PathNode()
        //{
        //    List<Path> paths = new List<Path>();
        //    Node start = new Node(0, 0, 0);
        //    Path path = new Path { Nodes = new List<Node> { start } };
        //    Queue<Path> queue = new Queue<Path>(new List<Path> { path });
        //    while (queue.Any())
        //    {
        //        path = queue.Dequeue();
        //        var lastTwo = path.Nodes.TakeLast(2);
        //        var last = lastTwo.Last();
        //        var prev = lastTwo.Count() > 1 ? lastTwo.First() : lastTwo.Last();
        //        var neighbors = input.GetNeighbors(last.Row, last.Column, false);
        //        if (last.Count == 3) { neighbors.RemoveAll(r => r.Item2 == prev.Row || r.Item3 == prev.Column); }
        //        foreach (var n in neighbors)
        //        {
        //            Node newNode = new Node(n.Item2, n.Item3, n.Item1);
        //            if (!path.ContainsNode(newNode.Row, newNode.Column))
        //            {
        //                newNode.Count = prev.Row == n.Item2 || prev.Column == n.Item3 ? last.Count + 1 : 1;
        //                Path newPath = path.DeepClone();
        //                newPath.Nodes.Add(newNode);
        //                newPath.Cost += newNode.Cost;
        //                if (newNode.IsDestination(input.GetLength(0) - 1, input.GetLength(1)))
        //                {
        //                    paths.Add(newPath);
        //                }
        //                else
        //                {
        //                    queue.Enqueue(newPath);
        //                }
        //            }
        //        }
        //    }
        //}

        //private Dictionary<(int, int), List<(long, (int, int), int)>> GetShortestPathX(int[,] input, (int, int) start)
        //{
        //    // variation of Dijkstra - as you cannot move in a straight line for more than 3 blocks
        //    // Dictionary<node, (cost, prev node, prev cont)>
        //    Dictionary<(int, int), List<(long, (int, int), int)>> costs = new Dictionary<(int, int), List<(long, (int, int), int)>>();
        //    HashSet<(int, int)> visited = new HashSet<(int, int)>();
        //    SortedSet<(long, (int, int), (int, int), int)> queue = new SortedSet<(long, (int, int), (int, int), int)>();  // (newCost, neighbor, previous, continuous)

        //    // begin with start
        //    queue.Add((0, start, start, 0));
        //    costs.Add(start, new List<(long, (int, int), int)>() { (0, start, 0) });

        //    while (queue.Count > 0)
        //    {
        //        // dequeue and visited
        //        var f = queue.First();
        //        var node = f.Item2;
        //        var prev = f.Item3;
        //        var continuous = f.Item4;
        //        queue.RemoveWhere(r => r.Item2 == node && r.Item3 == prev && r.Item4 == continuous);
        //        visited.Add(node);

        //        // get neighbors
        //        var neighbors = input.GetNeighbors(node.Item1, node.Item2, false);
        //        if (continuous == 3) { neighbors.RemoveAll(r => r.Item2 == prev.Item1 || r.Item3 == prev.Item2); }
        //        foreach (var neighbor in neighbors)
        //        {
        //            // if neighbor is not visited
        //            if (!visited.Contains((neighbor.Item2, neighbor.Item3)))
        //            {
        //                // add or update path cost
        //                var val = costs[node].Where(v => v.Item2 == prev).First();
        //                long newCost = val.Item1 + neighbor.Item1;
        //                int newContinuous = (prev.Item1 == neighbor.Item2 || prev.Item2 == neighbor.Item3 ? continuous + 1 : 1);
        //                if (costs.TryGetValue((neighbor.Item2, neighbor.Item3), out  List<(long, (int, int), int)> value))
        //                {
        //                    //if (newCost < val.Item1)
        //                    //{
        //                    //    value = (newCost, node, newContinuous);
        //                    //}
        //                    value.Add((newCost, node, newContinuous));
        //                }
        //                else
        //                {
        //                    value = new List<(long, (int, int), int)>() { (newCost, node, newContinuous) };
        //                }
        //                costs[(neighbor.Item2, neighbor.Item3)] = value;
        //                // update priority queue
        //                //try
        //                //{
        //                //    queue.RemoveWhere(r => r.Item2 == (neighbor.Item2, neighbor.Item3) && r.Item3 == value.Item2 && r.Item4 == value.Item3);
        //                //}
        //                //finally
        //                //{
        //                //    queue.Add((value.Item1, (neighbor.Item2, neighbor.Item3), value.Item2, value.Item3));
        //                //}
        //                queue.Add((newCost, (neighbor.Item2, neighbor.Item3), node, newContinuous));
        //            }
        //        }
        //    }
        //    return costs;
        //}
    }

    [Serializable]
    public class Path
    {
        public List<Node> Nodes { get; set; }
        public long Cost { get; set; }
        public bool ContainsNode(int row, int col)
        {
            return Nodes.Any(r => r.Row == row && r.Column == col);
        }
    }

    [Serializable]
    public class Node
    {
        public Node(int row, int col, int cost)
        {
            Row = row;
            Column = col;
            Cost = cost;
        }
        public int Row { get; set; }
        public int Column { get; set; }
        public int Cost { get; set; }
        public int Count { get; set; }
        public bool IsDestination(int lastRow, int lastCol)
        {
            return Row == lastRow && Column == lastCol;
        }
    }
}
