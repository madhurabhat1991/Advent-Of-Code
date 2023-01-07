using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;

namespace _2021.Day18
{
    public class Day18 : Day<string[], long, long>
    {
        public override string DayNumber { get { return "18"; } }

        public override long PartOne(string[] input)
        {
            var sum = input[0];
            for (int i = 0; i < input.Length - 1; i++)
            {
                sum = Add(sum, input[i + 1]);
            }
            return PacketParser.GetMagnitude(sum, Start);
        }

        public override long PartTwo(string[] input)
        {
            HashSet<long> magnitudes = new HashSet<long>();
            for (int i = 0; i < input.Length - 1; i++)
            {
                for (int j = i + 1; j < input.Length; j++)
                {
                    var sum1 = Add(input[i], input[j]);
                    var mag1 = PacketParser.GetMagnitude(sum1, Start);
                    var sum2 = Add(input[j], input[i]);
                    var mag2 = PacketParser.GetMagnitude(sum2, Start);
                    magnitudes.Add(Math.Max(mag1, mag2));
                }
            }
            return magnitudes.Max();
        }

        public override string[] ProcessInput(string[] input)
        {
            return input;
        }

        private const char Start = '[';
        private const char End = ']';
        private const int ExplodeLimit = 4;
        private const int SplitLimit = 10;

        private string Add(string first, string second)
        {
            // add 2 numbers
            string sum = $"[{first},{second}]";
            // reduce
            sum = Reduce(sum);
            return sum;
        }

        private string Reduce(string input)
        {
            // until nothing left to reduce
            while (true)
            {
                // check if anything to explode
                int index = 0;
                while (PacketParser.CheckExplode(input, Start, End, ExplodeLimit, out index))
                {
                    input = PacketParser.Explode(input, Start, End, index);
                }
                // check if anything to split
                int length = 0;
                if (PacketParser.CheckSplit(input, SplitLimit, out index, out length))
                {
                    input = PacketParser.Split(input, Start, End, index, length);
                }
                // end if nothing to explode or split
                if (!PacketParser.CheckExplode(input, Start, End, ExplodeLimit, out index)
                    && !PacketParser.CheckSplit(input, SplitLimit, out index, out length)) { break; }
            }
            return input;
        }
    }
}
