using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;
using Helpers;

namespace _2021.Day14
{
    public class Day14 : Day<(Dictionary<String, long>, Dictionary<String, Char>), long, long>
    {
        public override string DayNumber { get { return "14"; } }

        public override long PartOne((Dictionary<String, long>, Dictionary<String, Char>) input)
        {
            return Polymerize(input, StepsOne);
        }

        public override long PartTwo((Dictionary<String, long>, Dictionary<String, Char>) input)
        {
            return Polymerize(input, StepsTwo);
        }

        public override (Dictionary<String, long>, Dictionary<String, Char>) ProcessInput(string[] input)
        {
            Dictionary<String, long> pairs = new Dictionary<string, long>();
            var template = input[0];
            for (int i = 0; i < template.Length - 1; i++)
            {
                var pair = $"{template[i]}{template[i + 1]}";
                pairs.IncrementValue(pair, 1);
            }
            input = input.Skip(2).ToArray();

            Dictionary<String, Char> rules = new Dictionary<string, char>();
            while (input.Any())
            {
                var both = input[0].Split(" -> ", StringSplitOptions.RemoveEmptyEntries).ToArray();
                rules.AddValue(both[0], both[1][0]);
                input = input.Skip(1).ToArray();
            }
            return (pairs, rules);
        }

        private const int StepsOne = 10;
        private const int StepsTwo = 40;

        private long Polymerize((Dictionary<String, long>, Dictionary<String, Char>) input, int steps)
        {
            var pairs = input.Item1;
            var rules = input.Item2;

            var firstLetter = pairs.First().Key.First();
            var lastLetter = pairs.Last().Key.Last();
            Dictionary<Char, long> letters = new Dictionary<char, long>();
            int step = 1;
            while (step <= steps)
            {
                letters = new Dictionary<char, long>();
                Dictionary<String, long> newPairs = new Dictionary<string, long>();
                foreach (var pair in pairs)
                {
                    var polymer = pair.Key;
                    var count = pair.Value;
                    var insert = rules.ContainsKey(polymer) ? rules[polymer] : Char.MinValue;
                    var newPair1 = $"{polymer[0]}{insert}";
                    var newPair2 = $"{insert}{polymer[1]}";
                    new List<String>() { newPair1, newPair2 }.ForEach(r =>
                    {
                        newPairs.IncrementValue(r, count);
                        r.ToList().ForEach(c => { letters.IncrementValue(c, count); });
                    });
                }
                pairs = newPairs;
                step++;
            }

            // we have count each letter twice except 1st and last, so halve them
            letters.DivideValue(2);

            // now we lost the 1st and last, so add one for each
            letters.IncrementValue(firstLetter, 1);
            letters.IncrementValue(lastLetter, 1);

            return (letters.MaxValue() - letters.MinValue());
        }
    }
}
