using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers
{
    public static class Grid2D
    {
        /// <summary>
        /// Create 2D array
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Char[,] CreateGrid2D(this String[] input)
        {
            Char[,] array = new Char[input.Length, input[0].Length];
            for (int row = 0; row < input.Length; row++)
            {
                for (int col = 0; col < input[row].Length; col++)
                {
                    array[row, col] = input[row][col];
                }
            }
            return array;
        }

        /// <summary>
        /// Convert Char 2D array to Int 2D array
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Int32[,] CharToIntGrid2D(this Char[,] input)
        {
            Int32[,] array = new Int32[input.GetLength(0), input.GetLength(1)];
            for (int row = 0; row < input.GetLength(0); row++)
            {
                for (int col = 0; col < input.GetLength(1); col++)
                {
                    array[row, col] = Int32.Parse(input[row, col].ToString());
                }
            }
            return array;
        }

        public static (T, int, int) GetTopElement<T>(this T[,] input, int row, int col)
        {
            int nextRow = row - 1, nextCol = col;
            return (input[nextRow, nextCol], nextRow, nextCol);
        }

        public static (T, int, int) GetTopRightElement<T>(this T[,] input, int row, int col)
        {
            int nextRow = row - 1, nextCol = col + 1;
            return (input[nextRow, nextCol], nextRow, nextCol);
        }

        public static (T, int, int) GetRightElement<T>(this T[,] input, int row, int col)
        {
            int nextRow = row, nextCol = col + 1;
            return (input[nextRow, nextCol], nextRow, nextCol);
        }

        public static (T, int, int) GetBottomRightElement<T>(this T[,] input, int row, int col)
        {
            int nextRow = row + 1, nextCol = col + 1;
            return (input[nextRow, nextCol], nextRow, nextCol);
        }

        public static (T, int, int) GetBottomElement<T>(this T[,] input, int row, int col)
        {
            int nextRow = row + 1, nextCol = col;
            return (input[nextRow, nextCol], nextRow, nextCol);
        }

        public static (T, int, int) GetBottomLeftElement<T>(this T[,] input, int row, int col)
        {
            int nextRow = row + 1, nextCol = col - 1;
            return (input[nextRow, nextCol], nextRow, nextCol);
        }

        public static (T, int, int) GetLeftElement<T>(this T[,] input, int row, int col)
        {
            int nextRow = row, nextCol = col - 1;
            return (input[nextRow, nextCol], nextRow, nextCol);
        }

        public static (T, int, int) GetTopLeftElement<T>(this T[,] input, int row, int col)
        {
            int nextRow = row - 1, nextCol = col - 1;
            return (input[nextRow, nextCol], nextRow, nextCol);
        }

        /// <summary>
        /// Get the adjacent neighbors of an element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="includeDiagonal">true if diagonally adjacent neighbors are to be included, false otherwise</param>
        /// <returns>(Neighbor's element, row, column)</returns>
        public static List<(T, int, int)> FindNeighbors<T>(this T[,] input, int row, int col, bool includeDiagonal)
        {
            List<(T, int, int)> neighbors = new List<(T, int, int)>();
            (T, int, int) neighbor;

            // top
            if (row > 0)
            {
                neighbor = input.GetTopElement(row, col);
                neighbors.Add((neighbor.Item1, neighbor.Item2, neighbor.Item3));
            }
            // top right
            if (includeDiagonal && row > 0 && col < input.GetLength(1) - 1)
            {
                neighbor = input.GetTopRightElement(row, col);
                neighbors.Add((neighbor.Item1, neighbor.Item2, neighbor.Item3));
            }
            // right
            if (col < input.GetLength(1) - 1)
            {
                neighbor = input.GetRightElement(row, col);
                neighbors.Add((neighbor.Item1, neighbor.Item2, neighbor.Item3));
            }
            // bottom right
            if (includeDiagonal && row < input.GetLength(0) - 1 && col < input.GetLength(1) - 1)
            {
                neighbor = input.GetBottomRightElement(row, col);
                neighbors.Add((neighbor.Item1, neighbor.Item2, neighbor.Item3));
            }
            // bottom
            if (row < input.GetLength(0) - 1)
            {
                neighbor = input.GetBottomElement(row, col);
                neighbors.Add((neighbor.Item1, neighbor.Item2, neighbor.Item3));
            }
            // bottom left
            if (includeDiagonal && row < input.GetLength(0) - 1 && col > 0)
            {
                neighbor = input.GetBottomLeftElement(row, col);
                neighbors.Add((neighbor.Item1, neighbor.Item2, neighbor.Item3));
            }
            // left
            if (col > 0)
            {
                neighbor = input.GetLeftElement(row, col);
                neighbors.Add((neighbor.Item1, neighbor.Item2, neighbor.Item3));
            }
            // top left
            if (includeDiagonal && row > 0 && col > 0)
            {
                neighbor = input.GetTopLeftElement(row, col);
                neighbors.Add((neighbor.Item1, neighbor.Item2, neighbor.Item3));
            }

            return neighbors;
        }
    }
}
