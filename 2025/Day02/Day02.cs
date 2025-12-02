using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;

namespace _2025.Day02
{
    public class Day02 : Day<List<List<long>>, long, long>
    {
        public override string DayNumber { get { return "02"; } }

        public override long PartOne(List<List<long>> input)
        {
            long sum = 0;
            foreach (var range in input)
            {
                for (var i = range[0]; i <= range[1]; i++)
                {
                    string str = i.ToString();
                    if (str.Length % 2 == 0)
                    {
                        // split half & compare both
                        string first = str[0..(str.Length / 2)];
                        string second = str[(str.Length / 2)..];
                        if (first == second)
                        {
                            sum += i;
                        }
                    }
                }
            }
            return sum;
        }

        public override long PartTwo(List<List<long>> input)
        {
            long sum = 0;
            foreach (var range in input)
            {
                for (var i = range[0]; i <= range[1]; i++)
                {
                    string str = i.ToString();
                    // pattern length can be 1 to half of str length
                    for (var j = 1; j <= str.Length / 2; j++)
                    {
                        // will str accomodate those patterns?
                        if (str.Length % j == 0)
                        {
                            bool samePattern = true;
                            var sections = str.Length / j;
                            // check if each section has same pattern
                            int low = 0;
                            int high = j;
                            while (samePattern && sections > 1)
                            {
                                var first = str[low..high];
                                low += j;
                                high += j;
                                var second = str[low..high];
                                if (first != second)
                                {
                                    samePattern = false;
                                }
                                sections--;
                            }
                            if (samePattern)
                            {
                                sum += i;
                                break;  // break after finding single digit repeater
                            }
                        }
                    }
                }
            }
            return sum;
        }

        public override List<List<long>> ProcessInput(string[] input)
        {
            List<List<long>> nums = new List<List<long>>();
            foreach (var line in input)
            {
                var ranges = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var range in ranges)
                {
                    nums.Add(range.Split('-', StringSplitOptions.RemoveEmptyEntries).StringArrayToLongList());
                }
            }
            return nums;
        }
    }
}
