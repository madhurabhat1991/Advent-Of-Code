using Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Template;
using System.Linq;

namespace _2021.Day01
{
    public class Day01 : Day<List<int>, long, long>
    {
        public override long PartOne(List<int> input)
        {
            long count = 0;
            for (int i = 0; i < input.Count - 1; i++)
            {
                if (input[i + 1] > input[i])
                {
                    count += 1;
                }
            }
            return count;
        }

        public override long PartTwo(List<int> input)
        {
            long count = 0;
            for (int i = 0; i < input.Count - 2; i++)
            {
                if (i > 0 && ((input[i] + input[i + 1] + input[i + 2]) > (input[i - 1] + input[i] + input[i + 1])))
                {
                    count += 1;
                }
            }
            return count;
        }

        public override List<int> ProcessInput(string[] input)
        {
            return input.StringArrayToIntList();
        }
    }
}