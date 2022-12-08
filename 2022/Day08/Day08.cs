using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2022.Day08
{
    public class Day08 : Day<int[,], long, long>
    {
        public override string DayNumber { get { return "08"; } }

        public override long PartOne(int[,] input)
        {
            var dict = DetermineVisibility(input);
            // count edges
            long count = input.GetLength(0) * 2 + (input.GetLength(1) - 2) * 2;
            // count visible trees
            count += dict.Count(r => r.Value.Item1 == true);
            return count;
        }

        public override long PartTwo(int[,] input)
        {
            var dict = DetermineVisibility(input);
            return dict.Max(r => r.Value.Item2);
        }

        public override int[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D().CharToIntGrid2D();
        }

        /// <summary>
        /// Determine the visibility of the trees
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Dictionary<(row, col), (visible, view)></returns>
        private Dictionary<(int, int), (bool, long)> DetermineVisibility(int[,] input)
        {
            Dictionary<(int, int), (bool, long)> dict = new Dictionary<(int, int), (bool, long)>();
            for (int row = 1; row < input.GetLength(0) - 1; row++)
            {
                for (int col = 1; col < input.GetLength(1) - 1; col++)
                {
                    List<bool> visibilities = new List<bool>();
                    List<long> views = new List<long>();

                    // top
                    bool visible = true;
                    long view = 0;
                    for (int i = row - 1; i >= 0; i--)
                    {
                        view++;
                        if (visible && input[i, col] >= input[row, col])
                        {
                            visible = false;
                            break;
                        }
                    }
                    visibilities.Add(visible);
                    views.Add(view);

                    // right
                    visible = true;
                    view = 0;
                    for (int i = col + 1; i < input.GetLength(1); i++)
                    {
                        view++;
                        if (visible && input[row, i] >= input[row, col])
                        {
                            visible = false;
                            break;
                        }
                    }
                    visibilities.Add(visible);
                    views.Add(view);

                    // bottom
                    visible = true;
                    view = 0;
                    for (int i = row + 1; i < input.GetLength(0); i++)
                    {
                        view++;
                        if (visible && input[i, col] >= input[row, col])
                        {
                            visible = false;
                            break;
                        }
                    }
                    visibilities.Add(visible);
                    views.Add(view);

                    // left
                    visible = true;
                    view = 0;
                    for (int i = col - 1; i >= 0; i--)
                    {
                        view++;
                        if (visible && input[row, i] >= input[row, col])
                        {
                            visible = false;
                            break;
                        }
                    }
                    visibilities.Add(visible);
                    views.Add(view);

                    dict[(row, col)] = (visibilities.Any(r => r == true), views.Aggregate((a, x) => a * x));
                }
            }
            return dict;
        }
    }
}
