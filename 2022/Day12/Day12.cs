using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2022.Day12
{
    public class Day12 : Day<char[,], long, long>
    {
        public override string DayNumber { get { return "12"; } }

        public override long PartOne(char[,] input)
        {
            HashSet<long> shortPaths = new HashSet<long>();

            Queue<((char, int, int), List<(int, int)>)> visited = new Queue<((char, int, int), List<(int, int)>)>();
            var start = input.GetCellsEqualToValue(Start).First();
            var end = input.GetCellsEqualToValue(End).First();
            visited.Enqueue(((start.Item1, start.Item2, start.Item3), new List<(int, int)>()));

            while (visited.Any())
            {
                var current = visited.Dequeue();
                var neighbors = input.GetNeighbors(current.Item1.Item2, current.Item1.Item3, false);
                if (current.Item1.Item1 == 'z' && neighbors.Any(r => r.Item1.Equals(End)))
                {
                    shortPaths.Add(current.Item2.Count + 1);
                    continue;
                }
                foreach (var n in neighbors)
                {
                    if ((current.Item1.Item1.Equals(Start) && n.Item1 == 'a') || (n.Item1 <= current.Item1.Item1 + 1 && (n.Item1 != Start || n.Item1 != End)))
                    {
                        if (!current.Item2.Contains((n.Item2, n.Item3)))
                        {
                            var temp = new List<(int, int)>(current.Item2);
                            temp.Add((current.Item1.Item2, current.Item1.Item3));
                            visited.Enqueue(((n.Item1, n.Item2, n.Item3), temp));
                        }
                    }
                }
            }
            return shortPaths.Min();
        }

        public override long PartTwo(char[,] input)
        {
            return 0;
        }

        public override char[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D();
        }

        private const char Start = 'S';
        private const char End = 'E';
    }
}
