using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2021.Day07
{
    public class Day07 : Day<List<int>, long, long>
    {
        public override string DayNumber { get { return "07"; } }

        public override long PartOne(List<int> input)
        {
            return CalculateFuel(input, true);
        }

        public override long PartTwo(List<int> input)
        {
            return CalculateFuel(input, false);
        }

        public override List<int> ProcessInput(string[] input)
        {
            return input[0].Split(",", StringSplitOptions.RemoveEmptyEntries).StringArrayToIntList();
        }

        private long CalculateFuel(List<int> input, bool constantRate)
        {
            var min = input.Min();
            var max = input.Max();

            long minDistance = long.MaxValue;
            for (int i = min; i <= max; i++)
            {
                long sum = 0;
                foreach (var dist in input)
                {
                    var diff = Math.Abs(dist - i);
                    sum += constantRate ? diff : (diff * (diff + 1) / 2);
                }
                if (sum < minDistance) { minDistance = sum; }
            }
            return minDistance;
        }
    }
}
