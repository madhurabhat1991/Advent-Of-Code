using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;

namespace _2025.Day05
{
    public class Day05 : Day<(List<(long, long)>, List<long>), long, long>
    {
        public override string DayNumber { get { return "05"; } }

        public override long PartOne((List<(long, long)>, List<long>) input)
        {
            long count = 0;
            foreach (var id in input.Item2)
            {
                foreach (var range in input.Item1)
                {
                    if (id >= range.Item1 && id <= range.Item2)
                    {
                        count++;
                        break;
                    }
                }
            }
            return count;
        }

        public override long PartTwo((List<(long, long)>, List<long>) input)
        {
            var sorted = input.Item1.OrderBy(r => r.Item1).ToList();
            int mergeCounts = 0;
            do
            {
                // reset count
                mergeCounts = 0;
                List<(long, long)> merged = new List<(long, long)>();
                merged.Add(sorted[0]);
                for (var i = 1; i < sorted.Count(); i++)
                {
                    var current = sorted[i];
                    var last = merged[merged.Count - 1];
                    // merge: if start of current is within or on the boundary of last merged end value then use the max of end values
                    if (current.Item1 <= last.Item2 + 1)
                    {
                        merged[merged.Count - 1] = (last.Item1, Math.Max(current.Item2, last.Item2));
                        mergeCounts++;
                    }
                    else
                    {
                        merged.Add(current);
                    }
                }
                // sort for next
                sorted = merged.OrderBy(r => r.Item1).ToList();
            } while (mergeCounts > 0);  // until none merges
            return sorted.Sum(r => (r.Item2 - r.Item1 + 1));
        }

        public override (List<(long, long)>, List<long>) ProcessInput(string[] input)
        {
            var blocks = input.Blocks();
            List<(long, long)> ranges = new List<(long, long)>();
            foreach (var range in blocks[0])
            {
                var splits = range.Split('-', StringSplitOptions.RemoveEmptyEntries);
                ranges.Add((Int64.Parse(splits[0]), Int64.Parse(splits[1])));
            }
            List<long> ids = new List<long>();
            foreach (var id in blocks[1])
            {
                ids.Add(Int64.Parse(id));
            }
            return (ranges, ids);
        }
    }
}
