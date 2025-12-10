using Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Template;

namespace _2025.Day09
{
    public class Day09 : Day<List<(int, int)>, long, long>
    {
        public override string DayNumber { get { return "09"; } }

        public override long PartOne(List<(int, int)> input)
        {
            long maxArea = 0;
            for (int i = 0; i < input.Count - 1; i++)
            {
                for (int j = i + 1; j < input.Count; j++)
                {
                    (int, int) p = input[i], q = input[j];
                    var area = (long)(Math.Abs(p.Item1 - q.Item1) + 1) * (Math.Abs(p.Item2 - q.Item2) + 1);
                    maxArea = area > maxArea ? area : maxArea;
                }
            }
            return maxArea;
        }

        public override long PartTwo(List<(int, int)> input)
        {
            //// cannot plot on grid, input is too large, need to find only points that would fall on boundary and inside the boundary
            //List<((int, int), char, char)> tiles = new List<((int, int), char, char)>();

            //// boundary points
            //for (int i = 0, j = 1; i < input.Count; i++, j++)
            //{
            //    j = i == input.Count - 1 ? 0 : j;
            //    (int, int) p = input[i], q = input[j];
            //    tiles.Add((p, Red, Corner));
            //    // Right
            //    if (p.Item2 == q.Item2 && p.Item1 < q.Item1)
            //    {
            //        for (int row = p.Item2, col = p.Item1 + 1; col < q.Item1; col++)
            //        {
            //            tiles.Add(((col, row), Green, Horizontal));
            //        }
            //    }
            //    // Left
            //    else if (p.Item2 == q.Item2 && p.Item1 > q.Item1)
            //    {
            //        for (int row = p.Item2, col = p.Item1 - 1; col > q.Item1; col--)
            //        {
            //            tiles.Add(((col, row), Green, Horizontal));
            //        }
            //    }
            //    // Down
            //    else if (p.Item1 == q.Item1 && p.Item2 < q.Item2)
            //    {
            //        for (int row = p.Item2 + 1, col = p.Item1; row < q.Item2; row++)
            //        {
            //            tiles.Add(((col, row), Green, Vertical));
            //        }
            //    }
            //    // Down
            //    else if (p.Item1 == q.Item1 && p.Item2 > q.Item2)
            //    {
            //        for (int row = p.Item2 - 1, col = p.Item1; row > q.Item2; row--)
            //        {
            //            tiles.Add(((col, row), Green, Vertical));
            //        }
            //    }
            //}

            //// inside points - unable to determine the logic when the boundary is shared as in 3rd and 5th lines
            //int minRow = input.Min(x => x.Item2) - 1, maxRow = input.Max(x => x.Item2) + 1;
            //int minCol = input.Min(x => x.Item1) - 1, maxCol = input.Max(x => x.Item1) + 1;
            //bool isBoundary = false, isInside = false;
            //for (int row = minRow; row <= maxRow; row++)
            //{
            //    for (int col = minCol; col <= maxCol; col++)
            //    {
            //        if (tiles.Any(x => x.Item1 == (col, row)) && !isInside)
            //        {
            //            // stepping or staying on a boundary
            //            isBoundary = isInside = true;
            //        }
            //        else if (isBoundary && isInside && tiles.Any(x => x.Item1 == (col, row) && !(x.Item3 == Horizontal)))
            //        {
            //            // stepping out of the loop
            //            isInside = isBoundary = false;
            //        }
            //        else if (isBoundary && !tiles.Any(x => x.Item1 == (col, row)))
            //        {
            //            // inside loop
            //            isInside = true;
            //            tiles.Add(((col, row), Green, Inside));

            //        }
            //    }
            //}

            throw new NotImplementedException();
        }

        public override List<(int, int)> ProcessInput(string[] input)
        {
            List<(int, int)> positions = new List<(int, int)>();
            foreach (var line in input)
            {
                var nums = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
                positions.Add((Int32.Parse(nums[0]), Int32.Parse(nums[1])));
            }
            return positions;
        }

        private const char Red = '#';
        private const char Green = 'X';

        private const char Corner = 'C';
        private const char Horizontal = 'H';
        private const char Vertical = 'V';
        private const char Inside = 'I';
    }
}
