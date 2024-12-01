using System;
using System.Collections.Generic;
using System.Text;
using Template;

namespace _2024.Day01
{
    public class Day01 : Day<(List<int>, List<int>), long, long>
    {
        public override string DayNumber { get { return "01"; } }

        public override long PartOne((List<int>, List<int>) input)
        {
            long sum = 0;
            List<int> leftList = input.Item1, rightList = input.Item2;
            leftList.Sort();
            rightList.Sort();
            for (int i = 0; i< leftList.Count; i++)
            {
                var diff = Math.Abs(leftList[i] - rightList[i]);
                sum += diff;
            }
            return sum;
        }

        public override long PartTwo((List<int>, List<int>) input)
        {
            long sum = 0;
            List<int> leftList = input.Item1, rightList = input.Item2;
            foreach (var item in leftList)
            {
                var similar = rightList.Count(r => r == item);
                sum += item * similar;
            }
            return sum;
        }

        public override (List<int>, List<int>) ProcessInput(string[] input)
        {
            List<int> leftList = new List<int>(), rightList = new List<int>();
            foreach (var line in input)
            {
                var ids = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                leftList.Add(Int32.Parse(ids[0]));
                rightList.Add(Int32.Parse(ids[1]));
            }
            return (leftList, rightList);
        }
    }
}
