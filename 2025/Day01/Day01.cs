using System;
using System.Collections.Generic;
using System.Text;
using Template;

namespace _2025.Day01
{
    public class Day01 : Day<List<(char, int)>, long, long>
    {
        public override string DayNumber { get { return "01"; } }

        public override long PartOne(List<(char, int)> input)
        {
            long zeros = 0;
            long position = start;
            foreach (var rotation in input)
            {
                position = operations[rotation.Item1](position, rotation.Item2);
                if (position == 0) zeros++;
            }
            return zeros;
        }

        public override long PartTwo(List<(char, int)> input)
        {
            long zeros = 0;
            long position = start;
            foreach (var rotation in input)
            {
                (position, var passes) = operations2[rotation.Item1](position, rotation.Item2);
                if (passes > 0)
                {
                    zeros += passes;
                }
                else if (position == 0)
                {
                    zeros++;
                }
            }
            return zeros;
        }

        public override List<(char, int)> ProcessInput(string[] input)
        {
            List<(char, int)> rotations = new List<(char, int)>();
            foreach (var line in input)
            {
                rotations.Add((line[0], Int32.Parse(line[1..(line.Length)])));
            }
            return rotations;
        }

        private const int start = 50;
        private const int min = 0;
        private const int max = 99;
        private const int dials = max - min + 1;
        private const char right = 'R';
        private const char left = 'L';

        /// <summary>
        /// Part 1
        /// </summary>
        private readonly Dictionary<char, Func<long, long, long>> operations = new Dictionary<char, Func<long, long, long>>()
        {
            { right, new Func<long, long, long>(Add) },
            { left, new Func<long, long, long>(Subtract) }
        };

        // add the steps and offset to number of dials
        private static readonly Func<long, long, long> Add = (a, b) => ((a + b) % dials);

        // difference of steps, ensure non-negative result by adding and modulo again
        private static readonly Func<long, long, long> Subtract = (a, b) => ((((a - b) % dials) + dials) % dials);

        /// <summary>
        /// Part 2
        /// </summary>
        private readonly Dictionary<char, Func<long, long, (long, long)>> operations2 = new Dictionary<char, Func<long, long, (long, long)>>()
        {
            { right, new Func<long, long, (long, long)>(Add2) },
            { left, new Func<long, long, (long, long)>(Subtract2) }
        };

        private static readonly Func<long, long, (long, long)> Add2 = (a, b) => (
            ((a + b) % dials),
            // number of times dial crossed 0 while adding steps
            ((a + b) / dials)
        );

        private static readonly Func<long, long, (long, long)> Subtract2 = (a, b) => (
            ((((a - b) % dials) + dials) % dials),
            // if start is not 0 & going beyond 0 then no. of times dial crossed 0 during diff; else no. of times going left
            ((a != min && (a - b) < 0) ? ((Math.Abs(a - b) / dials) + 1) : (b / dials))
        );
    }
}
