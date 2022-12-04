using System;
using System.Collections.Generic;
using System.Text;
using Template;

namespace _2022.Day04
{
    public class Day04 : Day<List<Tuple<(int, int), (int, int)>>, long, long>
    {
        public override string DayNumber { get { return "04"; } }

        public override long PartOne(List<Tuple<(int, int), (int, int)>> input)
        {
            long count = 0;
            foreach (var pair in input)
            {
                (int, int) first = pair.Item1, second = pair.Item2;
                if (first.Item1 <= second.Item1 && first.Item2 >= second.Item2
                    || second.Item1 <= first.Item1 && second.Item2 >= first.Item2) { count++; }
            }
            return count;
        }

        public override long PartTwo(List<Tuple<(int, int), (int, int)>> input)
        {
            long count = 0;
            foreach (var pair in input)
            {
                (int, int) first = pair.Item1, second = pair.Item2;
                if (first.Item2 >= second.Item1 && first.Item1 <= second.Item2) { count++; }
            }
            return count;
        }

        public override List<Tuple<(int, int), (int, int)>> ProcessInput(string[] input)
        {
            List<Tuple<(int, int), (int, int)>> pairs = new List<Tuple<(int, int), (int, int)>>();
            foreach (var line in input)
            {
                var both = line.Split(",");
                var first = both[0].Split("-");
                var second = both[1].Split("-");

                var firstMin = Int32.Parse(first[0]);
                var firstMax = Int32.Parse(first[1]);
                var secondMin = Int32.Parse(second[0]);
                var secondMax = Int32.Parse(second[1]);

                pairs.Add(new Tuple<(int, int), (int, int)>((firstMin, firstMax), (secondMin, secondMax)));
            }
            return pairs;
        }
    }
}
