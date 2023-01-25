using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2022.Day15
{
    public class Day15 : Day<List<((int, int), (int, int))>, long, long>
    {
        public override string DayNumber { get { return "15"; } }

        public override long PartOne(List<((int, int), (int, int))> input)
        {
            Dictionary<(int, int), char> marks = new Dictionary<(int, int), char>();
            foreach (var pair in input)
            {
                int sx = pair.Item1.Item1, sy = pair.Item1.Item2;
                int bx = pair.Item2.Item1, by = pair.Item2.Item2;

                // mark sensor and beacon positions
                marks[(sx, sy)] = 'S';
                marks[(bx, by)] = 'B';

                // mark manhattan distance coverage
                var mDist = Math.Abs(sx - bx) + Math.Abs(sy - by);
                int x1 = sx - mDist, y1 = sy, y2 = sy, xSpan = x1 + 2 * mDist, ySpan = sy - mDist;
                do
                {
                    if (y1 == YOne || y2 == YOne)           // only for interested Y axis
                    {
                        int x11 = x1;
                        while (x11 <= xSpan)                // until the horizontal span reached
                        {
                            if (!marks.ContainsKey((x11, y1))) { marks[(x11, y1)] = Mark; }
                            if (!marks.ContainsKey((x11, y2))) { marks[(x11, y2)] = Mark; }
                            x11++;
                        }
                    }
                    x1++; xSpan--;
                    y1--; y2++;
                } while ((x1, y1) != (xSpan, ySpan));       // until the coverage point reached
            }

            // check the coverage on line
            var coverage = marks.Where(x => x.Key.Item2 == YOne).Where(x => x.Value.Equals(Mark)).ToList();
            return coverage.Count();
        }

        public override long PartTwo(List<((int, int), (int, int))> input)
        {
            throw new NotImplementedException();
        }

        public override List<((int, int), (int, int))> ProcessInput(string[] input)
        {
            List<((int, int), (int, int))> pairs = new List<((int, int), (int, int))>();
            foreach (var line in input)
            {
                var sb = line.Split(":");
                var s = sb[0].Replace("Sensor at ", "").Split(",", StringSplitOptions.RemoveEmptyEntries);
                var b = sb[1].Replace(" closest beacon is at ", "").Split(",", StringSplitOptions.RemoveEmptyEntries);
                pairs.Add(((Int32.Parse(s[0].Trim().Replace("x=", "")), Int32.Parse(s[1].Trim().Replace("y=", ""))),
                    (Int32.Parse(b[0].Trim().Replace("x=", "")), Int32.Parse(b[1].Trim().Replace("y=", "")))));
            }
            return pairs;
        }

        private const char Mark = '#';
        private const int YOne = 2000000;  // example - 10, input - 2000000
    }
}
