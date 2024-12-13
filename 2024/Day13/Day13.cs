using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;

namespace _2024.Day13
{
    public class Day13 : Day<List<((long, long), (long, long), (long, long))>, long, long>
    {
        public override string DayNumber { get { return "13"; } }

        public override long PartOne(List<((long, long), (long, long), (long, long))> input)
        {
            long sum = 0;
            foreach (var machine in input)
            {
                var a = machine.Item1;
                var b = machine.Item2;
                var p = machine.Item3;
                // you have 2 linear equations. a1x + b1y = p1 and a2x + b2y = p2
                // solve for y and substitute to find x
                long y = (p.Item2 * a.Item1 - a.Item2 * p.Item1) / (a.Item1 * b.Item2 - a.Item2 * b.Item1);
                long x = (p.Item1 - b.Item1 * y) / a.Item1;
                // confirm that the values solve the equations
                if (a.Item1 * x + b.Item1 * y == p.Item1 && a.Item2 * x + b.Item2 * y == p.Item2) { sum += x * 3 + y; }
            }
            return sum;
        }

        public override long PartTwo(List<((long, long), (long, long), (long, long))> input)
        {
            long sum = 0;
            foreach (var machine in input)
            {
                var a = machine.Item1;
                var b = machine.Item2;
                var p = (machine.Item3.Item1 + 10000000000000, machine.Item3.Item2 + 10000000000000);
                long y = (p.Item2 * a.Item1 - a.Item2 * p.Item1) / (a.Item1 * b.Item2 - a.Item2 * b.Item1);
                long x = (p.Item1 - b.Item1 * y) / a.Item1;
                if (a.Item1 * x + b.Item1 * y == p.Item1 && a.Item2 * x + b.Item2 * y == p.Item2) { sum += x * 3 + y; }
            }
            return sum;
        }

        public override List<((long, long), (long, long), (long, long))> ProcessInput(string[] input)
        {
            List<((long, long), (long, long), (long, long))> machines = new List<((long, long), (long, long), (long, long))>();
            var blocks = input.Blocks();
            foreach (var block in blocks)
            {
                (long, long) a = (Int64.Parse(block[0].Substring(12, block[0].IndexOf(',') - 12)),
                    Int64.Parse(block[0].Substring(block[0].IndexOf('+', 12) + 1, block[0].Length - 1 - block[0].IndexOf('+', 12))));
                (long, long) b = (Int64.Parse(block[1].Substring(12, block[1].IndexOf(',') - 12)),
                    Int64.Parse(block[1].Substring(block[1].IndexOf('+', 12) + 1, block[1].Length - 1 - block[1].IndexOf('+', 12))));
                (long, long) p = (Int64.Parse(block[2].Substring(9, block[2].IndexOf(',') - 9)),
                    Int64.Parse(block[2].Substring(block[2].IndexOf('=', 9) + 1, block[2].Length - 1 - block[2].IndexOf('=', 9))));
                machines.Add((a, b, p));
            }
            return machines;
        }
    }
}
