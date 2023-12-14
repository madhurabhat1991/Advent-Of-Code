using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2023.Day13
{
    public class Day13 : Day<List<char[,]>, long, long>
    {
        public override string DayNumber { get { return "13"; } }

        public override long PartOne(List<char[,]> input)
        {
            long sum = 0;
            foreach (var grid in input)
            {
                sum += FindReflection(grid).Item1;
            }
            return sum;
        }

        public override long PartTwo(List<char[,]> input)
        {
            long sum = 0;
            foreach (var grid in input)
            {
                sum += FindSmudge(grid);
            }
            return sum;
        }

        public override List<char[,]> ProcessInput(string[] input)
        {
            List<char[,]> grids = new List<char[,]>();
            var blocks = input.Blocks();
            foreach (var block in blocks)
            {
                grids.Add(block.ToArray().CreateGrid2D());
            }
            return grids;
        }

        private (long, HashSet<(int, int)>, HashSet<(int, int)>) FindReflection(char[,] grid,
            HashSet<(int, int)> oldHMirrors = null, HashSet<(int, int)> oldVMirrors = null)
        {
            long sum = 0;

            // find all identical rows/cols next to each other with a diff of 1 in their indexes - horizontal and vertical
            HashSet<(int, int)> hRows = new HashSet<(int, int)>(), vCols = new HashSet<(int, int)>();
            for (int r1 = 0, r2 = 1; r1 < grid.GetLength(0) - 1; r1++, r2++)
            {
                bool match = true;
                for (int c = 0; c < grid.GetLength(1); c++)
                {
                    if (grid[r1, c] != grid[r2, c])
                    {
                        match = false;
                        break;
                    }
                }
                if (match) { hRows.Add((r1, r2)); }
            }
            for (int c1 = 0, c2 = 1; c1 < grid.GetLength(1) - 1; c1++, c2++)
            {
                bool match = true;
                for (int r = 0; r < grid.GetLength(0); r++)
                {
                    if (grid[r, c1] != grid[r, c2])
                    {
                        match = false;
                        break;
                    }
                }
                if (match) { vCols.Add((c1, c2)); }
            }

            // span out from each matching rows/cols to it's edge to find symmetry
            HashSet<(int, int)> hMirrors = new HashSet<(int, int)>(), vMirrors = new HashSet<(int, int)>();
            bool mirror;
            foreach (var hRow in hRows)
            {
                if (oldHMirrors != null && oldHMirrors.Any() && oldHMirrors.Contains(hRow)) { continue; }   // skip old mirror
                mirror = true;
                for (int r1 = hRow.Item1 - 1, r2 = hRow.Item2 + 1; mirror && r1 >= 0 && r2 < grid.GetLength(0); r1--, r2++)
                {
                    for (int c = 0; c < grid.GetLength(1); c++)
                    {
                        if (grid[r1, c] != grid[r2, c])
                        {
                            mirror = false;
                            break;
                        }
                    }
                }
                if (mirror)
                {
                    sum = (hRow.Item1 + 1) * 100;
                    hMirrors.Add(hRow);
                    break;
                }
            }
            foreach (var vLine in vCols)
            {
                if (oldVMirrors != null && oldVMirrors.Any() && oldVMirrors.Contains(vLine)) { continue; }   // skip old mirror
                mirror = true;
                for (int c1 = vLine.Item1 - 1, c2 = vLine.Item2 + 1; mirror && c1 >= 0 && c2 < grid.GetLength(1); c1--, c2++)
                {
                    for (int r = 0; r < grid.GetLength(0); r++)
                    {
                        if (grid[r, c1] != grid[r, c2])
                        {
                            mirror = false;
                            break;
                        }
                    }
                }
                if (mirror)
                {
                    sum = (vLine.Item1 + 1);
                    vMirrors.Add(vLine);
                    break;
                }
            }
            return (sum, hMirrors, vMirrors);
        }

        private long FindSmudge(char[,] grid)
        {
            for (int r = 0; r < grid.GetLength(0); r++)
            {
                for (int c = 0; c < grid.GetLength(1); c++)
                {
                    var old = FindReflection(grid);
                    HashSet<(int, int)> oldHMirrors = old.Item2, oldVMirrors = old.Item3;
                    grid[r, c] = grid[r, c] == '.' ? '#' : '.'; ;
                    var newSum = FindReflection(grid, oldHMirrors, oldVMirrors).Item1;
                    grid[r, c] = grid[r, c] == '.' ? '#' : '.'; ;
                    if (newSum > 0) { return newSum; }
                }
            }
            return 0;
        }
    }
}
