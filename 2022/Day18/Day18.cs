using System;
using System.Collections.Generic;
using System.Text;
using Template;

namespace _2022.Day18
{
    public class Day18 : Day<List<(int, int, int)>, long, long>
    {
        public override string DayNumber { get { return "18"; } }

        public override long PartOne(List<(int, int, int)> input)
        {
            var area = input.Count * 6;
            for (int i = 0; i < input.Count - 1; i++)
            {
                for (int j = i + 1; j < input.Count; j++)
                {
                    if (Math.Abs(input[i].Item1 - input[j].Item1) + Math.Abs(input[i].Item2 - input[j].Item2) + Math.Abs(input[i].Item3 - input[j].Item3) == 1)
                    {
                        area -= 2;
                    }
                }
            }
            return area;
        }

        public override long PartTwo(List<(int, int, int)> input)
        {
            throw new NotImplementedException();
        }

        public override List<(int, int, int)> ProcessInput(string[] input)
        {
            List<(int, int, int)> pos = new List<(int, int, int)>();
            foreach (var line in input)
            {
                var p = line.Split(",", StringSplitOptions.RemoveEmptyEntries);
                pos.Add((Int32.Parse(p[0]), Int32.Parse(p[1]), Int32.Parse(p[2])));
            }
            return pos;
        }
    }
}
