using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2021.Day15
{
    public class Day15 : Day<int[,], long, long>
    {
        public override string DayNumber { get { return "15"; } }

        public override long PartOne(int[,] input)
        {
            return GetShortestPath(input);
        }

        public override long PartTwo(int[,] input)
        {
            var multiplier = 5;
            int[,] inputX = new int[input.GetLength(0) * multiplier, input.GetLength(1) * multiplier];
            for (int row = 0; row < inputX.GetLength(0); row++)
            {
                for (int col = 0; col < inputX.GetLength(1); col++)
                {
                    var offset = row / input.GetLength(0) + col / input.GetLength(1);
                    var oldVal = input[row % input.GetLength(0), col % input.GetLength(1)];
                    inputX[row, col] = ((oldVal + offset) % 10) + (oldVal + offset > 9 ? 1 : 0);
                }
            }
            return GetShortestPath(inputX);
        }

        public override int[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D().CharToIntGrid2D();
        }

        private long GetShortestPath(int[,] input)
        {
            var distances = input.Dijkstra((0, 0));
            return distances[(input.GetLength(0) - 1, input.GetLength(1) - 1)].Item1;
        }
    }
}
