using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;
using Helpers;

namespace _2022.Day01
{
    public class Day01 : Day<List<long>, long, long>
    {
        public override string DayNumber { get { return "01"; } }

        public override long PartOne(List<long> input)
        {
            return input.Max();
        }

        public override long PartTwo(List<long> input)
        {
            return input.OrderByDescending(r => r).Take(3).Sum();
        }

        public override List<long> ProcessInput(string[] input)
        {
            List<long> totals = new List<long>();

            var blocks = input.Blocks();
            foreach (var block in blocks)
            {
                long total = 0;
                foreach (var calorie in block)
                {
                    total += Int64.Parse(calorie);
                }
                totals.Add(total);
            }

            return totals;
        }
    }
}
