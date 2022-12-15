using System;
using System.Collections.Generic;
using System.Text;
using Template;

namespace _2022.Day15
{
    public class Day15 : Day<List<((int, int), (int, int))>, long, long>
    {
        public override string DayNumber { get { return "15"; } }

        public override long PartOne(List<((int, int), (int, int))> input)
        {
            throw new NotImplementedException();
        }

        public override long PartTwo(List<((int, int), (int, int))> input)
        {
            throw new NotImplementedException();
        }

        public override List<((int, int), (int, int))> ProcessInput(string[] input)
        {
            List<((int, int), (int, int))> pairs = new List<((int, int), (int, int))>();
            foreach (var line in input)
            {
                var sb = line.Split(":");
                var s = sb[0].Replace("Sensor at ", "").Split(",", StringSplitOptions.RemoveEmptyEntries);
                var b = sb[1].Replace(" closest beacon is at ", "").Split(",", StringSplitOptions.RemoveEmptyEntries);
                pairs.Add(((Int32.Parse(s[0].Trim().Replace("x=", "")), Int32.Parse(s[1].Trim().Replace("y=", ""))),
                    (Int32.Parse(b[0].Trim().Replace("x=", "")), Int32.Parse(b[1].Trim().Replace("y=", "")))));
            }
            return pairs;
        }
    }
}
