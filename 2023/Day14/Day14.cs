using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2023.Day14
{
    public class Day14 : Day<char[,], long, long>
    {
        public override string DayNumber { get { return "14"; } }

        public override long PartOne(char[,] input)
        {
            var grid = Tilt(input);
            return CalculateLoad(grid);
        }

        public override long PartTwo(char[,] input)
        {
            Dictionary<string, (long, string)> gridDict = new Dictionary<string, (long, string)>();     // Dictionary<grid, (load, next grid)>
            var grid = input;
            string flatGrid = grid.FlattenGrid2D();
            string cycleGrid = "";
            var cycle = 0;
            Dictionary<long, long> cycleDict = new Dictionary<long, long>();
            while (true)
            {
                if (gridDict.TryGetValue(flatGrid, out (long, string) val))
                {
                    if (cycleGrid == "") { cycleGrid = flatGrid; }  // cycle begins
                    else if (cycleGrid == flatGrid) { break; }      // cycle ends
                    cycle++;
                    cycleDict.Add(cycle, val.Item1);
                    flatGrid = val.Item2;
                }
                else
                {
                    grid = Cycle(grid);
                    var newFlatGrid = grid.FlattenGrid2D();
                    var newLoad = CalculateLoad(grid);
                    gridDict.Add(flatGrid, (newLoad, newFlatGrid));
                    flatGrid = newFlatGrid;
                }
            }
            var rem = Cycles - (gridDict.Count - cycle);    // doesn't belong to cycle
            rem -= ((rem / cycle) * cycle);                 // remaining after full cycles
            return cycleDict[rem];
        }

        public override char[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D();
        }

        private const char RoundRock = 'O';
        private const char CubeRock = '#';
        private const char Space = '.';

        private const char North = 'N';
        private const char East = 'E';
        private const char South = 'S';
        private const char West = 'W';

        private const long Cycles = 1000000000;

        private char[,] Tilt(char[,] grid, char direction = North)
        {
            switch (direction)
            {
                case North:
                case South:
                    for (int c = 0; c < grid.GetLength(1); c++)
                    {
                        Queue<int> spaces = new Queue<int>();
                        for (int r = direction == North ? 0 : grid.GetLength(0) - 1;
                            direction == North ? r < grid.GetLength(0) : r >= 0;
                            r = direction == North ? r + 1 : r - 1)
                        {
                            var item = grid[r, c];
                            if (item == Space)
                            {
                                spaces.Enqueue(r);
                            }
                            else if (item == RoundRock)
                            {
                                if (spaces != null && spaces.Count > 0)
                                {
                                    grid[spaces.Dequeue(), c] = RoundRock;
                                    grid[r, c] = Space;
                                    spaces.Enqueue(r);
                                }
                            }
                            else if (item == CubeRock)
                            {
                                spaces.Clear();
                            }
                        }
                    }
                    break;
                case West:
                case East:
                    for (int r = 0; r < grid.GetLength(0); r++)
                    {
                        Queue<int> spaces = new Queue<int>();
                        for (int c = direction == West ? 0 : grid.GetLength(1) - 1;
                             direction == West ? c < grid.GetLength(1) : c >= 0;
                             c = direction == West ? c + 1 : c - 1)
                        {
                            var item = grid[r, c];
                            if (item == Space)
                            {
                                spaces.Enqueue(c);
                            }
                            else if (item == RoundRock)
                            {
                                if (spaces != null && spaces.Count > 0)
                                {
                                    grid[r, spaces.Dequeue()] = RoundRock;
                                    grid[r, c] = Space;
                                    spaces.Enqueue(c);
                                }
                            }
                            else if (item == CubeRock)
                            {
                                spaces.Clear();
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            return grid;
        }

        private long CalculateLoad(char[,] grid)
        {
            long sum = 0;
            for (int r = 0; r < grid.GetLength(0); r++)
            {
                int rocks = 0;
                for (int c = 0; c < grid.GetLength(1); c++)
                {
                    rocks += grid[r, c] == RoundRock ? 1 : 0;
                }
                sum += (rocks * (grid.GetLength(1) - r));
            }
            return sum;
        }

        private char[,] Cycle(char[,] grid)
        {
            foreach (var direction in new char[] { North, West, South, East })
            {
                grid = Tilt(grid, direction);
            }
            return grid;
        }
    }
}
