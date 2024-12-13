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
            throw new NotImplementedException();
        }

        public override char[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D();
        }
    }

    public class Region
    {
        public Region(char plotname)
        {
            PlotName = plotname;
            Locations = new List<(int, int)>();
        }
        public char PlotName { get; set; }
        public long Area { get; set; }
        public long Perimeter { get; set; }
        public List<(int, int)> Locations { get; set; }
    }
}
