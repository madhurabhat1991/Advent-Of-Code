using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;

namespace _2024.Day12
{
    public class Day12 : Day<char[,], long, long>
    {
        public override string DayNumber { get { return "12"; } }

        public override long PartOne(char[,] input)
        {
            // scan for regions, cluster them
            // for each point if it is not in any region, using while loop and hashset until all its neighbors with same type have been discovered
            // there can be many reqions with same letter so don't go by letter
            // for each region, find area and perimeter
            HashSet<(int, int)> visited = new HashSet<(int, int)>();
            long cost = 0;
            for (int r = 0; r < input.GetLength(0); r++)
            {
                for (int c = 0; c < input.GetLength(1); c++)
                {
                    if (!visited.Contains((r, c)))
                    {
                        // forming a region
                        long area = 0, perimeter = 0;
                        Queue<(int, int)> toVisit = new Queue<(int, int)>([(r, c)]);
                        while (toVisit.Count > 0)
                        {
                            // while visiting accumulate area and perimeter
                            var visiting = toVisit.Dequeue();
                            area += 1;
                            if (visiting.Item1 == 0 || !(input.GetTopCell(visiting.Item1, visiting.Item2).Item1 == input[r, c])) { perimeter++; }
                            if (visiting.Item2 == input.GetLength(1) - 1 || !(input.GetRightCell(visiting.Item1, visiting.Item2).Item1 == input[r, c])) { perimeter++; }
                            if (visiting.Item1 == input.GetLength(0) - 1 || !(input.GetBottomCell(visiting.Item1, visiting.Item2).Item1 == input[r, c])) { perimeter++; }
                            if (visiting.Item2 == 0 || !(input.GetLeftCell(visiting.Item1, visiting.Item2).Item1 == input[r, c])) { perimeter++; }
                            visited.Add((visiting.Item1, visiting.Item2));
                            // discover neighboring plots
                            var neighbors = input.GetNeighbors(visiting.Item1, visiting.Item2, includeDiagonal: false)
                                .Where(x => x.Item1 == input[r, c] && !visited.Contains((x.Item2, x.Item3))).ToList();
                            neighbors.ForEach(x =>
                            {
                                if (!toVisit.Contains((x.Item2, x.Item3))) { toVisit.Enqueue((x.Item2, x.Item3)); }
                            });
                        }
                        cost += area * perimeter;
                    }
                }
            }
            return cost;
        }

        public override long PartTwo(char[,] input)
        {
            // find boundries along with direction (top/bottom/left/right) instead of adding perimeter
            // for each direction, group boundries by rows/cols, find contiguous ones => each contiguous = side
            HashSet<(int, int)> visited = new HashSet<(int, int)>();
            long cost = 0;
            for (int r = 0; r < input.GetLength(0); r++)
            {
                for (int c = 0; c < input.GetLength(1); c++)
                {
                    if (!visited.Contains((r, c)))
                    {
                        // forming a region
                        long area = 0;
                        HashSet<(int, int, char)> boundries = new HashSet<(int, int, char)>(); // HashSet<(row, col, direction)>
                        Queue<(int, int)> toVisit = new Queue<(int, int)>([(r, c)]);
                        while (toVisit.Count > 0)
                        {
                            // while visiting accumulate area and boundries
                            var visiting = toVisit.Dequeue();
                            area += 1;
                            if (visiting.Item1 == 0 || !(input.GetTopCell(visiting.Item1, visiting.Item2).Item1 == input[r, c])) { boundries.Add((visiting.Item1, visiting.Item2, Top)); }
                            if (visiting.Item2 == input.GetLength(1) - 1 || !(input.GetRightCell(visiting.Item1, visiting.Item2).Item1 == input[r, c])) { boundries.Add((visiting.Item1, visiting.Item2, Right)); }
                            if (visiting.Item1 == input.GetLength(0) - 1 || !(input.GetBottomCell(visiting.Item1, visiting.Item2).Item1 == input[r, c])) { boundries.Add((visiting.Item1, visiting.Item2, Bottom)); }
                            if (visiting.Item2 == 0 || !(input.GetLeftCell(visiting.Item1, visiting.Item2).Item1 == input[r, c])) { boundries.Add((visiting.Item1, visiting.Item2, Left)); }
                            visited.Add((visiting.Item1, visiting.Item2));
                            // discover neighboring plots
                            var neighbors = input.GetNeighbors(visiting.Item1, visiting.Item2, includeDiagonal: false)
                                .Where(x => x.Item1 == input[r, c] && !visited.Contains((x.Item2, x.Item3))).ToList();
                            neighbors.ForEach(x =>
                            {
                                if (!toVisit.Contains((x.Item2, x.Item3))) { toVisit.Enqueue((x.Item2, x.Item3)); }
                            });
                        }
                        // find sides based on boundries
                        long sides = 0;
                        var tops = boundries.Where(x => x.Item3 == Top).OrderBy(x => x.Item2).GroupBy(x => x.Item1).ToList();
                        foreach (var each in tops) { sides += Contiguous(each.Select(x => x.Item2).ToList()); }
                        var rights = boundries.Where(x => x.Item3 == Right).OrderBy(x => x.Item1).GroupBy(x => x.Item2).ToList();
                        foreach (var each in rights) { sides += Contiguous(each.Select(x => x.Item1).ToList()); }
                        var bottoms = boundries.Where(x => x.Item3 == Bottom).OrderBy(x => x.Item2).GroupBy(x => x.Item1).ToList();
                        foreach (var each in bottoms) { sides += Contiguous(each.Select(x => x.Item2).ToList()); }
                        var lefts = boundries.Where(x => x.Item3 == Left).OrderBy(x => x.Item1).GroupBy(x => x.Item2).ToList();
                        foreach (var each in lefts) { sides += Contiguous(each.Select(x => x.Item1).ToList()); }
                        cost += area * sides;
                    }
                }
            }
            return cost;
        }

        public override char[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D();
        }

        private readonly char Top = 't';
        private readonly char Right = 'r';
        private readonly char Bottom = 'b';
        private readonly char Left = 'l';

        private int Contiguous(List<int> numbers)
        {
            int counts = 1;
            for (int i = 1; i < numbers.Count; i++)
            {
                if (numbers[i] - numbers[i - 1] > 1) { counts++; }
            }
            return counts;
        }
    }
}
