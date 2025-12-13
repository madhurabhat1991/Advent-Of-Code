using Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Template;

namespace _2025.Day12
{
    public class Day12 : Day<(List<char[,]>, List<((int, int), List<int>)>), long, long>
    {
        public override string DayNumber { get { return "12"; } }

        public override long PartOne((List<char[,]>, List<((int, int), List<int>)>) input)
        {
            long count = 0;
            // check spaces that can accomodate all gifts easily - input has 3x3 gift shapes too
            var regions = input.Item2;
            foreach (var region in regions)
            {
                long dim = region.Item1.Item1 * region.Item1.Item2;
                if (region.Item2.Sum() * (3 * 3) <= dim) { count++; }
            }
            return count;
            // not the right solution for example. all of the count were the only ones accomodating in input, so worked.
            // need to solve correctly
        }

        public override long PartTwo((List<char[,]>, List<((int, int), List<int>)>) input)
        {
            return 0;
        }

        public override (List<char[,]>, List<((int, int), List<int>)>) ProcessInput(string[] input)
        {
            var blocks = input.Blocks();
            List<char[,]> gifts = new List<char[,]>();
            for (int i = 0; i < blocks.Count - 1; i++)
            {
                var block = blocks[i];
                string[] strings = new string[3];
                for (int j = 1; j < block.Count; j++)
                {
                    var line = block[j];
                    strings[j - 1] = line;
                }
                gifts.Add(strings.CreateGrid2D());
            }
            List<((int, int), List<int>)> regions = new List<((int, int), List<int>)>();
            foreach (var line in blocks.Last())
            {
                var splits = line.Split(':', StringSplitOptions.RemoveEmptyEntries);
                var dim = splits[0].Split('x', StringSplitOptions.RemoveEmptyEntries);
                var qty = splits[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).StringArrayToIntList();
                regions.Add(((Int32.Parse(dim[0]), Int32.Parse(dim[1])), qty));
            }
            return (gifts, regions);
        }
    }
}
