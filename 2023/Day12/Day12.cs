using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2023.Day12
{
    public class Day12 : Day<List<(string, List<int>)>, long, long>
    {
        public override string DayNumber { get { return "12"; } }

        public override long PartOne(List<(string, List<int>)> input)
        {
            long sum = 0;
            foreach (var record in input)
            {
                var str = record.Item1;
                var groups = record.Item2;
                sum += FindArrangements(str, groups, 0, 0, 0, 1);
            }
            return sum;
        }

        public override long PartTwo(List<(string, List<int>)> input)
        {
            throw new NotImplementedException();
        }

        public override List<(string, List<int>)> ProcessInput(string[] input)
        {
            List<(string, List<int>)> records = new List<(string, List<int>)>();
            foreach (var line in input)
            {
                var info = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                records.Add((info[0], info[1].Split(',', StringSplitOptions.RemoveEmptyEntries).StringArrayToIntList()));
            }
            return records;
        }

        //private readonly char Operational = '.';
        //private readonly char Damaged = '#';
        private readonly char Unknown = '?';

        private long FindArrangements(string str, List<int> groups, int index, int group, int amount, int permutations)
        {
            if (index == str.Length - 1) { return permutations; }   // end of string
            if (str[index] == Unknown)
            {
                string newStr = str;
                //newStr[index] = Operational;
                FindArrangements(newStr, groups, index, group, amount, permutations);
            }
            return 0;
        }
    }
}
