﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Helpers
{
    public static class Grid2DExtensions
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

        /// <summary>
        /// Get top cell of a cell
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>(Cell's value, row, column)</returns>
        public static (T, int, int) GetTopCell<T>(this T[,] input, int row, int col)
        {
            int nextRow = row - 1, nextCol = col;
            return (input[nextRow, nextCol], nextRow, nextCol);
        }

        /// <summary>
        /// Get top right cell of a cell
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>(Cell's value, row, column)</returns>
        public static (T, int, int) GetTopRightCell<T>(this T[,] input, int row, int col)
        {
            int nextRow = row - 1, nextCol = col + 1;
            return (input[nextRow, nextCol], nextRow, nextCol);
        }

        /// <summary>
        /// Get right cell of a cell
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>(Cell's value, row, column)</returns>
        public static (T, int, int) GetRightCell<T>(this T[,] input, int row, int col)
        {
            int nextRow = row, nextCol = col + 1;
            return (input[nextRow, nextCol], nextRow, nextCol);
        }

        /// <summary>
        /// Get bottom right cell of a cell
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>(Cell's value, row, column)</returns>
        public static (T, int, int) GetBottomRightCell<T>(this T[,] input, int row, int col)
        {
            int nextRow = row + 1, nextCol = col + 1;
            return (input[nextRow, nextCol], nextRow, nextCol);
        }

        /// <summary>
        /// Get bottom cell of a cell
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>(Cell's value, row, column)</returns>
        public static (T, int, int) GetBottomCell<T>(this T[,] input, int row, int col)
        {
            int nextRow = row + 1, nextCol = col;
            return (input[nextRow, nextCol], nextRow, nextCol);
        }

        /// <summary>
        /// Get bottom left cell of a cell
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>(Cell's value, row, column)</returns>
        public static (T, int, int) GetBottomLeftCell<T>(this T[,] input, int row, int col)
        {
            int nextRow = row + 1, nextCol = col - 1;
            return (input[nextRow, nextCol], nextRow, nextCol);
        }

        /// <summary>
        /// Get left cell of a cell
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>(Cell's value, row, column)</returns>
        public static (T, int, int) GetLeftCell<T>(this T[,] input, int row, int col)
        {
            int nextRow = row, nextCol = col - 1;
            return (input[nextRow, nextCol], nextRow, nextCol);
        }

        /// <summary>
        /// Get top left cell of a cell
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>(Cell's value, row, column)</returns>
        public static (T, int, int) GetTopLeftCell<T>(this T[,] input, int row, int col)
        {
            int nextRow = row - 1, nextCol = col - 1;
            return (input[nextRow, nextCol], nextRow, nextCol);
        }

        /// <summary>
        /// Get the adjacent neighbors of a cell
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="includeDiagonal">true if diagonally adjacent neighbors are to be included, false otherwise</param>
        /// <returns>List<(Neighbor's value, row, column)></returns>
        public static List<(T, int, int)> GetNeighbors<T>(this T[,] input, int row, int col, bool includeDiagonal)
        {
            List<(T, int, int)> neighbors = new List<(T, int, int)>();
            (T, int, int) neighbor;

            // top
            if (row > 0)
            {
                neighbor = input.GetTopCell(row, col);
                neighbors.Add((neighbor.Item1, neighbor.Item2, neighbor.Item3));
            }
            // top right
            if (includeDiagonal && row > 0 && col < input.GetLength(1) - 1)
            {
                neighbor = input.GetTopRightCell(row, col);
                neighbors.Add((neighbor.Item1, neighbor.Item2, neighbor.Item3));
            }
            // right
            if (col < input.GetLength(1) - 1)
            {
                neighbor = input.GetRightCell(row, col);
                neighbors.Add((neighbor.Item1, neighbor.Item2, neighbor.Item3));
            }
            // bottom right
            if (includeDiagonal && row < input.GetLength(0) - 1 && col < input.GetLength(1) - 1)
            {
                neighbor = input.GetBottomRightCell(row, col);
                neighbors.Add((neighbor.Item1, neighbor.Item2, neighbor.Item3));
            }
            // bottom
            if (row < input.GetLength(0) - 1)
            {
                neighbor = input.GetBottomCell(row, col);
                neighbors.Add((neighbor.Item1, neighbor.Item2, neighbor.Item3));
            }
            // bottom left
            if (includeDiagonal && row < input.GetLength(0) - 1 && col > 0)
            {
                neighbor = input.GetBottomLeftCell(row, col);
                neighbors.Add((neighbor.Item1, neighbor.Item2, neighbor.Item3));
            }
            // left
            if (col > 0)
            {
                neighbor = input.GetLeftCell(row, col);
                neighbors.Add((neighbor.Item1, neighbor.Item2, neighbor.Item3));
            }
            // top left
            if (includeDiagonal && row > 0 && col > 0)
            {
                neighbor = input.GetTopLeftCell(row, col);
                neighbors.Add((neighbor.Item1, neighbor.Item2, neighbor.Item3));
            }

            return neighbors;
        }

        /// <summary>
        /// Get the adjacent neighbors of List of cells
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="cells">List<(Cell's value, row, column)></param>
        /// <param name="includeDiagonal">true if diagonally adjacent neighbors are to be included, false otherwise</param>
        /// <returns>List<(Neighbor's value, row, column)></returns>
        public static List<(T, int, int)> GetNeighbors<T>(this T[,] input, List<(T, int, int)> cells, bool includeDiagonal)
        {
            List<(T, int, int)> neighbors = new List<(T, int, int)>();
            foreach (var cell in cells)
            {
                neighbors.AddRange(input.GetNeighbors(cell.Item2, cell.Item3, includeDiagonal));
            }
            return neighbors;
        }

        /// <summary>
        /// Get the adjacent neighbors of List of cells
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="cells">List<(Cell's row, column)></param>
        /// <param name="includeDiagonal">true if diagonally adjacent neighbors are to be included, false otherwise</param>
        /// <returns>List<(Neighbor's value, row, column)></returns>
        public static List<(T, int, int)> GetNeighbors<T>(this T[,] input, List<(int, int)> cells, bool includeDiagonal)
        {
            List<(T, int, int)> neighbors = new List<(T, int, int)>();
            foreach (var cell in cells)
            {
                neighbors.AddRange(input.GetNeighbors(cell.Item1, cell.Item2, includeDiagonal));
            }
            return neighbors;
        }

        /// <summary>
        /// Get all cells equal to given value
        /// </summary>
        /// <param name="input"></param>
        /// <param name="value"></param>
        /// <returns>List<(Cell's value, row, column)></returns>
        public static List<(T, int, int)> GetCellsEqualToValue<T>(this T[,] input, T value)
        {
            List<(T, int, int)> cells = new List<(T, int, int)>();

            for (int row = 0; row < input.GetLength(0); row++)
            {
                for (int col = 0; col < input.GetLength(1); col++)
                {
                    if (EqualityComparer<T>.Default.Equals(input[row, col], value))
                    {
                        cells.Add((input[row, col], row, col));
                    }
                }
            }
            return cells;
        }

        /// <summary>
        /// Get all cells greater than given value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="value"></param>
        /// <returns>List<(Cell's value, row, column)></returns>
        public static List<(T, int, int)> GetCellsGreaterThanValue<T>(this T[,] input, T value)
        {
            List<(T, int, int)> cells = new List<(T, int, int)>();

            for (int row = 0; row < input.GetLength(0); row++)
            {
                for (int col = 0; col < input.GetLength(1); col++)
                {
                    dynamic dx = input[row, col], dy = value;
                    if (dx > dy)
                    {
                        cells.Add((input[row, col], row, col));
                    }
                }
            }
            return cells;
        }

        /// <summary>
        /// Get all cells lesser than given value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="value"></param>
        /// <returns>List<(Cell's value, row, column)></returns>
        public static List<(T, int, int)> GetCellsLesserThanValue<T>(this T[,] input, T value)
        {
            List<(T, int, int)> cells = new List<(T, int, int)>();

            for (int row = 0; row < input.GetLength(0); row++)
            {
                for (int col = 0; col < input.GetLength(1); col++)
                {
                    dynamic dx = input[row, col], dy = value;
                    if (dx < dy)
                    {
                        cells.Add((input[row, col], row, col));
                    }
                }
            }
            return cells;
        }

        /// <summary>
        /// Set given cells of grid by given value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="cells">List<(Cell's value, row, column)></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T[,] SetCellsToValue<T>(this T[,] input, List<(T, int, int)> cells, T value)
        {
            foreach (var cell in cells)
            {
                input[cell.Item2, cell.Item3] = value;
            }
            return input;
        }

        /// <summary>
        /// Set given cells of grid by given value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="cells">List<(Cell's row, column)></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T[,] SetCellsToValue<T>(this T[,] input, List<(int, int)> cells, T value)
        {
            foreach (var cell in cells)
            {
                input[cell.Item1, cell.Item2] = value;
            }
            return input;
        }

        /// <summary>
        /// Increment every value of grid by given value
        /// </summary>
        /// <param name="input"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T[,] IncrementValue<T>(this T[,] input, T value)
        {
            for (int row = 0; row < input.GetLength(0); row++)
            {
                for (int col = 0; col < input.GetLength(1); col++)
                {
                    dynamic dx = input[row, col], dy = value;
                    input[row, col] = dx + dy;
                }
            }
            return input;
        }

        /// <summary>
        /// Increment value of given cells of grid by given value
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cells">List<(Cell's element, row, column)></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T[,] IncrementValue<T>(this T[,] input, List<(T, int, int)> cells, T value)
        {
            foreach (var cell in cells)
            {
                dynamic dx = input[cell.Item2, cell.Item3], dy = value;
                input[cell.Item2, cell.Item3] = dx + dy;
            }
            return input;
        }

        /// <summary>
        /// Increment value of given cells of grid by given value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="cells">List<(Cell's row, column)></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T[,] IncrementValue<T>(this T[,] input, List<(int, int)> cells, T value)
        {
            foreach (var cell in cells)
            {
                dynamic dx = input[cell.Item1, cell.Item2], dy = value;
                input[cell.Item1, cell.Item2] = dx + dy;
            }
            return input;
        }

        /// <summary>
        /// Print the grid content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        public static void Print<T>(this T[,] input)
        {
            for (int row = 0; row < input.GetLength(0); row++)
            {
                for (int col = 0; col < input.GetLength(1); col++)
                {
                    Console.Write(input[row, col] + (typeof(T) == typeof(Char) ? "" : "\t"));
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }

        /// <summary>
        /// Mark the grid by given mark positions and character
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">List<Tuple<x, y>></param>
        /// <param name="mark"></param>
        /// <param name="mark">unmark</param>
        /// <returns></returns>
        public static T[,] MarkGrid<T>(this List<Tuple<int, int>> input, T mark, T unmark)
        {
            T[,] array = new T[input.Select(r => r.Item2).Max() + 1, input.Select(r => r.Item1).Max() + 1];
            foreach (var pair in input)
            {
                array[pair.Item2, pair.Item1] = mark;
            }
            for (int row = 0; row < array.GetLength(0); row++)
            {
                for (int col = 0; col < array.GetLength(1); col++)
                {
                    if (!EqualityComparer<T>.Default.Equals(array[row, col], mark))
                    {
                        array[row, col] = unmark;
                    }
                }
            }
            return array;
        }
    }
}