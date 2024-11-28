using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;
using ProtoBuf;

namespace _2023.Day19
{
    public class Day19 : Day<(Dictionary<string, string[]>, List<Dictionary<char, int>>), long, UInt64>
    {
        public override string DayNumber { get { return "19"; } }

        public override long PartOne((Dictionary<string, string[]>, List<Dictionary<char, int>>) input)
        {
            long sum = 0;
            var workflows = input.Item1;
            var parts = input.Item2;
            foreach (var part in parts)
            {
                string next = "in";
                while (true)
                {
                    if (next.Length == 1 && next[0] == 'A')
                    {
                        sum += part.Values.Sum();
                        break;
                    }
                    else if (next.Length == 1 && next[0] == 'R')
                    {
                        break;
                    }
                    var wf = workflows[next];
                    foreach (var rule in wf)
                    {
                        var operation = rule.Split(new char[] { '<', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
                        if (operation.Length > 1)
                        {
                            if (rule.Contains('<'))
                            {
                                if (part[operation[0][0]] < Int32.Parse(operation[1]))
                                {
                                    next = operation[2];
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else if (rule.Contains('>'))
                            {
                                if (part[operation[0][0]] > Int32.Parse(operation[1]))
                                {
                                    next = operation[2];
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            next = rule;
                        }
                    }
                }
            }
            return sum;
        }

        // Concept reference - https://www.reddit.com/r/adventofcode/comments/18lwcw2/2023_day_19_an_equivalent_part_2_example_spoilers/
        public override UInt64 PartTwo((Dictionary<string, string[]>, List<Dictionary<char, int>>) input)
        {
            UInt64 combinations = 0;
            var workflows = input.Item1;

            Queue<Range> queue = new Queue<Range>();
            queue.Enqueue(new Range { Rules = workflows["in"].ToList() });
            while (queue.Any())
            {
                var branch = queue.Dequeue();
                var rule = branch.Rules[0];
                Range newBranch = null;
                if (branch.Rules.Count > 1)         // there are more rules that this branch doesn't satisfy
                {
                    newBranch = branch.DeepClone();
                    newBranch.Rules.RemoveAt(0);    // Rules[0] will be evaluated by branch, rest by newBranch
                }
                if (rule.Length == 1 && rule[0] == 'A')     // accepted
                {
                    combinations += ((ulong)(branch.X.Item2 - branch.X.Item1 + 1) * (ulong)(branch.M.Item2 - branch.M.Item1 + 1)
                        * (ulong)(branch.A.Item2 - branch.A.Item1 + 1) * (ulong)(branch.S.Item2 - branch.S.Item1 + 1));
                    continue;
                }
                if (rule.Length == 1 && rule[0] == 'R')     // rejected
                {
                    continue;
                }
                var operation = rule.Split(new char[] { '<', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (operation.Length > 1)                   // compare operation
                {
                    int limit = Int32.Parse(operation[1]);
                    switch (operation[0][0])
                    {
                        case 'x':
                            if (rule.Contains('<') && limit - 1 < branch.X.Item2)
                            {
                                branch.X.Item2 = limit - 1;
                                if (newBranch != null && limit > newBranch.X.Item1) { newBranch.X.Item1 = limit; }
                            }
                            else if (rule.Contains('>') && limit + 1 > branch.X.Item1)
                            {
                                branch.X.Item1 = limit + 1;
                                if (newBranch != null && limit < newBranch.X.Item2) { newBranch.X.Item2 = limit; }
                            }
                            break;
                        case 'm':
                            if (rule.Contains('<') && limit - 1 < branch.M.Item2)
                            {
                                branch.M.Item2 = limit - 1;
                                if (newBranch != null && limit > newBranch.M.Item1) { newBranch.M.Item1 = limit; }
                            }
                            else if (rule.Contains('>') && limit + 1 > branch.M.Item1)
                            {
                                branch.M.Item1 = limit + 1;
                                if (newBranch != null && limit < newBranch.M.Item2) { newBranch.M.Item2 = limit; }
                            }
                            break;
                        case 'a':
                            if (rule.Contains('<') && limit - 1 < branch.A.Item2)
                            {
                                branch.A.Item2 = limit - 1;
                                if (newBranch != null && limit > newBranch.A.Item1) { newBranch.A.Item1 = limit; }
                            }
                            else if (rule.Contains('>') && limit + 1 > branch.A.Item1)
                            {
                                branch.A.Item1 = limit + 1;
                                if (newBranch != null && limit < newBranch.A.Item2) { newBranch.A.Item2 = limit; }
                            }
                            break;
                        case 's':
                            if (rule.Contains('<') && limit - 1 < branch.S.Item2)
                            {
                                branch.S.Item2 = limit - 1;
                                if (newBranch != null && limit > newBranch.S.Item1) { newBranch.S.Item1 = limit; }
                            }
                            else if (rule.Contains('>') && limit + 1 > branch.S.Item1)
                            {
                                branch.S.Item1 = limit + 1;
                                if (newBranch != null && limit < newBranch.S.Item2) { newBranch.S.Item2 = limit; }
                            }
                            break;
                        default:
                            break;
                    }
                    branch.Rules = operation[2].Length == 1 ? new List<string>() { operation[2] } : workflows[operation[2]].ToList();   // resolve to the next wf/result
                    if (newBranch != null)
                    {
                        queue.Enqueue(newBranch);
                    }
                }
                else
                {
                    branch.Rules = workflows[operation[0]].ToList();    // new workflow - add its rules
                }
                queue.Enqueue(branch);
            }
            return combinations;
        }

        public override (Dictionary<string, string[]>, List<Dictionary<char, int>>) ProcessInput(string[] input)
        {
            var blocks = input.Blocks();

            Dictionary<string, string[]> workflows = new Dictionary<string, string[]>();
            foreach (var line in blocks[0])
            {
                var name = line.Substring(0, line.IndexOf('{'));
                var rules = line.Substring(line.IndexOf('{') + 1, line.Length - name.Length - 2).Split(',', StringSplitOptions.RemoveEmptyEntries);
                workflows.Add(name, rules);
            }

            List<Dictionary<char, int>> parts = new List<Dictionary<char, int>>();
            foreach (var line in blocks[1])
            {
                Dictionary<char, int> ratings = new Dictionary<char, int>();
                var info = line.Substring(1, line.Length - 2).Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var i in info)
                {
                    var r = i.Split('=', StringSplitOptions.RemoveEmptyEntries);
                    ratings.Add(r[0][0], Int32.Parse(r[1]));
                }
                parts.Add(ratings);
            }

            return (workflows, parts);
        }
    }

    [ProtoContract]
    public class Range
    {
        [ProtoMember(1)]
        public (int, int) X = (1, 4000);
        [ProtoMember(2)]
        public (int, int) M = (1, 4000);
        [ProtoMember(3)]
        public (int, int) A = (1, 4000);
        [ProtoMember(4)]
        public (int, int) S = (1, 4000);

        [ProtoMember(5)]
        public List<string> Rules { get; set; }
    }
}
