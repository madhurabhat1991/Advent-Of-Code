using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;

namespace _2021.Day08
{
    public class Day08 : Day<List<(List<string>, List<string>)>, long, long>
    {
        public override string DayNumber { get { return "08"; } }

        public override long PartOne(List<(List<string>, List<string>)> input)
        {
            // shorthand
            // return input.Sum(set => set.Item2.Count(o => o.Length == 2 || o.Length == 3 || o.Length == 4 || o.Length == 7));

            long sum = 0;
            foreach (var set in input)
            {
                var outputs = set.Item2;
                sum += outputs.Where(o => o.Length == 2 || o.Length == 3 || o.Length == 4 || o.Length == 7).Count();
            }
            return sum;
        }

        public override long PartTwo(List<(List<string>, List<string>)> input)
        {
            long sum = 0;
            foreach (var set in input)
            {
                var inputs = set.Item1;
                var outputs = set.Item2;

                Dictionary<int, string> digits = new Dictionary<int, string>();
                digits.Add(1, inputs.Where(r => r.Length == 2).FirstOrDefault());
                digits.Add(4, inputs.Where(r => r.Length == 4).FirstOrDefault());
                digits.Add(7, inputs.Where(r => r.Length == 3).FirstOrDefault());
                digits.Add(8, inputs.Where(r => r.Length == 7).FirstOrDefault());

                var temp = inputs.Where(r => r.Length == 6).ToList();
                temp.ForEach(r =>
                {
                    var diff = digits[8].Except(r).ToList();
                    if (digits[1].Contains(diff.FirstOrDefault()))
                    {
                        digits.Add(6, r);
                    }
                    else if (digits[4].Contains(diff.FirstOrDefault()))
                    {
                        digits.Add(0, r);
                    }
                    else
                    {
                        digits.Add(9, r);
                    }
                });

                temp = inputs.Where(r => r.Length == 5).ToList();
                temp.ForEach(r =>
                {
                    if (!digits[8].Except(digits[4].Union(r).Distinct()).ToList().Any())
                    {
                        digits.Add(2, r);
                    }
                    else if (digits[8].Except(digits[6].Union(r).Distinct()).ToList().Any())
                    {
                        digits.Add(5, r);
                    }
                    else
                    {
                        digits.Add(3, r);
                    }
                });

                string display = "";
                foreach (var output in outputs)
                {
                    display += digits.Where(d => d.Value.Length == output.Length
                                        && !d.Value.Except(output).ToList().Any()).FirstOrDefault().Key.ToString();
                }
                sum += Int64.Parse(display);
            }
            return sum;
        }

        public override List<(List<string>, List<string>)> ProcessInput(string[] input)
        {
            List<(List<string>, List<string>)> sets = new List<(List<string>, List<string>)>();
            while (input.Any())
            {
                var inOuts = input[0].Split(" | ", StringSplitOptions.RemoveEmptyEntries).ToList();
                List<String> inputs = inOuts[0].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                List<String> outputs = inOuts[1].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                sets.Add((inputs, outputs));
                input = input.Skip(1).ToArray();
            }
            return sets;
        }
    }

}
