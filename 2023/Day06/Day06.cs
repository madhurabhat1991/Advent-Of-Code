using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2023.Day06
{
    public class Day06 : Day<string[], long, long>
    {
        public override string DayNumber { get { return "06"; } }

        public override long PartOne(string[] input)
        {
            return FindWays(DecodeRaceRecord(input)).Aggregate((a, x) => a * x);
        }

        public override long PartTwo(string[] input)
        {
            return FindWays(DecodeRaceRecord(input, true)).First();
        }

        public override string[] ProcessInput(string[] input)
        {
            return input;
        }

        private List<(long, long)> DecodeRaceRecord(string[] input, bool part2 = false)
        {
            List<(long, long)> tds = new List<(long, long)>();
            var parseTime = input[0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1];
            var parseDist = input[1].Split(':', StringSplitOptions.RemoveEmptyEntries)[1];
            var times = !part2 ? parseTime.Split(' ', StringSplitOptions.RemoveEmptyEntries).StringArrayToLongList() : new string[1] { parseTime.Replace(" ", "") }.StringArrayToLongList();
            var dists = !part2 ? parseDist.Split(' ', StringSplitOptions.RemoveEmptyEntries).StringArrayToLongList() : new string[1] { parseDist.Replace(" ", "") }.StringArrayToLongList();
            for (int i = 0; i < times.Count; i++)
            {
                tds.Add((times[i], dists[i]));
            }
            return tds;
        }

        private List<long> FindWays(List<(long, long)> tds)
        {
            List<long> cntList = new List<long>();
            foreach (var td in tds)
            {
                var time = td.Item1;
                var dist = td.Item2;
                List<long> ways = new List<long>();
                for (long t = 0; t <= time; t++)
                {
                    var moved = (time - t) * t;
                    if (moved > dist) { ways.Add(moved); }
                }
                cntList.Add(ways.Count);
            }
            return cntList;
        }
    }
}
