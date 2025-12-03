using Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Template;

namespace _2025.Day03
{
    public class Day03 : Day<List<List<int>>, long, long>
    {
        public override string DayNumber { get { return "03"; } }

        public override long PartOne(List<List<int>> input)
        {
            long sum = 0;
            foreach (var bank in input)
            {
                long maxForBank = 0;
                for (var i = 0; i < bank.Count; i++)
                {
                    long maxForIndex = 0;
                    // what is the biggest 2-digit number this index can make?
                    for (var j = i + 1; j < bank.Count; j++)
                    {
                        long joltage = Int64.Parse($"{bank[i]}{bank[j]}");
                        if (joltage > maxForIndex)
                        {
                            maxForIndex = joltage;
                        }
                    }
                    if (maxForIndex > maxForBank)
                    {
                        maxForBank = maxForIndex;
                    }
                }
                sum += maxForBank;
            }
            return sum;
        }

        public override long PartTwo(List<List<int>> input)
        {
            long sum = 0;
            int bLength = 12;
            foreach (var bank in input)
            {
                string maxForBank = "";
                int lastMaxIndex = -1;
                while (maxForBank.Length < bLength)
                {
                    // range for current index
                    int index = maxForBank.Length;
                    (int min, int max) = (index, bank.Count - bLength + index);
                    // adjust min if index of last found max is above current min
                    if (lastMaxIndex >= min)
                    {
                        min = lastMaxIndex + 1;
                    }
                    // first max value and its index in that range
                    int maxForRange = 0;
                    for (var i = min; i <= max; i++)
                    {
                        if (bank[i] > maxForRange)
                        {
                            maxForRange = bank[i];
                            lastMaxIndex = i;
                        }
                    }
                    maxForBank += maxForRange;
                }
                sum += Int64.Parse(maxForBank);
            }
            return sum;
        }

        public override List<List<int>> ProcessInput(string[] input)
        {
            List<List<int>> ratings = new List<List<int>>();
            foreach (var line in input)
            {
                List<int> nums = new List<int>();
                foreach (var c in line)
                {
                    nums.Add(c.CharToInt());
                }
                ratings.Add(nums);
            }
            return ratings;
        }
    }
}
