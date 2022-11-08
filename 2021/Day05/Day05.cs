using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;
using Helpers;

namespace _2021.Day05
{
    public class Day05 : Day<List<(List<int>, List<int>)>, long, long>
    {
        public override string DayNumber { get { return "05"; } }

        public override long PartOne(List<(List<int>, List<int>)> input)
        {
            input = input.Where(r => r.Item1[0] == r.Item2[0] || r.Item1[1] == r.Item2[1]).ToList();
            return PlotVents(input);
        }

        public override long PartTwo(List<(List<int>, List<int>)> input)
        {
            return PlotVents(input);
        }

        public override List<(List<int>, List<int>)> ProcessInput(string[] input)
        {
            List<(List<int>, List<int>)> cords = new List<(List<int>, List<int>)>();
            while (input.Any())
            {
                var x = input[0].Split(" -> ", StringSplitOptions.RemoveEmptyEntries).ToList();
                var from = x[0].Split(",", StringSplitOptions.RemoveEmptyEntries).StringArrayToIntList();
                var to = x[1].Split(",", StringSplitOptions.RemoveEmptyEntries).StringArrayToIntList();
                cords.Add((from, to));
                input = input.Skip(1).ToArray();
            }
            return cords;
        }

        private long PlotVents(List<(List<int>, List<int>)> input)
        {
            Dictionary<(int, int), long> plots = new Dictionary<(int, int), long>();
            foreach (var cord in input)
            {
                var x1 = cord.Item1[0];
                var y1 = cord.Item1[1];
                var x2 = cord.Item2[0];
                var y2 = cord.Item2[1];

                for (int x = x1, y = y1;
                    x1 != x2 ? (x1 < x2 ? x <= x2 : x >= x2) : (y1 < y2 ? y <= y2 : y >= y2);
                    x = (x1 == x2 ? x : x1 < x2 ? x + 1 : x - 1), y = (y1 == y2 ? y : y1 < y2 ? y + 1 : y - 1))
                {
                    if (plots.ContainsKey((x, y)))
                    {
                        plots[(x, y)]++;
                    }
                    else
                    {
                        plots[(x, y)] = 1;
                    }
                }
            }
            return plots.Where(r => r.Value >= 2).Count();
        }
    }
}
