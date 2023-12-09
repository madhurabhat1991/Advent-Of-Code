using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2023.Day09
{
    public class Day09 : Day<List<List<long>>, long, long>
    {
        public override string DayNumber { get { return "09"; } }

        public override long PartOne(List<List<long>> input)
        {
            long sum = 0;
            foreach (var line in input)
            {
                List<List<long>> differences = FindSequences(line);
                sum += Extrapolate(differences);            // extrapolate last
            }
            return sum;
        }

        public override long PartTwo(List<List<long>> input)
        {
            long sum = 0;
            foreach (var line in input)
            {
                List<List<long>> differences = FindSequences(line);
                sum += Extrapolate(differences, true);      // extrapolate first
            }
            return sum;
        }

        public override List<List<long>> ProcessInput(string[] input)
        {
            List<List<long>> values = new List<List<long>>();
            foreach (var line in input)
            {
                values.Add(line.Split(' ', StringSplitOptions.RemoveEmptyEntries).StringArrayToLongList());
            }
            return values;
        }

        private List<List<long>> FindSequences(List<long> line)
        {
            List<List<long>> differences = new List<List<long>>(new List<List<long>> { new List<long>(line) });
            while (!(differences.Last().All(r => r == 0)))
            {
                List<long> diff = new List<long>();
                for (int i = 0; i < differences.Last().Count - 1; i++)
                {
                    diff.Add(differences.Last()[i + 1] - differences.Last()[i]);
                }
                differences.Add(diff);
            }
            return differences;
        }

        private long Extrapolate(List<List<long>> differences, bool first = false)
        {
            // part 2
            if (first)
            {
                for (int i = differences.Count - 1; i >= 0; i--)
                {
                    differences[i].Insert(0, differences[i].First() - (i == differences.Count - 1 ? 0 : differences[i + 1].First()));
                }
                return differences[0].First();
            }
            // part 1
            for (int i = differences.Count - 1; i >= 0; i--)
            {
                differences[i].Add(differences[i].Last() + (i == differences.Count - 1 ? 0 : differences[i + 1].Last()));
            }
            return differences[0].Last();
        }
    }
}
