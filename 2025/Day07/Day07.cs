using Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Template;

namespace _2025.Day07
{
    public class Day07 : Day<char[,], long, long>
    {
        public override string DayNumber { get { return "07"; } }

        public override long PartOne(char[,] input)
        {
            // find start - (S, row, col)
            var start = input.GetCellsEqualToValue(Start)[0];
            // beams - Queue<(value, row, col)>
            Queue<(char, int, int)> beams = new Queue<(char, int, int)>();
            beams.Enqueue(start);
            // splits - HashSet<(^, row, col)>
            HashSet<(char, int, int)> splits = new HashSet<(char, int, int)>();
            while (beams.Count > 0)
            {
                var beam = beams.Dequeue();
                // next cell downwards
                (char, int, int) next;
                if (beam.Item2 < input.GetLength(0) - 1)
                {
                    next = input.GetBottomCell(beam.Item2, beam.Item3);
                }
                else
                {
                    continue;
                }
                // empty - continue
                if (next.Item1 == Empty)
                {
                    if (!beams.Contains(next)) { beams.Enqueue(next); }
                }
                // splitter - split
                if (next.Item1 == Splitter)
                {
                    // split left
                    if (next.Item3 > 0)
                    {
                        var split1 = input.GetLeftCell(next.Item2, next.Item3);
                        // split right
                        if (next.Item3 < input.GetLength(1) - 1)
                        {
                            var split2 = input.GetRightCell(next.Item2, next.Item3);
                            if (!beams.Contains(split1)) { beams.Enqueue(split1); }
                            if (!beams.Contains(split2)) { beams.Enqueue(split2); }
                            splits.Add(next);
                        }
                    }
                }
            }
            return splits.Count;
        }

        public override long PartTwo(char[,] input)
        {
            // took long when I tried keeping track of beam paths that generated in each step
            // each step results in timelines
            // start results in 1, empty carries past timeline, splitter carries past timeline to both sides, total in last line

            // grid to track timelines after each split
            long[,] grid = new long[input.GetLength(0), input.GetLength(1)];
            // find start - (S, row, col)
            var start = input.GetCellsEqualToValue(Start)[0];
            // start results in 1 timeline
            grid[start.Item2, start.Item3] = 1;
            // beams - Queue<(value, row, col)>
            Queue<(char, int, int)> beams = new Queue<(char, int, int)>();
            beams.Enqueue(start);
            long timelines = 0;
            while (beams.Count > 0)
            {
                var beam = beams.Dequeue();
                // next cell downwards
                (char, int, int) next;
                if (beam.Item2 < input.GetLength(0) - 1)
                {
                    next = input.GetBottomCell(beam.Item2, beam.Item3);
                }
                else
                {
                    timelines += grid[beam.Item2, beam.Item3];
                    continue;
                }
                // empty - continue
                if (next.Item1 == Empty)
                {
                    if (!beams.Contains(next)) { beams.Enqueue(next); }
                    grid[next.Item2, next.Item3] += grid[beam.Item2, beam.Item3];
                }
                // splitter - split
                if (next.Item1 == Splitter)
                {
                    // split left
                    if (next.Item3 > 0)
                    {
                        var split1 = input.GetLeftCell(next.Item2, next.Item3);
                        // split right
                        if (next.Item3 < input.GetLength(1) - 1)
                        {
                            var split2 = input.GetRightCell(next.Item2, next.Item3);
                            if (!beams.Contains(split1)) { beams.Enqueue(split1); }
                            if (!beams.Contains(split2)) { beams.Enqueue(split2); }
                            grid[split1.Item2, split1.Item3] += grid[beam.Item2, beam.Item3];
                            grid[split2.Item2, split2.Item3] += grid[beam.Item2, beam.Item3];
                        }
                    }
                }
                //grid.Print(false);
            }
            return timelines;
        }

        public override char[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D();
        }

        private const char Start = 'S';
        private const char Empty = '.';
        private const char Splitter = '^';
    }
}
