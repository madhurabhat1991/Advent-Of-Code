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
            var h = Heights(input);

            var start = input.GetCellsEqualToValue(Start).First();
            var d = GetPaths(h, (start.Item2, start.Item3));

            var end = input.GetCellsEqualToValue(End).First();
            return d[end.Item2, end.Item3];
        }

        public override long PartTwo(char[,] input)
        {
            var h = Heights(input);

            var starts = h.GetCellsEqualToValue(0);
            var paths = new List<long>();
            foreach (var start in starts)
            {
                var d = GetPaths(h, (start.Item2, start.Item3));
                var end = input.GetCellsEqualToValue(End).First();
                paths.Add(d[end.Item2, end.Item3]);
            }
            return paths.Where(r => r != -1).Min();
        }

        public override char[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D();
        }

        private const char Start = 'S';
        private const char End = 'E';

        private int[,] Heights(char[,] input)
        {
            int[,] h = new int[input.GetLength(0), input.GetLength(1)];
            for (int row = 0; row < input.GetLength(0); row++)
            {
                for (int col = 0; col < input.GetLength(1); col++)
                {
                    var val = 0;
                    switch (input[row, col])
                    {
                        case Start:
                            val = 0;
                            break;
                        case End:
                            val = 'z' - 'a';
                            break;
                        default:
                            val = input[row, col] - 'a';
                            break;
                    }
                    h[row, col] = val;
                }
            }
            return h;
        }

        /// <summary>
        /// BFS to find the distance from a point
        /// </summary>
        /// <param name="input"></param>
        /// <param name="start">Starting point(row, col)</param>
        /// <returns></returns>
        public static long[,] GetPaths(int[,] input, (int, int) start)
        {
            // create a distance grid with unvisited node as -1
            var unvisited = -1;
            long[,] distances = new long[input.GetLength(0), input.GetLength(1)].MarkGrid(unvisited);
            distances[start.Item1, start.Item2] = 0;

            // queue to explore
            Queue<(int, int)> explore = new Queue<(int, int)>();
            explore.Enqueue((start.Item1, start.Item2));

            while (explore.Any())
            {
                var current = explore.Dequeue();
                var neighbors = input.GetNeighbors(current.Item1, current.Item2, false);
                foreach (var n in neighbors)
                {
                    // if the neighbor is unvisted and at atmost 1 from current then calc distance & explore
                    if (distances[n.Item2, n.Item3] == unvisited && n.Item1 <= input[current.Item1, current.Item2] + 1)
                    {
                        distances[n.Item2, n.Item3] = distances[current.Item1, current.Item2] + 1;
                        explore.Enqueue((n.Item2, n.Item3));
                    }
                }
            }
            return distances;
        }
    }
}
