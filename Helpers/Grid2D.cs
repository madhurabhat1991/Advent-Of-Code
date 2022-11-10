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

        public static (T, int, int) GetRightElement<T>(this T[,] input, int row, int col)
        {
            int nextRow = row, nextCol = col + 1;
            return (input[nextRow, nextCol], nextRow, nextCol);
        }

        public static (T, int, int) GetBottomElement<T>(this T[,] input, int row, int col)
        {
            int nextRow = row + 1, nextCol = col;
            return (input[nextRow, nextCol], nextRow, nextCol);
        }

        public static (T, int, int) GetLeftElement<T>(this T[,] input, int row, int col)
        {
            int nextRow = row, nextCol = col - 1;
            return (input[nextRow, nextCol], nextRow, nextCol);
        }
    }
}
