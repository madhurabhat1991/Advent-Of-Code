using Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Template;

namespace _2022.Day13
{
    public class Day13 : Day<List<(String, String)>, long, long>
    {
        public override string DayNumber { get { return "13"; } }

        public override long PartOne(List<(string, string)> input)
        {
            long sum = 0;

            for (int i = 0; i < input.Count; i++)
            {
                var order = Compare(input[i].Item1, input[i].Item2);
            }

            return sum;
        }

        public override long PartTwo(List<(string, string)> input)
        {
            return 0;
        }

        public override List<(string, string)> ProcessInput(string[] input)
        {
            List<(string, string)> pairs = new List<(string, string)>();
            input.Blocks().ForEach(block => pairs.Add((block[0], block[1])));
            return pairs;
        }

        private const char Open = '[';
        private const char Close = ']';

        private string OpenBrackets(string str)
        {
            if (str.Length > 2) { str = str.Substring(1, str.Length - 2); }
            else str = "";
            return str;
        }

        private bool Compare(string first, string second)
        {
            // both are lists - open brackets
            if (first[0].Equals(Open) && second[0].Equals(Open))
            {
                Compare(OpenBrackets(first), OpenBrackets(second));
            }
            return false;
        }
    }
}
