using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2023.Day21
{
    public class Day21 : Day<char[,], long, UInt64>
    {
        public override string DayNumber { get { return "21"; } }

        public override long PartOne(char[,] input)
        {
            HashSet<(int, int)> paths = new HashSet<(int, int)>();
            var start = input.GetCellsEqualToValue('S').First();
            paths.Add((start.Item2, start.Item3));
            int steps = 1;
            while (steps <= Steps1)
            {
                HashSet<(int, int)> newPaths = new HashSet<(int, int)>();
                foreach (var path in paths)
                {
                    var neighbors = input.GetNeighbors(path.Item1, path.Item2, false).Where(r => r.Item1 != '#').ToList();
                    neighbors.ForEach(r => newPaths.Add((r.Item2, r.Item3)));
                }
                paths = newPaths;
                steps++;
            }
            return paths.Count;
        }

        public override UInt64 PartTwo(char[,] input)
        {
            HashSet<(int, int)> paths = new HashSet<(int, int)>();
            var start = input.GetCellsEqualToValue('S').First();
            paths.Add((start.Item2, start.Item3));
            int steps = 1;
            while (steps <= Steps2)
            {
                HashSet<(int, int)> newPaths = new HashSet<(int, int)>();
                foreach (var path in paths)
                {
                    int row = path.Item1, col = path.Item2;
                    // top neighbor
                    int newRow = row - 1, newCol = col;
                    NeighborPlot(input, newRow, newCol, ref newPaths);
                    // bottom neighbor
                    newRow = row + 1; newCol = col;
                    NeighborPlot(input, newRow, newCol, ref newPaths);
                    // left neighbor
                    newRow = row; newCol = col - 1;
                    NeighborPlot(input, newRow, newCol, ref newPaths);
                    // right neighbor
                    newRow = row; newCol = col + 1;
                    NeighborPlot(input, newRow, newCol, ref newPaths);
                }
                paths = newPaths;
                steps++;
            }
            // print plots - for each of the example in Steps2 - to display pattern
            //int rowMin = paths.Min(r => r.Item1), rowMax = paths.Max(r => r.Item1), colMin = paths.Min(r => r.Item2), colMax = paths.Max(r => r.Item2);
            //int nRows = rowMax - rowMin + 1, nCols = colMax - colMin + 1;
            //List<(int, int)> gPlots = paths.Select(r => ((r.Item2 + (colMin * (-1))),(r.Item1 + (rowMin * (-1))))).ToList();
            //gPlots.MarkGrid('O', '.').Print(false);
            return (ulong)paths.Count;
        }

        public override char[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D();
        }

        private const int Steps1 = 64;      // 6, 64
        private const int Steps2 = 50;     // 6, 10, 50, 100, 500, 1000, 5000, 26501365

        private int RelativeTopRow(char[,] input, int newRow)
        {
            return (newRow + ((((newRow * (-1)) / input.GetLength(0)) + ((newRow * (-1)) % input.GetLength(0) == 0 ? 0 : 1)) * input.GetLength(0)));
        }

        private int RelativeBottomRow(char[,] input, int newRow)
        {
            return (newRow - ((newRow / input.GetLength(0)) * input.GetLength(0)));
        }

        private int RelativeLeftColumn(char[,] input, int newCol)
        {
            return (newCol + ((((newCol * (-1)) / input.GetLength(1)) + ((newCol * (-1)) % input.GetLength(1) == 0 ? 0 : 1)) * input.GetLength(1)));
        }

        private int RelativeRightColumn(char[,] input, int newCol)
        {
            return (newCol - ((newCol / input.GetLength(1)) * input.GetLength(1)));
        }

        private void NeighborPlot(char[,] input, int newRow, int newCol, ref HashSet<(int, int)> newPaths)
        {
            int relRow = newRow < 0 ? RelativeTopRow(input, newRow) : newRow > input.GetLength(0) - 1 ? RelativeBottomRow(input, newRow) : newRow;
            int relCol = newCol < 0 ? RelativeLeftColumn(input, newCol) : newCol > input.GetLength(1) - 1 ? RelativeRightColumn(input, newCol) : newCol;
            char neighbor = input[relRow, relCol];
            if (neighbor != '#') { newPaths.Add((newRow, newCol)); }
        }
    }
}
