using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;
using Helpers;

namespace _2021.Day13
{
    public class Day13 : Day<(Char[,], List<(char, int)>), long, object>
    {
        public override string DayNumber { get { return "13"; } }

        public override long PartOne((Char[,], List<(char, int)>) input)
        {
            return (long)Origami(input, false);
        }

        public override object PartTwo((Char[,], List<(char, int)>) input)
        {
            return Origami(input, true);
        }

        public override (Char[,], List<(char, int)>) ProcessInput(string[] input)
        {
            List<(int, int)> dots = new List<(int, int)>();
            while (input[0].Any())
            {
                var both = input[0].Split(",").ToList();
                dots.Add((Int32.Parse(both.First()), Int32.Parse(both.Last())));
                input = input.Skip(1).ToArray();
            }
            var grid = dots.MarkGrid(Mark, Unmark);
            //grid.Print();
            input = input.Skip(1).ToArray();

            List<(char, int)> folds = new List<(char, int)>();
            while (input.Any())
            {
                var both = input[0].Split("=").ToList();
                folds.Add((both.First().Last(), Int32.Parse(both.Last())));
                input = input.Skip(1).ToArray();
            }
            return (grid, folds);
        }

        private Char[,] Input { get; set; }

        private const char XAxis = 'x';
        private const char YAxis = 'y';
        private const char Mark = '#';
        private const char Unmark = '.';

        private object Origami((Char[,], List<(char, int)>) input, bool complete)
        {
            Input = input.Item1;
            //Input.Print();
            foreach (var fold in input.Item2)
            {
                Input = FoldPaper(Input, fold.Item1, fold.Item2);
                //Input.Print();
                if (!complete) { return (long)Input.GetCellsEqualToValue(Mark).Count; }
            }
            if (complete) { Input.Print(); }
            return null;
        }

        private char[,] FoldPaper(char[,] input, char axis, int value)
        {
            char[,] result = new char[0, 0];
            switch (axis)
            {
                case XAxis:
                    result = new char[input.GetLength(0), value];
                    for (int row = 0; row < result.GetLength(0); row++)
                    {
                        for (int col = 0; col < result.GetLength(1); col++)
                        {
                            result[row, col] = input[row, col] == Mark || input[row, input.GetLength(1) - 1 - col] == Mark ? Mark : Unmark;
                        }
                    }
                    break;
                case YAxis:
                    result = new char[value, input.GetLength(1)];
                    for (int row = 0; row < result.GetLength(0); row++)
                    {
                        for (int col = 0; col < result.GetLength(1); col++)
                        {
                            result[row, col] = input[row, col] == Mark || input[input.GetLength(0) - 1 - row, col] == Mark ? Mark : Unmark;
                        }
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
