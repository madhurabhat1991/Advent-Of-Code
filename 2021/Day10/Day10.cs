using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;

namespace _2021.Day10
{
    public class Day10 : Day<List<String>, long, long>
    {
        public override string DayNumber { get { return "10"; } }

        public override long PartOne(List<string> input)
        {
            long points = 0;
            foreach (var line in input)
            {
                var syntax = SyntaxChecker(line, true, false);
                points += syntax.Item1 ? syntax.Item2 : 0;
            }
            return points;
        }

        public override long PartTwo(List<string> input)
        {
            List<long> points = new List<long>();
            foreach (var line in input)
            {
                var syntax = SyntaxChecker(line, false, true);
                if (!syntax.Item1) { points.Add(syntax.Item2); }
            }
            points.Sort();
            return points[points.Count / 2];
        }

        public override List<string> ProcessInput(string[] input)
        {
            return input.ToList();
        }

        private Dictionary<Char, Char> Syntax = new Dictionary<char, char>()
        {
            { ')', '(' },
            { ']', '[' },
            { '}', '{' },
            { '>', '<' }
        };

        private Dictionary<Char, Int32> CorruptPoints = new Dictionary<char, int>()
        {
            { ')', 3 },
            { ']', 57 },
            { '}', 1197 },
            { '>', 25137 }
        };

        private Dictionary<Char, Int32> CompletePoints = new Dictionary<char, int>()
        {
            { ')', 1 },
            { ']', 2 },
            { '}', 3 },
            { '>', 4 }
        };

        private (bool, long) SyntaxChecker(String line, bool checkCorruption, bool checkCompletion)
        {
            Stack<char> stack = new Stack<char>();
            bool corrupt = false;
            long score = 0;

            foreach (var c in line)
            {
                if (!Syntax.ContainsKey(c))
                {
                    stack.Push(c);
                }
                else
                {
                    if (stack.Peek() == Syntax[c])
                    {
                        stack.Pop();
                    }
                    else
                    {
                        corrupt = true;
                        if (checkCorruption)
                        {
                            score = CorruptPoints[c];
                        }
                        return (corrupt, score);
                    }
                }
            }

            score = 0;
            if (checkCompletion && !corrupt)
            {
                while (stack.Any())
                {
                    var open = stack.Pop();
                    var close = Syntax.Where(x => x.Value == open).FirstOrDefault().Key;
                    score = score * 5 + CompletePoints[close];
                }
            }

            return (corrupt, score);
        }
    }
}
