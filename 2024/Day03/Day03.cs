using System;
using System.Collections.Generic;
using System.Text;
using Template;

namespace _2024.Day03
{
    public class Day03 : Day<string, long, long>
    {
        public override string DayNumber { get { return "03"; } }

        public override long PartOne(string input)
        {
            long sum = 0;
            var i = 0;
            while (i < input.Length)
            {
                sum += ParseMul(input, ref i);
                i++;
            }
            return sum;
        }

        public override long PartTwo(string input)
        {
            long sum = 0;
            bool multiply = true;
            var i = 0;
            while (i < input.Length)
            {
                ParseDoOrDont(input, ref i, dont, ref multiply);
                ParseDoOrDont(input, ref i, @do, ref multiply);
                if (multiply)
                {
                    sum += ParseMul(input, ref i);
                }
                i++;
            }
            return sum;
        }

        public override string ProcessInput(string[] input)
        {
            return String.Join("", input);
        }

        private readonly string mul = "mul(";
        private readonly string @do = "do()";
        private readonly string dont = "don't()";
        private readonly int digitLen = 3;

        /// <summary>
        /// Parse mul(123,123) from input string at given index
        /// </summary>
        /// <param name="input"></param>
        /// <param name="i"></param>
        /// <returns>Product of 2 numbers if parsed</returns>
        private long ParseMul(string input, ref int i)
        {
            long product = 0;
            if (i + mul.Length - 1 < input.Length && input.Substring(i, mul.Length) == mul)
            {
                i += mul.Length;
                var closingIndex = input.IndexOf(')', i);
                if (closingIndex - i <= digitLen * 2 + 1)   // two three-digit and a comma
                {
                    var op = input[i..closingIndex];
                    if (op.Contains(',') && !op.Contains(' '))
                    {
                        var nums = op.Split(",");
                        if (int.TryParse(nums[0], out int num1))
                        {
                            if (int.TryParse(nums[1], out int num2))
                            {
                                product = num1 * num2;
                                i = closingIndex;
                            }
                        }
                    }
                }
            }
            return product;
        }

        /// <summary>
        /// Parse do() or don't() from input string at given index and determine whether to multiply
        /// </summary>
        /// <param name="input"></param>
        /// <param name="i"></param>
        /// <param name="doOrDont"></param>
        /// <param name="multiply"></param>
        /// <returns>True if do, false if don't, same as multiply otherwise</returns>
        private void ParseDoOrDont(string input, ref int i, string doOrDont, ref bool multiply)
        {
            if (i + doOrDont.Length - 1 < input.Length && (input.Substring(i, doOrDont.Length) == doOrDont))
            {
                multiply = doOrDont != dont;
                i += doOrDont.Length;
            }
        }
    }
}
