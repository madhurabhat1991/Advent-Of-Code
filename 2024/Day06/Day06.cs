using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;

namespace _2024.Day06
{
    public class Day06 : Day<(char[,], (int, int)), long, long>
    {
        public override string DayNumber { get { return "06"; } }

        public override long PartOne((char[,], (int, int)) input)
        {
            var grid = input.Item1;
            var guard = input.Item2;
            HashSet<(int, int)> pathPoints = new HashSet<(int, int)>();     // vistied (row, col)
            string direction = Up;
            (char, int, int) next = (Char.MinValue, 0, 0);

            while (true)    // until exit
            {
                pathPoints.Add(guard);              // including starting position
                int r = guard.Item1, c = guard.Item2;
                if (direction == Up)
                {
                    if (r == 0) { break; }          // exit
                    else
                    {
                        next = grid.GetTopCell(r, c);
                        if (next.Item1 == '#') { direction = Right; continue; }     // obstacle
                    }
                }
                else if (direction == Right)
                {
                    if (c == grid.GetLength(1) - 1) { break; }
                    else
                    {
                        next = grid.GetRightCell(r, c);
                        if (next.Item1 == '#') { direction = Down; continue; }
                    }
                }
                else if (direction == Down)
                {
                    if (r == grid.GetLength(0) - 1) { break; }
                    else
                    {
                        next = grid.GetBottomCell(r, c);
                        if (next.Item1 == '#') { direction = Left; continue; }
                    }
                }
                else if (direction == Left)
                {
                    if (c == 0) { break; }
                    else
                    {
                        next = grid.GetLeftCell(r, c);
                        if (next.Item1 == '#') { direction = Up; continue; }
                    }
                }
                guard = (next.Item2, next.Item3);   // step
            }
            return pathPoints.Count;
        }

        public override long PartTwo((char[,], (int, int)) input)
        {
            var grid = input.Item1;
            var guard = input.Item2;
            HashSet<(int, int)> pathPoints = new HashSet<(int, int)>();
            string direction = Up;
            (char, int, int) next = (Char.MinValue, 0, 0);

            // first find all the points in guard's path if there was no 'extra' obstacle
            while (true)    // until exit
            {
                int r = guard.Item1, c = guard.Item2;
                if (direction == Up)
                {
                    if (r == 0) { break; }
                    else
                    {
                        next = grid.GetTopCell(r, c);
                        if (next.Item1 == '#') { direction = Right; continue; }
                    }
                }
                else if (direction == Right)
                {
                    if (c == grid.GetLength(1) - 1) { break; }
                    else
                    {
                        next = grid.GetRightCell(r, c);
                        if (next.Item1 == '#') { direction = Down; continue; }
                    }
                }
                else if (direction == Down)
                {
                    if (r == grid.GetLength(0) - 1) { break; }
                    else
                    {
                        next = grid.GetBottomCell(r, c);
                        if (next.Item1 == '#') { direction = Left; continue; }
                    }
                }
                else if (direction == Left)
                {
                    if (c == 0) { break; }
                    else
                    {
                        next = grid.GetLeftCell(r, c);
                        if (next.Item1 == '#') { direction = Up; continue; }
                    }
                }
                guard = (next.Item2, next.Item3);
                pathPoints.Add(guard);      // exclude starting position
            }

            // use guard's initial path to introduce one obstacle
            long loops = 0;
            foreach (var obstacle in pathPoints)
            {
                guard = input.Item2;
                direction = Up;
                next = (Char.MinValue, 0, 0);
                HashSet<(int, int, int, int, string)> paths = new HashSet<(int, int, int, int, string)>();  // visited (from row, from, col, to row, to col, direction)
                (int, int) pathStart = guard;               // visited from

                while (true)                                // until exit or loop
                {
                    int r = guard.Item1, c = guard.Item2;   // current position
                    if (direction == Up)
                    {
                        if (r == 0) { break; }              // exit
                        else
                        {
                            next = grid.GetTopCell(r, c);
                            if (next.Item1 == '#' || (next.Item2, next.Item3) == obstacle)                  // obstacle
                            {
                                if (paths.Add((pathStart.Item1, pathStart.Item2, r, c, direction)))         // add visited from, to and direction before turning
                                {
                                    pathStart = (r, c);
                                    direction = Right;
                                    continue;
                                }
                                else                        // already visited from - to in same direction - will loop
                                {
                                    loops++;
                                    break;
                                }
                            }
                        }
                    }
                    else if (direction == Right)            // all others are same as Up
                    {
                        if (c == grid.GetLength(1) - 1) { break; }
                        else
                        {
                            next = grid.GetRightCell(r, c);
                            if (next.Item1 == '#' || (next.Item2, next.Item3) == obstacle)
                            {
                                if (paths.Add((pathStart.Item1, pathStart.Item2, r, c, direction)))
                                {
                                    pathStart = (r, c);
                                    direction = Down;
                                    continue;
                                }
                                else
                                {
                                    loops++;
                                    break;
                                }
                            }
                        }
                    }
                    else if (direction == Down)
                    {
                        if (r == grid.GetLength(0) - 1) { break; }
                        else
                        {
                            next = grid.GetBottomCell(r, c);
                            if (next.Item1 == '#' || (next.Item2, next.Item3) == obstacle)
                            {
                                if (paths.Add((pathStart.Item1, pathStart.Item2, r, c, direction)))
                                {
                                    pathStart = (r, c);
                                    direction = Left;
                                    continue;
                                }
                                else
                                {
                                    loops++;
                                    break;
                                }
                            }
                        }
                    }
                    else if (direction == Left)
                    {
                        if (c == 0) { break; }
                        else
                        {
                            next = grid.GetLeftCell(r, c);
                            if (next.Item1 == '#' || (next.Item2, next.Item3) == obstacle)
                            {
                                if (paths.Add((pathStart.Item1, pathStart.Item2, r, c, direction)))
                                {
                                    pathStart = (r, c);
                                    direction = Up;
                                    continue;
                                }
                                else
                                {
                                    loops++;
                                    break;
                                }
                            }
                        }
                    }
                    guard = (next.Item2, next.Item3);       // step
                }
            }
            return loops;
        }

        public override (char[,], (int, int)) ProcessInput(string[] input)
        {
            var grid = input.CreateGrid2D();
            var start = grid.GetCellsEqualToValue('^')[0];
            var guard = (start.Item2, start.Item3);
            return (grid, guard);
        }

        private readonly string Up = "Up";
        private readonly string Right = "Right";
        private readonly string Down = "Down";
        private readonly string Left = "Left";
    }
}
