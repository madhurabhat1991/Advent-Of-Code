using Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
                    maxArea = Math.Max((long)(Math.Abs(p.Item1 - q.Item1) + 1) * (Math.Abs(p.Item2 - q.Item2) + 1), maxArea);
                }
            }
            return maxArea;
        }

        public override long PartTwo(List<(int, int)> input)
        {
            // cannot plot on grid, input is too large. too long to find walls using even/odd rule, need to find only points that would fall on boundary
            // build a fence around the boundary
            List<((int, int), char)> tiles = new List<((int, int), char)>();

            // boundary and fence points
            for (int i = 0, j = 1; i < input.Count; i++, j++)
            {
                j = i == input.Count - 1 ? 0 : j;
                (int, int) p = input[i], q = input[j];
                tiles.Add((p, Red));
                // Right
                if (p.Item2 == q.Item2 && p.Item1 < q.Item1)
                {
                    if (!tiles.Contains(((p.Item1, p.Item2 - 1), Red)) && !tiles.Contains(((p.Item1, p.Item2 - 1), Green)))
                    {
                        tiles.Add(((p.Item1, p.Item2 - 1), Fence));
                    }
                    for (int row = p.Item2, col = p.Item1 + 1; col < q.Item1; col++)
                    {
                        tiles.Add(((col, row), Green));
                        tiles.Add(((col, row - 1), Fence));
                    }
                }
                // Left
                else if (p.Item2 == q.Item2 && p.Item1 > q.Item1)
                {
                    if (!tiles.Contains(((p.Item1, p.Item2 + 1), Red)) && !tiles.Contains(((p.Item1, p.Item2 + 1), Green)))
                    {
                        tiles.Add(((p.Item1, p.Item2 + 1), Fence));
                    }
                    for (int row = p.Item2, col = p.Item1 - 1; col > q.Item1; col--)
                    {
                        tiles.Add(((col, row), Green));
                        tiles.Add(((col, row + 1), Fence));
                    }
                }
                // Down
                else if (p.Item1 == q.Item1 && p.Item2 < q.Item2)
                {
                    if (!tiles.Contains(((p.Item1 + 1, p.Item2), Red)) && !tiles.Contains(((p.Item1 + 1, p.Item2), Green)))
                    {
                        tiles.Add(((p.Item1 + 1, p.Item2), Fence));
                    }
                    for (int row = p.Item2 + 1, col = p.Item1; row < q.Item2; row++)
                    {
                        tiles.Add(((col, row), Green));
                        tiles.Add(((col + 1, row), Fence));
                    }
                }
                // Up
                else if (p.Item1 == q.Item1 && p.Item2 > q.Item2)
                {
                    if (!tiles.Contains(((p.Item1 - 1, p.Item2), Red)) && !tiles.Contains(((p.Item1 - 1, p.Item2), Green)))
                    {
                        tiles.Add(((p.Item1 - 1, p.Item2), Fence));
                    }
                    for (int row = p.Item2 - 1, col = p.Item1; row > q.Item2; row--)
                    {
                        tiles.Add(((col, row), Green));
                        tiles.Add(((col - 1, row), Fence));
                    }
                }
            }

            // if perimeter bounds of any of the potential rectangles has fence it is invalid, otherwise compare area
            long maxArea = 0;
            for (int i = 0; i < input.Count - 1; i++)
            {
                for (int j = i + 1; j < input.Count; j++)
                {
                    bool fence = false;
                    (int, int) p = input[i], q = input[j];
                    (int, int) r = (q.Item1, p.Item2), s = (p.Item1, q.Item2);
                    // top and bottom edges
                    var edges = new List<(int, int)>() { p, q, r, s }.GroupBy(x => x.Item2).ToList();
                    foreach (var edge in edges)
                    {
                        int row = edge.First().Item2, minCol = Math.Min(edge.First().Item1, edge.Last().Item1), maxCol = Math.Max(edge.First().Item1, edge.Last().Item1);
                        if (tiles.Any(x => x.Item1.Item2 == row && (x.Item1.Item1 >= minCol && x.Item1.Item1 <= maxCol) && x.Item2 == Fence))
                        {
                            fence = true; break;
                        }
                    }
                    if (!fence)
                    {
                        // right and left edges
                        edges = new List<(int, int)>() { p, q, r, s }.GroupBy(x => x.Item1).ToList();
                        foreach (var edge in edges)
                        {
                            int col = edge.First().Item1, minRow = Math.Min(edge.First().Item2, edge.Last().Item2), maxRow = Math.Max(edge.First().Item2, edge.Last().Item2);
                            if (tiles.Any(x => x.Item1.Item1 == col && (x.Item1.Item2 >= minRow && x.Item1.Item2 <= maxRow) && x.Item2 == Fence))
                            {
                                fence = true; break;
                            }
                        }
                        if (!fence)
                        {
                            maxArea = Math.Max((long)(Math.Abs(p.Item1 - q.Item1) + 1) * (Math.Abs(p.Item2 - q.Item2) + 1), maxArea);
                        }
                    }
                }
            }
            return maxArea;
            // very slow (15 mins), fetched correct answer midway on debug mode as I knew from reddit that max is midway, need to optimize
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
        private const char Fence = '^';

        private const char SouthEast = 'F';
        private const char SouthWest = '7';
        private const char NorthEast = 'L';
        private const char NorthWest = 'J';
        private const char NorthSouth = '|';
        private const char EastWest = '-';
        private const char Inside = 'I';
    }
}
