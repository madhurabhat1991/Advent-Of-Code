using System;
using System.Collections.Generic;
using System.Text;
using Template;
using static _2025.Day06.Day06;

namespace _2025.Day06
{
    public class Day06 : Day<string[], long, long>
    {
        public override string DayNumber { get { return "06"; } }

        public override long PartOne(string[] input)
        {
            // parse numbers as long and create problem set (unnecessary, didn't expect part 2)
            List<List<string>> rawSplits = new List<List<string>>();
            foreach (var line in input)
            {
                var splits = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
                rawSplits.Add(splits);
            }
            List<Problem> problems = new List<Problem>(rawSplits[0].Count);
            foreach (var line in rawSplits)
            {
                bool isNum = true;
                if (!Char.IsNumber(line[0], 0)) { isNum = false; }
                for (int i = 0; i < line.Count; i++)
                {
                    if (isNum)
                    {
                        if (problems.Count < rawSplits[0].Count)
                        {
                            problems.Add(new Problem { Numbers = new List<long>() { Int64.Parse(line[i]) } });
                        }
                        else
                        {
                            problems[i].Numbers.Add(Int64.Parse(line[i]));
                        }
                    }
                    else
                    {
                        problems[i].Operator = line[i][0];
                    }
                }
            }
            // solve each problem and accumulate sum
            long sum = 0;
            foreach (var problem in problems)
            {
                sum += problem.Operator == Add ? problem.Numbers.Sum(x => x) : problem.Numbers.Aggregate((a, x) => a * x);
            }
            return sum;
        }

        public override long PartTwo(string[] input)
        {
            // start from end of last line. index of operator is the starting index of a problem.
            // List<(startIndex, endIndex, operator)>
            List<(int, int, char)> pIndices = new List<(int, int, char)>();
            var lastLine = input[input.Length - 1];
            for (int i = lastLine.Length - 1, j = lastLine.Length - 1, pNum = 1; i >= 0; i--)
            {
                var current = lastLine[i];
                if (current == Add || current == Multiply)
                {
                    pIndices.Add((i, j, current));
                    j = i - 2;  // go to end of next problem
                    i--;        // go to end of next problem, loop decrements another
                    pNum++;     // next problem number
                }
            }
            // perform operation using indices of each problem, accumulate sum
            long sum = 0;
            foreach (var pIndex in pIndices)
            {
                var op = pIndex.Item3;
                List<long> numbers = new List<long>();
                for (int index = pIndex.Item2; index >= pIndex.Item1; index--)
                {
                    string number = "";
                    for (int line = 0; line < input.Length - 1; line++)
                    {
                        var digit = input[line][index];
                        if (digit != ' ') { number += digit; }
                    }
                    numbers.Add(Int64.Parse(number));
                }
                sum += op == Add ? numbers.Sum(x => x) : numbers.Aggregate((a, x) => a * x);
            }
            return sum;
        }

        public override string[] ProcessInput(string[] input)
        {
            // Processed input to Problem class for part 1
            // Need to preserve indents for part 2
            // Moving initial process input logic to part 1, using input as it is
            return input;
        }

        private const char Add = '+';
        private const char Multiply = '*';

        public class Problem
        {
            public List<long> Numbers = [];
            public char Operator;
        }
    }
}
