using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template;
using Helpers;

namespace _2023.Day08
{
    public class Day08 : Day<(string, Dictionary<string, (string, string)>), long, long>
    {
        public override string DayNumber { get { return "08"; } }

        public override long PartOne((string, Dictionary<string, (string, string)>) input)
        {
            string start = "AAA", next = start, end = "ZZZ";
            return ReachEnd(next, end, input);
        }

        public override long PartTwo((string, Dictionary<string, (string, string)>) input)
        {
            // Each start reaches it's end and cycles on same path until all start nodes reach their end nodes simulataneously
            // Get steps for each start node and calculate LCM
            // The input is framed such that each node cycles, otherwise it wouldn't have been possible to solve using LCM

            List<string> startList = new List<string>(input.Item2.Where(kvp => kvp.Key.EndsWith('A')).Select(kvp => kvp.Key));
            List<long> stepsList = new List<long>();
            foreach (var start in startList)
            {
                var next = start;
                stepsList.Add(ReachEnd(next, "", input, true));
            }
            return MathExtensions.LCM(stepsList);

            // Bruteforce -- takes long time for input -- cannot work with this
            //List<string> nextList = startList.ToList();
            //while (!(nextList.All(r => r.EndsWith('Z'))))
            //{
            //    nextList = new List<string>();
            //    if (i == instr.Length) { i = 0; }
            //    var turn = instr[i];
            //    foreach (var node in startList)
            //    {
            //        var current = nodes[node];
            //        switch (turn)
            //        {
            //            case 'L':
            //                nextList.Add(current.Item1);
            //                break;
            //            case 'R':
            //                nextList.Add(current.Item2);
            //                break;
            //            default:
            //                break;
            //        }
            //    }
            //    startList = nextList.ToList();
            //    steps++;
            //    i++;
            //}
        }

        public override (string, Dictionary<string, (string, string)>) ProcessInput(string[] input)
        {
            var instr = input[0];
            Dictionary<string, (string, string)> nodes = new Dictionary<string, (string, string)>();    // Dictionary<node, (left, right)>

            for (int i = 2; i < input.Length; i++)
            {
                var line = input[i];
                var info = line.Split('=', StringSplitOptions.RemoveEmptyEntries);
                var root = info[0].Trim();
                var branches = info[1].Trim(new char[] { ' ', '(', ')' }).Split(',', StringSplitOptions.RemoveEmptyEntries);
                var left = branches[0].Trim();
                var right = branches[1].Trim();
                nodes.Add(root, (left, right));
            }
            return (instr, nodes);
        }

        private long ReachEnd(string next, string end, (string, Dictionary<string, (string, string)>) input, bool part2 = false)
        {
            var instr = input.Item1;
            var nodes = input.Item2;
            long steps = 0;
            int i = 0;
            while ((!part2 && next != end) || (part2 && !(next.EndsWith('Z'))))
            {
                var current = nodes[next];
                if (i == instr.Length) { i = 0; }
                switch (instr[i])
                {
                    case 'L':
                        next = current.Item1;
                        break;
                    case 'R':
                        next = current.Item2;
                        break;
                    default:
                        break;
                }
                steps++;
                i++;
            }
            return steps;
        }
    }
}
