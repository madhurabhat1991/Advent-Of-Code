using Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Template;

namespace _2025.Day04
{
    public class Day04 : Day<char[,], long, long>
    {
        public override string DayNumber { get { return "04"; } }

        public override long PartOne(char[,] input)
        {
            long count = 0;
            for (int r = 0; r < input.GetLength(0); r++)
            {
                for (int c = 0; c < input.GetLength(1); c++)
                {
                    if (input[r, c] == Roll)
                    {
                        var neighbors = input.GetNeighbors(r, c, includeDiagonal: true);
                        if (neighbors.Count(n => n.Item1 == Roll) < 4)
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        public override long PartTwo(char[,] input)
        {
            long count = 0;
            char[,] grid = (char[,])input.Clone();
            while (true)
            {
                //grid.Print(false);
                List<(int, int)> remove = new List<(int, int)>();
                for (int r = 0; r < grid.GetLength(0); r++)
                {
                    for (int c = 0; c < grid.GetLength(1); c++)
                    {
                        if (grid[r, c] == Roll)
                        {
                            var neighbors = grid.GetNeighbors(r, c, includeDiagonal: true);
                            if (neighbors.Count(n => n.Item1 == Roll) < 4)
                            {
                                count++;
                                remove.Add((r, c));
                            }
                        }
                    }
                }
                if (remove.Count == 0) { break; }
                grid = grid.SetCellsToValue(remove, Empty);
            }
            return count;
        }

        public override char[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D();
        }

        private const char Roll = '@';
        private const char Empty = '.';
    }
}
