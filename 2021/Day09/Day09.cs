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
                                FindNeighbors(input, nextBasin.Item1, nextBasin.Item2).ForEach(r => basins.Enqueue(r));
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
            var digit = input[row, col];
            bool lowPoint = true;
            if (lowPoint && row > 0 && digit >= input.GetTopElement(row, col).Item1) { lowPoint = false; }
            if (lowPoint && col < input.GetLength(1) - 1 && digit >= input.GetRightElement(row, col).Item1) { lowPoint = false; }
            if (lowPoint && row < input.GetLength(0) - 1 && digit >= input.GetBottomElement(row, col).Item1) { lowPoint = false; }
            if (lowPoint && col > 0 && digit >= input.GetLeftElement(row, col).Item1) { lowPoint = false; }
            return lowPoint;
        }

        private List<(int, int)> FindNeighbors(int[,] input, int row, int col)
        {
            List<(int, int)> basins = new List<(int, int)>();
            var digit = input[row, col];

            if (row > 0)
            {
                var nextElement = input.GetTopElement(row, col);
                if (nextElement.Item1 != 9) { basins.Add((nextElement.Item2, nextElement.Item3)); }
            }
            if (col < input.GetLength(1) - 1)
            {
                var nextElement = input.GetRightElement(row, col);
                if (nextElement.Item1 != 9) { basins.Add((nextElement.Item2, nextElement.Item3)); }
            }
            if (row < input.GetLength(0) - 1)
            {
                var nextElement = input.GetBottomElement(row, col);
                if (nextElement.Item1 != 9) { basins.Add((nextElement.Item2, nextElement.Item3)); }
            }
            if (col > 0)
            {
                var nextElement = input.GetLeftElement(row, col);
                if (nextElement.Item1 != 9) { basins.Add((nextElement.Item2, nextElement.Item3)); }
            }

            return basins;
        }
    }
}
