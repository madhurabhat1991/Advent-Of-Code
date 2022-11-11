using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2021.Day09
{
    public class Day09 : Day<int[,], long, long>
    {
        public override string DayNumber { get { return "09"; } }

        public override long PartOne(int[,] input)
        {
            long lowPointsSum = 0;

            for (int row = 0; row < input.GetLength(0); row++)
            {
                for (int col = 0; col < input.GetLength(1); col++)
                {
                    if (LowPoint(input, row, col)) { lowPointsSum += input[row, col] + 1; }
                }
            }

            return lowPointsSum;
        }

        public override long PartTwo(int[,] input)
        {
            List<long> basinCounts = new List<long>();

            for (int row = 0; row < input.GetLength(0); row++)
            {
                for (int col = 0; col < input.GetLength(1); col++)
                {
                    if (LowPoint(input, row, col))
                    {
                        Queue<(int, int)> basins = new Queue<(int, int)>();
                        basins.Enqueue((row, col));
                        HashSet<(int, int)> visited = new HashSet<(int, int)>();
                        do
                        {
                            var nextBasin = basins.Dequeue();
                            if (!visited.Contains(nextBasin))
                            {
                                input.FindNeighbors(nextBasin.Item1, nextBasin.Item2, false)
                                    .Where(r => r.Item1 != 9)
                                    .Select(r => (r.Item2, r.Item3))
                                    .ToList()
                                    .ForEach(r => basins.Enqueue(r));
                                visited.Add(nextBasin);
                            }
                        } while (basins.Any());
                        basinCounts.Add(visited.Count);
                    }
                }
            }
            basinCounts.Sort();
            return basinCounts.TakeLast(3).ToList().Aggregate((a, x) => a * x);
        }

        public override int[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D().CharToIntGrid2D();
        }

        private bool LowPoint(int[,] input, int row, int col)
        {
            bool lowPoint = true;
            var neighbors = input.FindNeighbors(row, col, false);
            if (neighbors.Where(r => input[row, col] >= r.Item1).Any()) { lowPoint = false; }
            return lowPoint;
        }
    }
}
