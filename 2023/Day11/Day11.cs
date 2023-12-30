using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2023.Day11
{
    public class Day11 : Day<char[,], long, long>
    {
        public override string DayNumber { get { return "11"; } }

        public override long PartOne(char[,] input)
        {
            var expandRC = FindRowsAndColsToExpand(input);
            HashSet<long> exRows = expandRC.Item1, exCols = expandRC.Item2;

            // Expand universe - each row/col without galaxy '#' will expand twice (add 1 more with space '.')
            // Expand by (Multiplier - 1). Ex1: for twice = (2-1) = 1 for each empty row/col. Ex2: For 10 = (10-1) = 9
            char[,] inputX = new char[input.GetLength(0) + (exRows.Count * (Multiplier1 - 1)), input.GetLength(1) + (exCols.Count * (Multiplier1 - 1))];
            long rx = 0;
            for (int r = 0; r < input.GetLength(0); r++, rx++)
            {
                long cx = 0;
                for (int c = 0; c < input.GetLength(1); c++, cx++)
                {
                    inputX[rx, cx] = input[r, c];
                    if (exRows.Contains(r))
                    {
                        for (int i = 1; i < Multiplier1; i++)
                        {
                            inputX[rx + i, cx] = '.';
                        }
                    }
                    else if (exCols.Contains(c))
                    {
                        for (int i = 1; i < Multiplier1; i++)
                        {
                            inputX[rx, cx + i] = '.';
                        }
                        cx += (Multiplier1 - 1);   // skip for extra cols
                    }
                }
                if (exRows.Contains(r)) { rx += (Multiplier1 - 1); }   // skip for extra rows
            }
            //inputX.Print(false);

            var galaxies = inputX.GetCellsEqualToValueLong('#');
            return FindSumOfGalaxyDistances(galaxies);
        }

        public override long PartTwo(char[,] input)
        {
            var expandRC = FindRowsAndColsToExpand(input);
            HashSet<long> exRows = expandRC.Item1, exCols = expandRC.Item2;

            // expand - we only need to know position of galaxies in expanded universe
            // find by adding (multiplier - 1) for each empty row/col appearing before current galaxy
            var galaxies = input.GetCellsEqualToValueLong('#');
            for (int i = 0; i < galaxies.Count; i++)
            {
                var extraRows = exRows.Where(r => r < galaxies[i].Item2).Count() * (Multiplier2 - 1);
                var extraCols = exCols.Where(r => r < galaxies[i].Item3).Count() * (Multiplier2 - 1);
                galaxies[i] = (galaxies[i].Item1, galaxies[i].Item2 + extraRows, galaxies[i].Item3 + extraCols);
            }

            return FindSumOfGalaxyDistances(galaxies);
        }

        public override char[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D();
        }

        private const long Multiplier1 = 2;         // part1
        private const long Multiplier2 = 1000000;   // part2 - 10, 100, 1000000

        private (HashSet<long>, HashSet<long>) FindRowsAndColsToExpand(char[,] input)
        {
            var rc = input.GetRowsAndColsList();
            Dictionary<long, List<char>> rows = rc.Item1, cols = rc.Item2;
            var exRows = rows.Where(kvp => !kvp.Value.Any(v => v == '#')).Select(kvp => kvp.Key).ToHashSet();
            var exCols = cols.Where(kvp => !kvp.Value.Any(v => v == '#')).Select(kvp => kvp.Key).ToHashSet();
            return (exRows, exCols);
        }

        private long FindSumOfGalaxyDistances(List<(char, long, long)> galaxies)
        {
            long sum = 0;
            for (int g1 = 0; g1 < galaxies.Count; g1++)
            {
                var first = galaxies[g1];
                // Dijkstra calculates distance b/w first and all other nodes in grid, inputD has all 1 in grid same dim as inputX
                // takes long time (2.14 mins) - we only need dist b/w 2 nodes - so use manhattan distance
                //var distances = inputD.Dijkstra((first.Item2, first.Item3));
                for (int g2 = g1 + 1; g2 < galaxies.Count; g2++)
                {
                    var second = galaxies[g2];
                    //var shortest = distances.Where(kvp => kvp.Key == (second.Item2, second.Item3)).First().Value.Item1;
                    sum += MathExtensions.ManhattanDistance(first.Item3, first.Item2, second.Item3, second.Item2);  // row = y, col = x
                }
            }
            return sum;
        }
    }
}
