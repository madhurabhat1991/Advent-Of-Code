using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;

namespace _2024.Day08
{
    public class Day08 : Day<(Dictionary<char, List<(int, int)>>, char[,]), long, long>
    {
        public override string DayNumber { get { return "08"; } }

        public override long PartOne((Dictionary<char, List<(int, int)>>, char[,]) input)
        {
            HashSet<(int, int)> antinodes = new HashSet<(int, int)>();
            var aDict = input.Item1;
            var grid = input.Item2;
            foreach (var aType in aDict.Keys)
            {
                var antennas = aDict[aType];
                for (var i = 0; i < antennas.Count; i++)
                {
                    for (var j = 0; j < antennas.Count && i != j; j++)
                    {
                        (int, int) antenna1 = antennas[i], antenna2 = antennas[j];
                        int dx = Math.Abs(antenna1.Item1 - antenna2.Item1);
                        int dy = Math.Abs(antenna1.Item2 - antenna2.Item2);
                        (int, int) antinode1 = ((antenna1.Item1 < antenna2.Item1 ? antenna1.Item1 - dx : antenna1.Item1 + dx),
                            (antenna1.Item2 < antenna2.Item2 ? antenna1.Item2 - dy : antenna1.Item2 + dy));
                        (int, int) antinode2 = ((antenna2.Item1 < antenna1.Item1 ? antenna2.Item1 - dx : antenna2.Item1 + dx),
                            (antenna2.Item2 < antenna1.Item2 ? antenna2.Item2 - dy : antenna2.Item2 + dy));
                        if (antinode1.Item1 >= 0 && antinode1.Item1 <= grid.GetLength(0) - 1 && antinode1.Item2 >= 0 && antinode1.Item2 <= grid.GetLength(1) - 1) { antinodes.Add(antinode1); }
                        if (antinode2.Item1 >= 0 && antinode2.Item1 <= grid.GetLength(0) - 1 && antinode2.Item2 >= 0 && antinode2.Item2 <= grid.GetLength(1) - 1) { antinodes.Add(antinode2); }
                    }
                }
            }
            return antinodes.Count;
        }

        public override long PartTwo((Dictionary<char, List<(int, int)>>, char[,]) input)
        {
            HashSet<(int, int)> antinodes = new HashSet<(int, int)>();
            var aDict = input.Item1;
            var grid = input.Item2;
            foreach (var aType in aDict.Keys)
            {
                var antennas = aDict[aType];
                for (var i = 0; i < antennas.Count; i++)
                {
                    for (var j = 0; j < antennas.Count && i != j; j++)
                    {
                        (int, int) antenna1 = antennas[i], antenna2 = antennas[j];
                        antinodes.Add(antenna1);        // consider antennas as antinodes
                        antinodes.Add(antenna2);
                        int dx = Math.Abs(antenna1.Item1 - antenna2.Item1);
                        int dy = Math.Abs(antenna1.Item2 - antenna2.Item2);
                        while (true)
                        {
                            (int, int) antinode1 = ((antenna1.Item1 < antenna2.Item1 ? antenna1.Item1 - dx : antenna1.Item1 + dx),
                                (antenna1.Item2 < antenna2.Item2 ? antenna1.Item2 - dy : antenna1.Item2 + dy));
                            if (antinode1.Item1 >= 0 && antinode1.Item1 <= grid.GetLength(0) - 1 && antinode1.Item2 >= 0 && antinode1.Item2 <= grid.GetLength(1) - 1)
                            {
                                antinodes.Add(antinode1);
                                antenna2 = antenna1;    // progress
                                antenna1 = antinode1;
                            }
                            else { break; }
                        }
                        antenna1 = antennas[i]; antenna2 = antennas[j];
                        while (true)
                        {
                            (int, int) antinode2 = ((antenna2.Item1 < antenna1.Item1 ? antenna2.Item1 - dx : antenna2.Item1 + dx),
                                (antenna2.Item2 < antenna1.Item2 ? antenna2.Item2 - dy : antenna2.Item2 + dy));
                            if (antinode2.Item1 >= 0 && antinode2.Item1 <= grid.GetLength(0) - 1 && antinode2.Item2 >= 0 && antinode2.Item2 <= grid.GetLength(1) - 1)
                            {
                                antinodes.Add(antinode2);
                                antenna1 = antenna2;
                                antenna2 = antinode2;
                            }
                            else { break; }
                        }

                    }
                }
            }
            return antinodes.Count;
        }

        public override (Dictionary<char, List<(int, int)>>, char[,]) ProcessInput(string[] input)
        {
            Dictionary<char, List<(int, int)>> aDict = new Dictionary<char, List<(int, int)>>();
            var grid = input.CreateGrid2D();
            for (int r = 0; r < grid.GetLength(0); r++)
            {
                for (int c = 0; c < grid.GetLength(1); c++)
                {
                    if (Char.IsLetterOrDigit(grid[r, c]))
                    {
                        if (aDict.ContainsKey(grid[r, c])) { aDict[grid[r, c]].Add((r, c)); }
                        else { aDict[grid[r, c]] = new List<(int, int)>([(r, c)]); }
                    }
                }
            }
            return (aDict, grid);
        }
    }
}
