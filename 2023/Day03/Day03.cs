using System;
using System.Collections.Generic;
using System.Text;
using Template;
using Helpers;
using System.Linq;

namespace _2023.Day03
{
    public class Day03 : Day<char[,], long, long>
    {
        public override string DayNumber { get { return "03"; } }

        public override long PartOne(char[,] input)
        {
            return FindSum(input).Item1;
        }

        public override long PartTwo(char[,] input)
        {
            return FindSum(input).Item2;
        }

        public override char[,] ProcessInput(string[] input)
        {
            return input.CreateGrid2D();
        }

        /// <summary>
        /// Find sum of part numbers (Part 1) and sum of gear ratios (Part 2)
        /// </summary>
        /// <param name="input"></param>
        /// <returns>(Part Sum, Gear Sum)</returns>
        private (long, long) FindSum(char[,] input)
        {
            long partSum = 0;
            Dictionary<(int, int), List<long>> gears = new Dictionary<(int, int), List<long>>();
            for (int r = 0; r < input.GetLength(0); r++)
            {
                string numberStr = "";
                bool partNumber = false;
                (char, int, int) symbol = ('.', 0, 0);
                for (int c = 0; c < input.GetLength(1); c++)
                {
                    if (Char.IsDigit(input[r, c]))
                    {
                        numberStr += input[r, c];
                        // check if any neighbors of the cell is a symbol = part number
                        var neighbors = input.GetNeighbors(r, c, true);
                        if (!partNumber && !(neighbors.All(r => Char.IsDigit(r.Item1) || r.Item1 == '.')))
                        {
                            partNumber = true;
                            symbol = neighbors.First(r => !(Char.IsDigit(r.Item1) || r.Item1 == '.'));
                        }
                        // check if the number has ended - last cell in the row or if next cell is not digit
                        var rightCell = (c < input.GetLength(1) - 1) ? input.GetRightCell(r, c) : ('.', 0, 0);
                        if (c == input.GetLength(1) - 1 || !Char.IsDigit(rightCell.Item1))
                        {
                            if (partNumber)
                            {
                                long numberVal = Int64.Parse(numberStr);
                                // calculate sum of part numbers
                                partSum += numberVal;
                                // store index of gears and their part numbers in dict
                                if (symbol.Item1 == '*')
                                {
                                    List<long> pNums = gears.ContainsKey((symbol.Item2, symbol.Item3)) ? gears[(symbol.Item2, symbol.Item3)] : new List<long>();
                                    pNums.Add(numberVal);
                                    gears[(symbol.Item2, symbol.Item3)] = pNums;
                                }
                                partNumber = false;
                            }
                            numberStr = "";
                        }
                    }
                }
            }
            // calculate gear ratios and their sum
            long gearSum = gears.Where(kvp => kvp.Value.Count == 2).Select(kvp => kvp.Value).Sum(l => l.Aggregate((a, x) => a * x));
            return (partSum, gearSum);
        }
    }
}
