using System;
using System.Collections.Generic;
using System.Text;
using Template;

namespace _2022.Day04
{
    public class Day04 : Day<List<((int, int), (int, int))>, long, long>
    {
        public override string DayNumber { get { return "04"; } }

        public override long PartOne(List<((int, int), (int, int))> input)
        {
            long count = 0;
            foreach (var pair in input)
            {
                int aStart = pair.Item1.Item1, aEnd = pair.Item1.Item2, bStart = pair.Item2.Item1, bEnd = pair.Item2.Item2;
                if (aStart <= bStart && aEnd >= bEnd || bStart <= aStart && bEnd >= aEnd) { count++; }
            }
            return count;
        }

        public override long PartTwo(List<((int, int), (int, int))> input)
        {
            long count = 0;
            foreach (var pair in input)
            {
                int aStart = pair.Item1.Item1, aEnd = pair.Item1.Item2, bStart = pair.Item2.Item1, bEnd = pair.Item2.Item2;
                if (aEnd >= bStart && aStart <= bEnd) { count++; }
            }
            return count;
        }

        public override List<((int, int), (int, int))> ProcessInput(string[] input)
        {
            List<((int, int), (int, int))> pairs = new List<((int, int), (int, int))>();
            foreach (var line in input)
            {
                var both = line.Split(",");
                var a = both[0].Split("-");
                var b = both[1].Split("-");

                int aStart = Int32.Parse(a[0]), aEnd = Int32.Parse(a[1]);
                int bStart = Int32.Parse(b[0]), bEnd = Int32.Parse(b[1]);

                pairs.Add(((aStart, aEnd), (bStart, bEnd)));
            }
            return pairs;
        }
    }
}
