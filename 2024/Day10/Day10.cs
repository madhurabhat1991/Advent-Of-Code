using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;

namespace _2024.Day10
{
    public class Day10 : Day<int[,], long, long>
    {
        public override string DayNumber { get { return "10"; } }

        public override long PartOne(int[,] input)
        {
            long sum = 0;
            List<(int, int, int)> trailheads = input.GetCellsEqualToValue(0);   // (value, row, column)
            foreach (var trailhead in trailheads)
            {
                Queue<(int, int, int)> q = new Queue<(int, int, int)>([trailhead]);
                HashSet<(int, int, int)> trailends = new HashSet<(int, int, int)>();
                while (q.Count > 0)
                {
                    var head = q.Dequeue();
                    var nexts = input.GetNeighbors(head.Item2, head.Item3, includeDiagonal: false).Where(r => r.Item1 == head.Item1 + 1).ToList();
                    foreach (var next in nexts)
                    {
                        if (next.Item1 == 9) { trailends.Add(next); }
                        else if (!q.Contains(next)) { q.Enqueue(next); }
                    }
                }
                sum += trailends.Count;
            }
            return sum;
        }

        public override long PartTwo(int[,] input)
        {
            long sum = 0;
            List<(int, int, int)> trailheads = input.GetCellsEqualToValue(0);   // (value, row, column)
            foreach (var trailhead in trailheads)
            {
                Queue<List<(int, int, int)>> q = new Queue<List<(int, int, int)>>([new List<(int, int, int)>() { trailhead }]);
                HashSet<List<(int, int, int)>> trailends = new HashSet<List<(int, int, int)>>();
                while (q.Count > 0)
                {
                    var path = q.Dequeue();
                    var head = path.Last();
                    var nexts = input.GetNeighbors(head.Item2, head.Item3, includeDiagonal: false).Where(r => r.Item1 == head.Item1 + 1).ToList();
                    foreach (var next in nexts)
                    {
                        var newPath = path.ToList();
                        newPath.Add(next);
                        if (next.Item1 == 9) { trailends.Add(newPath); }
                        else { q.Enqueue(newPath); }
                    }
                }
                sum += trailends.Count;
            }
            return sum;
        }

        public override int[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D().CharToIntGrid2D();
        }
    }
}
