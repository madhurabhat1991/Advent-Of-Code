using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2021.Day11
{
    public class Day11 : Day<int[,], long, long>
    {
        public override string DayNumber { get { return "11"; } }

        public override long PartOne(int[,] input)
        {
            long flashes = 0;
            var grid = input;
            for (int step = 1; step <= StepsOne; step++)
            {
                flashes += OctopusLife(grid);
                //Debug(step, input);
            }
            return flashes;
        }

        public override long PartTwo(int[,] input)
        {
            long steps = 1;
            var grid = input;
            while (true)
            {
                long flashes = OctopusLife(grid);
                //Debug(Int32.Parse(steps.ToString()), grid);
                if (flashes == (input.GetLength(0) * input.GetLength(1))) { break; }
                steps++;
            }
            return steps;
        }

        public override int[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D().CharToIntGrid2D();
        }

        private int StepsOne = 100;

        private const int MinLevel = 0;
        private const int MaxLevel = 9;

        private long OctopusLife(int[,] input)
        {
            List<(int, int)> flashed = new List<(int, int)>();

            // Step 1
            input = input.IncrementValue(1);

            // Step 2
            do
            {
                var cells = input.GetCellsGreaterThanValue(MaxLevel);
                var flashing = cells.Select(r => (r.Item2, r.Item3)).ToList().Except(flashed).ToList();
                if (!flashing.Any()) { break; }
                flashed.AddRange(flashing);
                var neighbors = input.GetNeighbors(flashing, true);
                input = input.IncrementValue(neighbors, 1);
            } while (true);

            // Step 3
            input = input.SetCellsToValue(flashed, MinLevel);

            return flashed.Count;
        }

        private void Debug(int step, int[,] input)
        {
            Console.WriteLine("Step " + step);
            input.Print();
        }
    }
}
