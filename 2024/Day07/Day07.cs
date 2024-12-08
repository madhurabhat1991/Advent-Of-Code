using Helpers;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Template;

namespace _2024.Day07
{
    public class Day07 : Day<List<(long, List<long>)>, long, long>
    {
        public override string DayNumber { get { return "07"; } }

        public override long PartOne(List<(long, List<long>)> input)
        {
            long sum = 0;
            foreach (var eq in input)
            {
                var testVal = eq.Item1;
                var nums = eq.Item2;
                bool correct = false;

                Queue<long> q = new Queue<long>([nums[0]]);
                int i = 1;
                while (i < nums.Count)
                {
                    HashSet<long> running = new HashSet<long>();
                    while (q.Count != 0)
                    {
                        var accumulated = q.Dequeue();
                        foreach (var op in part1)
                        {
                            var eval = operations[op](accumulated, nums[i]);
                            if (i == nums.Count - 1 && eval == testVal) { correct = true; }
                            else if (i < nums.Count && eval <= testVal) { running.Add(eval); }
                        }
                    }
                    q = new Queue<long>(running);
                    i++;
                }
                sum += correct ? testVal : 0;
            }
            return sum;
        }

        public override long PartTwo(List<(long, List<long>)> input)
        {
            long sum = 0;
            foreach (var eq in input)
            {
                var testVal = eq.Item1;
                var nums = eq.Item2;
                bool correct = false;

                Queue<long> q = new Queue<long>([nums[0]]);
                int i = 1;
                while (i < nums.Count)
                {
                    HashSet<long> running = new HashSet<long>();
                    while (q.Count != 0)
                    {
                        var accumulated = q.Dequeue();
                        foreach (var op in part2)           // use additional operation for part 2
                        {
                            var eval = operations[op](accumulated, nums[i]);
                            if (i == nums.Count - 1 && eval == testVal) { correct = true; }
                            else if (i < nums.Count && eval <= testVal) { running.Add(eval); }
                        }
                    }
                    q = new Queue<long>(running);
                    i++;
                }
                sum += correct ? testVal : 0;
            }
            return sum;
        }

        public override List<(long, List<long>)> ProcessInput(string[] input)
        {
            List<(long, List<long>)> equations = new List<(long, List<long>)>();
            foreach (var line in input)
            {
                var info = line.Split(':', StringSplitOptions.RemoveEmptyEntries);
                equations.Add((long.Parse(info[0]), info[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).StringArrayToLongList()));
            }
            return equations;
        }

        private static readonly Func<long, long, long> Add = (a, b) => a + b;
        private static readonly Func<long, long, long> Multiply = (a, b) => a * b;
        private static readonly Func<long, long, long> Concatenate = (a, b) => long.Parse(a.ToString() + b.ToString());

        private readonly Dictionary<string, Func<long, long, long>> operations = new Dictionary<string, Func<long, long, long>>()
        {
            { "+", new Func<long, long, long>(Add) },
            { "*", new Func<long, long, long>(Multiply) },
            { "||", new Func<long, long, long>(Concatenate) }
        };

        private readonly List<string> part1 = new List<string>() { "+", "*" };
        private readonly List<string> part2 = new List<string>() { "+", "*", "||" };

    }
}
