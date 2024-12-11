using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Numerics;

namespace _2024.Day11
{
    public class Day11 : Day<List<long>, long, long>
    {
        public override string DayNumber { get { return "11"; } }

        public override long PartOne(List<long> input)
        {
            Queue<long> q = new Queue<long>(input);
            for (int i = 1; i <= Blinks1; i++)
            {
                Queue<long> q2 = new Queue<long>();
                while (q.Count > 0)
                {
                    var item = q.Dequeue();
                    if (item == 0) { q2.Enqueue(1); }
                    else if (item.ToString().Length % 2 == 0)
                    {
                        var left = long.Parse(item.ToString().Substring(0, item.ToString().Length / 2));
                        var right = long.Parse(item.ToString().Substring(item.ToString().Length / 2, item.ToString().Length / 2));
                        q2.Enqueue(left);
                        q2.Enqueue(right);
                    }
                    else { q2.Enqueue(item * 2024); }
                }
                q = q2;
            }
            return q.Count;
        }

        public override long PartTwo(List<long> input)
        {
            // using part 1 code takes long time. numbers are repeating so keep count of numbers. deja vu.. lanternfish 2021 day 6
            // part 2 = 72ms, part 1 naive = 64ms, part 1 dict = 7ms, leaving the naive code
            Dictionary<long, long> numbers = new Dictionary<long, long>();      // <number, count>
            foreach (var item in input) { numbers[item] = 1; }
            for (int i = 1; i <= Blinks2; i++)
            {
                Dictionary<long, long> newNumbers = new Dictionary<long, long>();
                foreach (var number in numbers.Keys)
                {
                    var count = numbers[number];
                    if (number == 0) { newNumbers.IncrementValue(1, count); }
                    else if (number.ToString().Length % 2 == 0)
                    {
                        var left = long.Parse(number.ToString().Substring(0, number.ToString().Length / 2));
                        var right = long.Parse(number.ToString().Substring(number.ToString().Length / 2, number.ToString().Length / 2));
                        newNumbers.IncrementValue(left, count);
                        newNumbers.IncrementValue(right, count);
                    }
                    else { newNumbers.IncrementValue(number * 2024, count); }
                }
                numbers = newNumbers;
            }
            return numbers.Values.Sum();
        }

        public override List<long> ProcessInput(string[] input)
        {
            return input[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).StringArrayToLongList();
        }

        private readonly int Blinks1 = 25;
        private readonly int Blinks2 = 75;
    }
}
