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
                var sizes = record.Item2;

                int idx = 0;
                foreach (var size in sizes)
                {
                    var rem = size;
                    while (rem > 0)
                    {
                        if (str[idx] == Operational) { idx++; }
                        else if (str[idx] == Damaged) { rem--; }
                        else if (str[idx] == Unknown)
                        {
                            // find contiguous '?'
                            int block = 1;
                            int b = idx + 1;
                            while (str[b] != Operational)
                            {

                            }
                        }
                    }
                }
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

        private readonly char Operational = '.';
        private readonly char Damaged = '#';
        private readonly char Unknown = '?';
    }
}
