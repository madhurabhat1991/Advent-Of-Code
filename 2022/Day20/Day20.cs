using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;

namespace _2022.Day20
{
    public class Day20 : Day<List<int>, long, long>
    {
        public override string DayNumber { get { return "20"; } }

        public override long PartOne(List<int> input)
        {
            var decrypt = input.DeepClone();
            int shiftIndex = 0;
            for (int i = 0; i < input.Count; i++)
            {
                Mix(decrypt, ref shiftIndex);
            }
            return 0;
        }

        public override long PartTwo(List<int> input)
        {
            return 0;
        }

        public override List<int> ProcessInput(string[] input)
        {
            return input.StringArrayToIntList();
        }

        private void Mix(List<int> decrypt, ref int shiftIndex)
        {
            if (decrypt[shiftIndex] > 0)        // shift forward
            {

            }
            else if (decrypt[shiftIndex] < 0)   // shift backward
            {

            }
        }
    }
}
