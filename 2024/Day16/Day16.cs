using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2024.Day16
{
    public class Day16 : Day<char[,], long, long>
    {
        public override string DayNumber { get { return "16"; } }

        public override long PartOne(char[,] input)
        {
            // Dijkstra, but the cost will be determined by the turns taken - need to turn this into Priority queue when I update to .NET 9 (offers remove)
            Dictionary<(int, int, char), (long, (int, int, char))> costs = new Dictionary<(int, int, char), (long, (int, int, char))>();
            HashSet<(int, int, char)> visited = new HashSet<(int, int, char)>();
            SortedSet<(long, (int, int, char))> queue = new SortedSet<(long, (int, int, char))>();

            // begin with start
            var sNode = input.GetCellsEqualToValue(Start).First();
            var start = (sNode.Item2, sNode.Item3, East);
            queue.Add((0, start));
            costs.Add(start, (0, start));

            while (queue.Count > 0)
            {
                // dequeue and visited
                var node = queue.First().Item2;
                queue.RemoveWhere(r => r.Item2 == node);
                visited.Add(node);
                // get neighbors with direction
                List<(char, int, int, char)> neighbors = new List<(char, int, int, char)>();
                switch (node.Item3)
                {
                    case North:
                        var n = input.GetTopCell(node.Item1, node.Item2);
                        if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, North)); }
                        n = input.GetRightCell(node.Item1, node.Item2);
                        if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, East)); }
                        n = input.GetLeftCell(node.Item1, node.Item2);
                        if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, West)); }
                        break;
                    case South:
                        n = input.GetBottomCell(node.Item1, node.Item2);
                        if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, South)); }
                        n = input.GetRightCell(node.Item1, node.Item2);
                        if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, East)); }
                        n = input.GetLeftCell(node.Item1, node.Item2);
                        if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, West)); }
                        break;
                    case East:
                        n = input.GetRightCell(node.Item1, node.Item2);
                        if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, East)); }
                        n = input.GetTopCell(node.Item1, node.Item2);
                        if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, North)); }
                        n = input.GetBottomCell(node.Item1, node.Item2);
                        if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, South)); }
                        break;
                    case West:
                        n = input.GetLeftCell(node.Item1, node.Item2);
                        if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, West)); }
                        n = input.GetTopCell(node.Item1, node.Item2);
                        if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, North)); }
                        n = input.GetBottomCell(node.Item1, node.Item2);
                        if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, South)); }
                        break;
                }
                foreach (var neighbor in neighbors)
                {
                    // if neighbor is not visited
                    if (!visited.Contains((neighbor.Item2, neighbor.Item3, neighbor.Item4)))
                    {
                        // add or update path cost
                        long newCost = costs[node].Item1 + (node.Item3 == neighbor.Item4 ? 1 : 1001);
                        (long, (int, int, char)) value;
                        if (costs.TryGetValue((neighbor.Item2, neighbor.Item3, neighbor.Item4), out value))
                        {
                            if (newCost < value.Item1)
                            {
                                value = (newCost, node);
                            }
                        }
                        else
                        {
                            value = (newCost, node);
                        }
                        costs[(neighbor.Item2, neighbor.Item3, neighbor.Item4)] = value;
                        // update priority queue
                        try
                        {
                            queue.RemoveWhere(r => r.Item2 == (neighbor.Item2, neighbor.Item3, neighbor.Item4));
                        }
                        finally
                        {
                            queue.Add((value.Item1, (neighbor.Item2, neighbor.Item3, neighbor.Item4)));
                        }
                    }
                }
            }
            // look for end node with least path cost
            var eNode = input.GetCellsEqualToValue(End).First();
            var end = (eNode.Item2, eNode.Item3);
            return costs.Where(r => r.Key.Item1 == end.Item1 && r.Key.Item2 == end.Item2).ToList().Min(r => r.Value).Item1;
        }

        public override long PartTwo(char[,] input)
        {
            throw new NotImplementedException();
        }

        public override char[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D();
        }

        private const char North = 'N';
        private const char South = 'S';
        private const char East = 'E';
        private const char West = 'W';

        private readonly char Start = 'S';
        private readonly char End = 'E';
        private readonly char Wall = '#';
        private readonly char Empty = '.';

        //public override long PartTwoNaive(char[,] input)
        //{
        //    // Naive approach by collecting all paths, doesn't complete
        //    // Dijkstra, but the cost will be determined by the turns taken - need to turn this into Priority queue when I update to .NET 9 (offers remove)
        //    List<(List<(int, int, char)>, long)> costs = new List<(List<(int, int, char)>, long)>();        // List<(path, cost)>
        //    Queue<(List<(int, int, char)>, long)> queue = new Queue<(List<(int, int, char)>, long)>();      // <(path, cost)>

        //    // begin with start
        //    var sNode = input.GetCellsEqualToValue(Start).First();
        //    var start = (sNode.Item2, sNode.Item3, East);
        //    queue.Enqueue((new List<(int, int, char)>() { start }, 0));

        //    while (queue.Count > 0)
        //    {
        //        // dequeue and visited
        //        var dq = queue.Dequeue();
        //        var node = dq.Item1.Last();
        //        // get neighbors with direction
        //        List<(char, int, int, char)> neighbors = new List<(char, int, int, char)>();
        //        switch (node.Item3)
        //        {
        //            case North:
        //                var n = input.GetTopCell(node.Item1, node.Item2);
        //                if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, North)); }
        //                n = input.GetRightCell(node.Item1, node.Item2);
        //                if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, East)); }
        //                n = input.GetLeftCell(node.Item1, node.Item2);
        //                if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, West)); }
        //                break;
        //            case South:
        //                n = input.GetBottomCell(node.Item1, node.Item2);
        //                if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, South)); }
        //                n = input.GetRightCell(node.Item1, node.Item2);
        //                if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, East)); }
        //                n = input.GetLeftCell(node.Item1, node.Item2);
        //                if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, West)); }
        //                break;
        //            case East:
        //                n = input.GetRightCell(node.Item1, node.Item2);
        //                if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, East)); }
        //                n = input.GetTopCell(node.Item1, node.Item2);
        //                if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, North)); }
        //                n = input.GetBottomCell(node.Item1, node.Item2);
        //                if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, South)); }
        //                break;
        //            case West:
        //                n = input.GetLeftCell(node.Item1, node.Item2);
        //                if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, West)); }
        //                n = input.GetTopCell(node.Item1, node.Item2);
        //                if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, North)); }
        //                n = input.GetBottomCell(node.Item1, node.Item2);
        //                if (n.Item1 == Empty || n.Item1 == End) { neighbors.Add((n.Item1, n.Item2, n.Item3, South)); }
        //                break;
        //        }
        //        foreach (var neighbor in neighbors)
        //        {
        //            // if neighbor is not visited
        //            if (!dq.Item1.Contains((neighbor.Item2, neighbor.Item3, neighbor.Item4)))
        //            {
        //                // add or update path cost
        //                long newCost = dq.Item2 + (node.Item3 == neighbor.Item4 ? 1 : 1001);
        //                var newPath = dq.Item1.ToList();
        //                newPath.Add((neighbor.Item2, neighbor.Item3, neighbor.Item4));
        //                if (neighbor.Item1 == End) { costs.Add((newPath, newCost)); }
        //                else { queue.Enqueue((newPath, newCost)); }
        //            }
        //        }
        //    }

        //    var bestPaths = costs.Where(r => r.Item2 == costs.Min(x => x.Item2)).ToList();
        //    var uniquePoints = bestPaths.SelectMany(r => r.Item1).Select(r => (r.Item1, r.Item2)).Distinct().Count();
        //    return uniquePoints;
        //}
    }
}
