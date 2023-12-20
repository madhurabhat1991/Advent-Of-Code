using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2023.Day19
{
    public class Day19 : Day<(Dictionary<string, string[]>, List<Dictionary<char, int>>), long, long>
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

        public override long PartTwo((Dictionary<string, string[]>, List<Dictionary<char, int>>) input)
        {
            UInt64 combinations = 1;
            var workflows = input.Item1;
            List<Range> ranges = new List<Range>();

            Queue<Range> queue = new Queue<Range>();
            queue.Enqueue(new Range { Rules = workflows["in"].ToList() });
            while (queue.Any())
            {
                var eval = queue.Dequeue();
                var rule = eval.Rules[0];
                Range more = null;
                if (eval.Rules.Count > 1)
                {
                    more = eval.DeepClone();
                    more.Rules.RemoveAt(0);
                }
                if (rule.Length == 1 && rule[0] == 'A')
                {
                    //combinations *= ((eval.X.Item2 - eval.X.Item1) * (eval.M.Item2 - eval.M.Item1)
                    //    * (eval.A.Item2 - eval.A.Item1) * (eval.S.Item2 - eval.S.Item1));
                    ranges.Add(eval);
                    continue;
                }
                if (rule.Length == 1 && rule[0] == 'R')
                {
                    continue;
                }
                var operation = rule.Split(new char[] { '<', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (operation.Length > 1)
                {
                    int limit = Int32.Parse(operation[1]);
                    switch (operation[0][0])
                    {
                        case 'x':
                            if (rule.Contains('<') && limit < eval.X.Item2)
                            {
                                eval.X.Item2 = limit;
                                if (more != null && limit + 1 > more.X.Item1) { more.X.Item1 = limit + 1; }
                            }
                            else if (rule.Contains('>') && limit > eval.X.Item1)
                            {
                                eval.X.Item1 = limit;
                                if (more != null && limit - 1 < more.X.Item2) { more.X.Item2 = limit - 1; }
                            }
                            break;
                        case 'm':
                            if (rule.Contains('<') && limit < eval.M.Item2)
                            {
                                eval.M.Item2 = limit;
                                if (more != null && limit + 1 > more.M.Item1) { more.M.Item1 = limit + 1; }
                            }
                            else if (rule.Contains('>') && limit > eval.M.Item1)
                            {
                                eval.M.Item1 = limit;
                                if (more != null && limit - 1 < more.M.Item2) { more.M.Item2 = limit - 1; }
                            }
                            break;
                        case 'a':
                            if (rule.Contains('<') && limit < eval.A.Item2)
                            {
                                eval.A.Item2 = limit;
                                if (more != null && limit + 1 > more.A.Item1) { more.A.Item1 = limit + 1; }
                            }
                            else if (rule.Contains('>') && limit > eval.A.Item1)
                            {
                                eval.A.Item1 = limit;
                                if (more != null && limit - 1 < more.A.Item2) { more.A.Item2 = limit - 1; }
                            }
                            break;
                        case 's':
                            if (rule.Contains('<') && limit < eval.S.Item2)
                            {
                                eval.S.Item2 = limit;
                                if (more != null && limit + 1 > more.S.Item1) { more.S.Item1 = limit + 1; }
                            }
                            else if (rule.Contains('>') && limit > eval.S.Item1)
                            {
                                eval.S.Item1 = limit;
                                if (more != null && limit - 1 < more.S.Item2) { more.S.Item2 = limit - 1; }
                            }
                            break;
                        default:
                            break;
                    }
                    eval.Rules = operation[2].Length == 1 ? new List<string>() { operation[2] } : workflows[operation[2]].ToList();
                    if (more != null)
                    {
                        queue.Enqueue(more);
                    }
                }
                else
                {
                    eval.Rules = workflows[operation[0]].ToList();
                }
                queue.Enqueue(eval);
            }

            List<int> x = new List<int>(), m = new List<int>(), a = new List<int>(), s = new List<int>();
            ranges.ForEach(r =>
            {
                x.AddRange(Enumerable.Range(r.X.Item1, (r.X.Item2 - r.X.Item1 + 1)).ToList());
                m.AddRange(Enumerable.Range(r.M.Item1, (r.M.Item2 - r.M.Item1 + 1)).ToList());
                a.AddRange(Enumerable.Range(r.A.Item1, (r.A.Item2 - r.A.Item1 + 1)).ToList());
                s.AddRange(Enumerable.Range(r.S.Item1, (r.S.Item2 - r.S.Item1 + 1)).ToList());
            });
            x = x.Distinct().ToList();
            m = m.Distinct().ToList();
            a = a.Distinct().ToList();
            s = s.Distinct().ToList();
            combinations = (ulong)x.Count * (ulong)m.Count * (ulong)a.Count * (ulong)s.Count;
            return 0;
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
    [Serializable]
    public class Range
    {
        public (int, int) X = (1, 4000);
        public (int, int) M = (1, 4000);
        public (int, int) A = (1, 4000);
        public (int, int) S = (1, 4000);

        public List<string> Rules { get; set; }
    }
}
