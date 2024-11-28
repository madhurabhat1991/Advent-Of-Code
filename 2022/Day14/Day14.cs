using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;

namespace _2022.Day14
{
    public class Day14 : Day<(char[,], (int, int)), long, long>
    {
        public override string DayNumber { get { return "14"; } }

        public override long PartOne((char[,], (int, int)) input)
        {
            var grid = input.Item1;
            var srcPoint = input.Item2;

           return Cave(ref grid, ref srcPoint, true);
        }

        public override long PartTwo((char[,], (int, int)) input)
        {
            var grid = input.Item1;
            var srcPoint = input.Item2;

            // add 2 extra floors on the bottom
            grid = grid.AddRowsToBottom(2);
            // last but one is an air path
            for (int row = grid.GetLength(0) - 2, col = 0; col < grid.GetLength(1); col++)
            {
                grid[row, col] = Air;
            }
            // last one is a rock path - considered floor
            for (int row = grid.GetLength(0) - 1, col = 0; col < grid.GetLength(1); col++)
            {
                grid[row, col] = Rock;
            }

            return Cave(ref grid, ref srcPoint, false);
        }

        /// <summary>
        /// Process Input
        /// </summary>
        /// <param name="input"></param>
        /// <returns>(Grid, (source row, source column))</returns>
        public override (char[,], (int, int)) ProcessInput(string[] input)
        {
            // need everything in one place to find the range
            List<List<(int, int)>> paths = new List<List<(int, int)>>();
            foreach (var line in input)
            {
                List<(int, int)> path = new List<(int, int)>();
                var points = line.Split("->", StringSplitOptions.RemoveEmptyEntries);
                foreach (var point in points)
                {
                    var xy = point.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    path.Add((Int32.Parse(xy[0]), Int32.Parse(xy[1])));
                }
                paths.Add(path);
            }

            // find range of grid
            var minX = paths.Min(x => x.Min(y => y.Item1));
            var maxX = paths.Max(x => x.Max(y => y.Item1));
            var minY = paths.Min(x => x.Min(y => y.Item2));
            var maxY = paths.Max(x => x.Max(y => y.Item2));

            // offset x co-ordinate to (0 + range) - to avoid bulk grid
            for (int i = 0; i < paths.Count; i++)
            {
                var path = paths[i];
                for (int j = 0; j < path.Count; j++)
                {
                    path[j] = (path[j].Item1 - minX, path[j].Item2);
                }
            }
            char[,] grid = new char[maxY + 1, maxX - minX + 1];

            // populate grid with rocks
            foreach (var path in paths)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    for (int x = Math.Min(path[i].Item1, path[i + 1].Item1), y = Math.Min(path[i].Item2, path[i + 1].Item2);
                        (path[i].Item1 != path[i + 1].Item1 ? x <= Math.Max(path[i].Item1, path[i + 1].Item1) : y <= Math.Max(path[i].Item2, path[i + 1].Item2));
                        x += path[i].Item1 != path[i + 1].Item1 ? 1 : 0, y += path[i].Item2 != path[i + 1].Item2 ? 1 : 0)
                    {
                        grid[y, x] = Rock;
                    }
                }
            }

            // fill with air
            grid.MarkGrid(Char.MinValue, Air);

            // add sand source - easy to debug visually
            var srcPoint = (0, 500 - minX);  // (row, column) vs (y, x)
            grid[0, 500 - minX] = Source;

            //grid.Print();
            return (grid, srcPoint);
        }

        private const char Rock = '#';
        private const char Air = '.';
        private const char Source = '+';
        private const char Sand = 'o';

        /// <summary>
        /// Cave system
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="srcPoint">(source row, source column)</param>
        /// <param name="abyssExists">true if abyss exists (part 1), false otherwise</param>
        /// <returns>Sand units</returns>
        private long Cave(ref char[,] grid, ref (int, int) srcPoint, bool abyssExists)
        {
            long units = 0;
            bool abort = false;
            while (!abort)      // haven't fallen into abyss (part 1) OR haven't reached the source (part 2)
            {
                abort = DropSand(ref grid, ref srcPoint, ref units, abyssExists);
            }
            //grid.Print();
            return units;
        }

        /// <summary>
        /// Drop the sand from source
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="srcPoint">(source row, source column)</param>
        /// <param name="units">number of san units</param>
        /// <param name="abyssExists">true if abyss exists (part 1), false otherwise</param>
        /// <returns>true if fell into the abyss OR collision point has reached source point, false otherwise</returns>
        private bool DropSand(ref char[,] grid, ref (int, int) srcPoint, ref long units, bool abyssExists)
        {
            var collision = srcPoint;
            bool rest = false;
            while (!rest)       // sand hasn't rested
            {
                // drop down - from source or collision point
                collision = FindSourceBlockPoint(grid, collision);
                try
                {
                    // is down-left free?
                    var diagonal = grid.GetBottomLeftCell(collision.Item1, collision.Item2);
                    if (diagonal.Item1 == Air)
                    {
                        collision = (diagonal.Item2, diagonal.Item3);
                    }
                    else
                    {
                        try
                        {
                            // is down-right free?
                            diagonal = grid.GetBottomRightCell(collision.Item1, collision.Item2);
                            if (diagonal.Item1 == Air)
                            {
                                collision = (diagonal.Item2, diagonal.Item3);
                            }
                            else
                            {
                                // rest where you are
                                rest = true;
                                grid[collision.Item1, collision.Item2] = Sand;
                                units++;
                                //grid.Print();
                                break;
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            if (abyssExists)
                            {
                                // oops! fell into the abyss on the right
                                return true;
                            }
                            else
                            {
                                // extend column to the right
                                grid = grid.AddColumnsToRight(1);
                                // all air except a rock on floor
                                for (int row = 0, col = grid.GetLength(1) - 1; row < grid.GetLength(0) - 1; row++)
                                {
                                    grid[row, col] = Air;
                                }
                                grid[grid.GetLength(0) - 1, grid.GetLength(1) - 1] = Rock;
                                //grid.Print();
                            }
                        }
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    if (abyssExists)
                    {
                        // dang! now fell into the abyss on the left
                        return true;
                    }
                    else
                    {
                        // extend column to the left
                        grid = grid.AddColumnsToLeft(1);
                        // column of source and collision point shifts to the right
                        collision.Item2 += 1;
                        srcPoint.Item2 += 1;
                        // all air except a rock on floor
                        for (int row = 0, col = 0; row < grid.GetLength(0) - 1; row++)
                        {
                            grid[row, col] = Air;
                        }
                        grid[grid.GetLength(0) - 1, 0] = Rock;
                        //grid.Print();
                    }
                }
            }
            if (!abyssExists && collision == srcPoint)
            {
                // reached the source
                return true;
            }
            return false;
        }

        /// <summary>
        /// Find a collision point when the sand drops down
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="srcPoint">(source row, source column)</param>
        /// <returns>(collision row, collision column)</returns>
        private (int, int) FindSourceBlockPoint(char[,] grid, (int, int) srcPoint)
        {
            (int, int) collision = (srcPoint.Item1, srcPoint.Item2);
            for (int row = collision.Item1; row < grid.GetLength(0) - 1; row++)
            {
                if (new char[] { Rock, Sand }.Contains(grid[row + 1, collision.Item2]))
                {
                    collision = (row, collision.Item2);
                    break;
                }
            }
            return collision;
        }
    }
}
