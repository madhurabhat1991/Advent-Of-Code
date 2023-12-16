using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2023.Day16
{
    public class Day16 : Day<char[,], long, long>
    {
        public override string DayNumber { get { return "16"; } }

        public override long PartOne(char[,] input)
        {
            var edge = ((0, -1), (0, 0));   // top left to right
            return EnergizeTiles(input, edge);
        }

        public override long PartTwo(char[,] input)
        {
            Dictionary<((int, int), (int, int)), long> edges = new Dictionary<((int, int), (int, int)), long>();    // Dictionary<edge, #tiles>
            for (int r = 0; r < input.GetLength(0); r++)    // consider corners in left and right edges
            {
                edges.Add(((r, -1), (r, 0)), 0);
                edges.Add(((r, input.GetLength(1)), (r, input.GetLength(1) - 1)), 0);
            }
            for (int c = 1; c < input.GetLength(1); c++)
            {
                edges.Add(((-1, c), (0, c)), 0);
                edges.Add(((input.GetLength(0), c), (input.GetLength(0) - 1, c)), 0);
            }
            foreach (var edge in edges.Keys.ToList())
            {
                edges[edge] = EnergizeTiles(input, edge);
            }
            return edges.Max(r => r.Value);
        }

        public override char[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D();
        }

        private long EnergizeTiles(char[,] input, ((int, int), (int, int)) edge)
        {
            HashSet<(int, int)> tiles = new HashSet<(int, int)>();                                                  // tiles visited
            Dictionary<(int, int), HashSet<(int, int)>> paths = new Dictionary<(int, int), HashSet<(int, int)>>();  // Dictionary<from, HashSet<to>>
            Queue<((int, int), (int, int))> beams = new Queue<((int, int), (int, int))>();                          // Queue<(from, to)>
            beams.Enqueue(edge);                                                                                    // start at edge

            // for each beam - track the path until its next path is already visited
            while (beams.Any())
            {
                var beam = beams.Dequeue();
                (int, int) from = beam.Item1, to = beam.Item2;
                while (!(paths.ContainsKey(from) && paths[from].Contains(to)))
                {
                    if (to.Item1 < 0 || to.Item1 > input.GetLength(0) - 1 || to.Item2 < 0 || to.Item2 > input.GetLength(1) - 1) { break; }    // out of range
                    tiles.Add(to);
                    if (paths.ContainsKey(from)) { paths[from].Add(to); }
                    else { paths.Add(from, new HashSet<(int, int)>() { to }); }
                    (int, int) next = (0, 0);
                    // find next path
                    switch (input[to.Item1, to.Item2])
                    {
                        case '.':
                            if (from.Item1 == to.Item1)
                            {
                                next.Item1 = to.Item1;
                                next.Item2 = to.Item2 + Math.Sign(to.Item2 - from.Item2);
                            }
                            else
                            {
                                next.Item1 = to.Item1 + Math.Sign(to.Item1 - from.Item1);
                                next.Item2 = to.Item2;
                            }
                            break;
                        case '/':
                            if (from.Item1 == to.Item1)
                            {
                                next.Item1 = to.Item1 - Math.Sign(to.Item2 - from.Item2);
                                next.Item2 = to.Item2;
                            }
                            else
                            {
                                next.Item1 = to.Item1;
                                next.Item2 = to.Item2 - Math.Sign(to.Item1 - from.Item1);
                            }
                            break;
                        case '\\':
                            if (from.Item1 == to.Item1)
                            {
                                next.Item1 = to.Item1 + Math.Sign(to.Item2 - from.Item2);
                                next.Item2 = to.Item2;
                            }
                            else
                            {
                                next.Item1 = to.Item1;
                                next.Item2 = to.Item2 + Math.Sign(to.Item1 - from.Item1);
                            }
                            break;
                        case '|':
                            if (from.Item1 == to.Item1)
                            {
                                // first beam like /
                                next.Item1 = to.Item1 - Math.Sign(to.Item2 - from.Item2);
                                next.Item2 = to.Item2;
                                // second beam like \
                                beams.Enqueue(((to.Item1, to.Item2), ((to.Item1 + Math.Sign(to.Item2 - from.Item2)), to.Item2)));
                            }
                            else
                            {
                                next.Item1 = to.Item1 + Math.Sign(to.Item1 - from.Item1);
                                next.Item2 = to.Item2;
                            }
                            break;
                        case '-':
                            if (from.Item1 == to.Item1)
                            {
                                next.Item1 = to.Item1;
                                next.Item2 = to.Item2 + Math.Sign(to.Item2 - from.Item2);
                            }
                            else
                            {
                                // first beam like /
                                next.Item1 = to.Item1;
                                next.Item2 = to.Item2 - Math.Sign(to.Item1 - from.Item1);
                                // second beam like \
                                beams.Enqueue(((to.Item1, to.Item2), (to.Item1, (to.Item2 + Math.Sign(to.Item1 - from.Item1)))));
                            }
                            break;
                        default:
                            break;
                    }
                    from = to;
                    to = next;
                }
            }
            return tiles.Count;
        }
    }
}
