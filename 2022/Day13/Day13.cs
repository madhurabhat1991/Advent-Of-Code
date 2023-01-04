using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;

namespace _2022.Day13
{
    public class Day13 : Day<string[], long, long>
    {
        public override string DayNumber { get { return "13"; } }

        public override long PartOne(string[] input)
        {
            long sum = 0;
            var packets = input.Blocks();
            int i = 0;
            packets.ForEach(r =>
            {
                if (PacketParser.CompareElements(ParsePacket(r[0]), ParsePacket(r[1])) <= 0)
                {
                    sum += i + 1;
                }
                i++;
            });
            return sum;
        }

        public override long PartTwo(string[] input)
        {
            long prod = 1;
            var packets = input.Blocks().SelectMany(r => r).ToList();
            packets.AddRange(DividerPkts);
            List<(string, List<object>)> pairs = new List<(string, List<object>)>();
            packets.ForEach(r => { pairs.Add((r, ParsePacket(r))); });
            pairs.Sort((f, s) => PacketParser.CompareElements(f.Item2, s.Item2));
            DividerPkts.ForEach(r => prod = prod * (pairs.IndexOf(pairs.First(p => p.Item1.Equals(r))) + 1));
            return prod;
        }

        public override string[] ProcessInput(string[] input)
        {
            return input;
        }

        private const char Start = '[';
        private const char End = ']';

        private List<string> DividerPkts = new List<string>() { "[[2]]", "[[6]]" };

        private List<object> ParsePacket(string input)
        {
            return PacketParser.Parse(input, Start, End);
        }
    }
}
